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
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.WebApi.Controllers
{
    public class QuerstionController : ApiController
    {
        private DataMigrationEntities db = new DataMigrationEntities();

        // GET: api/Querstion
        public IQueryable<qa_questions> Getqa_questions()
        {



            return db.qa_questions;
        }
        // GET: api/Querstion/5
        public List<qa_questionsView> Getqa_questions(string id)
        {
           var all = db.qa_questions.Where(a=>a.tz_id==id&&a.Status==0).OrderByDescending(a=>a.Sort)
                .Select(a => new qa_questionsView { id = a.Id, title = a.text+"_"+a.score,parent=a.parent }).ToList();
            var view = all.Where(a => a.parent == string.Empty).ToList();
            ToView(all,view);
            return view;
        }

        private  void ToView(List<qa_questionsView> all,List<qa_questionsView> view)
        {
            foreach (var item in view)
            {
                item.children= all.Where(a => a.parent == item.id).ToList();
                if (item.children!=null&&item.children.Count>0)
                {
                    ToView(all,item.children);
                }
            }
        }




        // PUT: api/Querstion/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putqa_questions(string id, qa_questions qa_questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qa_questions.Id)
            {
                return BadRequest();
            }

            db.Entry(qa_questions).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!qa_questionsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Querstion
        [ResponseType(typeof(qa_questions))]
        public IHttpActionResult Postqa_questions(qa_questions qa_questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrWhiteSpace(qa_questions.parent))
            {
                qa_questions.parent = string.Empty;
            }
            if (db.qa_questions.Any(a=>a.text==qa_questions.text&&a.parent==qa_questions.parent&&a.tz_id==qa_questions.tz_id&&a.Status==0))
            {
                throw new CheckException("变量名称已存在");
            }
            qa_questions.Id = Guid.NewGuid().ToString();
            qa_questions.LastUpdateUser = qa_questions.Creater = "1";
            qa_questions.LastUpdateUserName = qa_questions.CreaterName = "测试";
            qa_questions.LastUpdateTime = qa_questions.CreateTime = DateTime.Now;
            qa_questions.Status = (sbyte)DataState.Normal;
            qa_questions.StatusDesc = DataState.Normal.GetDescription();
            qa_questions.type = 0;
            qa_questions.typeDesc = "选项";
            qa_questions.Path = string.Empty;
           

           db.qa_questions.Add(qa_questions);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (qa_questionsExists(qa_questions.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = qa_questions.Id }, qa_questions);
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