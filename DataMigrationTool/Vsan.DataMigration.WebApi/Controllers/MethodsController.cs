using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [Description("方法")]
    public class MethodsController : ApiController
    {
        private DataMigrationEntities db = new DataMigrationEntities();

        // GET: api/DataSource
        [HttpGet]
        public PageIQueryableResult<method> GetPageList([FromUri]PageParam param)
        {
            var deletedState = (int)DataState.Deleted;

            var query = db.method.AsNoTracking().Where(a => a.State != deletedState);

            if (!string.IsNullOrEmpty(param.keyword))
            {
                query = query.Where(a => a.MethodName.Contains(param.keyword)||a.Description.Contains(param.keyword));
            }

            var count = query.Count();

            var skip = (param.page - 1) * param.limit;

            var list = query.OrderByDescending(a => a.CreateTime).Skip(() => skip).Take(param.limit);


            var result = new PageIQueryableResult<method>
            {
                code = 0,
                message = "获取方法成功",
                count = count,
                data = list,
            };
            return result;
        }

        // GET: api/Methods/5
        [ResponseType(typeof(method))]
        public IHttpActionResult Getmethod(long id)
        {
            method method = db.method.Find(id);
            if (method == null)
            {
                return NotFound();
            }

            return Ok(method);
        }

        // PUT: api/Methods/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putmethod(long id, method method)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != method.Id)
            {
                return BadRequest();
            }

            db.Entry(method).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!methodExists(id))
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

        // POST: api/Methods
        [ResponseType(typeof(method))]
        public IHttpActionResult Postmethod(method method)
        {
            var userId = "1";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isExist = db.method.Any(a => a.Creator == userId && a.MethodName == method.MethodName && a.TypeFillName == method.TypeFillName && a.AssemblyPath == method.AssemblyPath);

            if (isExist)
            {
               throw new Exception("方法已存在请勿重复添加");
            }
            method.Creator = userId;
            method.CreateTime = DateTime.Now;
            method.Modifier = userId;
            method.ModifyTime = DateTime.Now;
            method.Params = string.Empty;
            method.ReturnType = string.Empty;
            method.TypeCode = string.Empty;

            db.method.Add(method);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = method.Id }, method);
        }

        // DELETE: api/Methods/5
        [ResponseType(typeof(method))]
        public IHttpActionResult Deletemethod(long id)
        {
            method method = db.method.Find(id);
            if (method == null)
            {
                return NotFound();
            }

            db.method.Remove(method);
            db.SaveChanges();

            return Ok(method);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool methodExists(long id)
        {
            return db.method.Count(e => e.Id == id) > 0;
        }
    }
}