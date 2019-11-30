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

    /// <summary>
    /// 问题组管理
    /// </summary>
    public class QuerstionGroupController : ApiController
    {
        private DataMigrationEntities db = new DataMigrationEntities();


        /// <summary>
        /// 获取上传过的程序集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/QuerstionGroup/GetSelectOption")]
        public JsonDataModel<IEnumerable<object>> GetSelectOption()
        {
            var deletedState = (int)DataState.Deleted;
            var query = db.qa_questions_group.AsNoTracking().Where(a => a.Status != deletedState)
                .Select(a=>new {
                    id=a.Id,
                    text=a.Name
                }).ToArray();

            return new JsonDataModel<IEnumerable<object>>(query);

        }



        // GET: api/QuerstionGroup
        public PageIQueryableResult<qa_questions_group> Getqa_questions_group([FromUri]PageParam param)
        {
            var deletedState = (int)DataState.Deleted;

            var query = db.qa_questions_group.AsNoTracking().Where(a => a.Status != deletedState);

            if (!string.IsNullOrEmpty(param.keyword))
            {
                query = query.Where(a => a.Name.Contains(param.keyword));
            }

            var count = query.Count();

            var skip = (param.page - 1) * param.limit;

            var list = query.OrderByDescending(a => a.CreateTime).Skip(() => skip).Take(param.limit);


            var result = new PageIQueryableResult<qa_questions_group>
            {
                code = 0,
                message = "获取方法成功",
                count = count,
                data = list,
            };
            return result;
        }

        // GET: api/QuerstionGroup/5
        [ResponseType(typeof(qa_questions_group))]
        public IHttpActionResult Getqa_questions_group(string id)
        {
            qa_questions_group qa_questions_group = db.qa_questions_group.Find(id);
            if (qa_questions_group == null)
            {
                return NotFound();
            }

            return Ok(qa_questions_group);
        }

        // PUT: api/QuerstionGroup/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putqa_questions_group(string id, qa_questions_group qa_questions_group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qa_questions_group.Id)
            {
                return BadRequest();
            }

            db.Entry(qa_questions_group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!qa_questions_groupExists(id))
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

        // POST: api/QuerstionGroup
        [ResponseType(typeof(qa_questions_group))]
        public IHttpActionResult Postqa_questions_group(qa_questions_group qa_questions_group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.qa_questions_group.Any(a => a.Name == qa_questions_group.Name))
            {
                throw new Exception($"问题组'{qa_questions_group.Name}'已存在请勿重复添加");
            };
            qa_questions_group.Id = Guid.NewGuid().ToString();
            qa_questions_group.LastUpdateUser = qa_questions_group.Creater = "1";
            qa_questions_group.LastUpdateUserName = qa_questions_group.CreaterName = "测试";
            qa_questions_group.LastUpdateTime = qa_questions_group.CreateTime = DateTime.Now;

            qa_questions_group.Status = (sbyte)DataState.Normal;
            qa_questions_group.StatusDesc = DataState.Normal.GetDescription();


            db.qa_questions_group.Add(qa_questions_group);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (qa_questions_groupExists(qa_questions_group.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = qa_questions_group.Id }, qa_questions_group);
        }

        // DELETE: api/QuerstionGroup/5
        [ResponseType(typeof(qa_questions_group))]
        public IHttpActionResult Deleteqa_questions_group(string id)
        {
            qa_questions_group qa_questions_group = db.qa_questions_group.Find(id);
            if (qa_questions_group == null)
            {
                return NotFound();
            }
            qa_questions_group.Status=(sbyte)DataState.Deleted;
            qa_questions_group.LastUpdateTime = DateTime.Now;

            db.SaveChanges();

            return Ok(qa_questions_group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool qa_questions_groupExists(string id)
        {
            return db.qa_questions_group.Count(e => e.Id == id) > 0;
        }
    }
}