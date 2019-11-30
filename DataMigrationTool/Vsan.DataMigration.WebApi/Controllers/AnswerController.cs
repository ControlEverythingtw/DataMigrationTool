using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vsan.Common;
using Vsan.DataMigration.Models;
using Vsan.DataMigration.Models.Param;
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.WebApi.Controllers
{
    public class AnswerController : ApiController
    {
        private DataMigrationEntities db = new DataMigrationEntities();
       

        private void GetIds(List<qa_questionsView> data,List<string> ids)
        {
            foreach (var item in data)
            {
                ids.Add(item.id);
                if (item.children!=null&&item.children.Count>0)
                {
                    GetIds(item.children, ids);
                }
            }
        }


        // POST: api/Querstion
        [ResponseType(typeof(qa_score))]
        public IHttpActionResult Post(AnswerParam param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = db.qa_questions_group.FirstOrDefault(a => a.Id == param.Select_Group);
            if (group==null)
            {
                throw new CheckException("group不存在");
            }

            var qData = JsonConvert.DeserializeObject<List<qa_questionsView>>(param.SelectVar);
            var ids = new List<string>();

            GetIds(qData, ids);

            List<qa_answer> qas = new List<qa_answer>();
            foreach (var item in ids)
            {
                var q = db.qa_questions.FirstOrDefault(a=>a.Id==item);
                if (q!=null)
                {
                    var qa = new qa_answer();
                    qa.Id = Guid.NewGuid().ToString();
                    qa.LastUpdateUser = qa.Creater = "1";
                    qa.LastUpdateUserName = qa.CreaterName = "测试";
                    qa.LastUpdateTime = qa.CreateTime = DateTime.Now;
                    qa.Status = (sbyte)DataState.Normal;
                    qa.StatusDesc = DataState.Normal.GetDescription();
                    qa.LastUpdateUser = param.Select_Group;
                    qa.wt_id = q.Id;
                    qa.wt_score = q.score;
                    qa.dtr_id= param.UserName;
                    qas.Add(qa);
                }

            }
            db.qa_answer.AddRange(qas);
            var score=qas.Sum(a=>a.wt_score);

            var qa_score = new qa_score();
            qa_score.Id = Guid.NewGuid().ToString();
            qa_score.LastUpdateUser = qa_score.Creater = "1";
            qa_score.LastUpdateUserName = qa_score.CreaterName = param.UserName;
            qa_score.LastUpdateTime = qa_score.CreateTime = DateTime.Now;
            qa_score.Status = (sbyte)DataState.Normal;
            qa_score.StatusDesc = DataState.Normal.GetDescription();
            qa_score.score = score;
            qa_score.tz_id = param.Select_Group;
            qa_score.tz_name = group.Name;
            qa_score.dtr_id = param.UserName;
            db.qa_score.Add(qa_score);


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (qa_questionsExists(qa_score.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = qa_score.Id }, param);
        }

        // DELETE: api/Querstion/5
        [ResponseType(typeof(qa_questions))]
        public IHttpActionResult Deleteqa_questions(string id)
        {
            var ids = id.Split(',');
            var qa_questions = db.qa_questions.Where(a=> ids.Any(b=>b==a.Id));
            if (qa_questions == null)
            {
                return NotFound();
            }
            foreach (var item in qa_questions)
            {
                item.Status = (sbyte)DataState.Deleted;
                item.LastUpdateTime = DateTime.Now;
            }
            db.SaveChanges();

            return Ok(qa_questions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool qa_questionsExists(string id)
        {
            return db.qa_questions.Count(e => e.Id == id) > 0;
        }
    }
}