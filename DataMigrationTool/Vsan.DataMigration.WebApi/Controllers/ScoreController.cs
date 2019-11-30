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
using Vsan.DataMigration.Models;
using Vsan.DataMigration.Models.Param;
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.WebApi.Controllers
{
    public class ScoreController : ApiController
    {
        private DataMigrationEntities db = new DataMigrationEntities();

        // GET: api/Score
        public PageIQueryableResult<qa_score> GetPageList([FromUri]PageParam param)
        {
            var deletedState = (int)DataState.Deleted;

            var query = db.qa_score.AsNoTracking().Where(a => a.Status != deletedState);

            if (!string.IsNullOrEmpty(param.keyword))
            {
                query = query.Where(a => a.CreaterName.Contains(param.keyword) || a.tz_name.Contains(param.keyword));
            }

            var count = query.Count();

            var skip = (param.page - 1) * param.limit;

            var list = query.OrderByDescending(a => a.CreateTime).Skip(() => skip).Take(param.limit);


            var result = new PageIQueryableResult<qa_score>
            {
                code = 0,
                message = "获取qa_score成功",
                count = count,
                data = list,
            };
            return result;
        }

        // GET: api/Score/5
        [ResponseType(typeof(qa_score))]
        public IHttpActionResult Getqa_score(string id)
        {
            qa_score qa_score = db.qa_score.Find(id);
            if (qa_score == null)
            {
                return NotFound();
            }

            return Ok(qa_score);
        }

        // PUT: api/Score/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putqa_score(string id, qa_score qa_score)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qa_score.Id)
            {
                return BadRequest();
            }

            db.Entry(qa_score).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!qa_scoreExists(id))
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

        // POST: api/Score
        [ResponseType(typeof(qa_score))]
        public IHttpActionResult Postqa_score(qa_score qa_score)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.qa_score.Add(qa_score);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (qa_scoreExists(qa_score.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = qa_score.Id }, qa_score);
        }

        // DELETE: api/Score/5
        [ResponseType(typeof(qa_score))]
        public IHttpActionResult Deleteqa_score(string id)
        {
            qa_score qa_score = db.qa_score.Find(id);
            if (qa_score == null)
            {
                return NotFound();
            }

            db.qa_score.Remove(qa_score);
            db.SaveChanges();

            return Ok(qa_score);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool qa_scoreExists(string id)
        {
            return db.qa_score.Count(e => e.Id == id) > 0;
        }
    }
}