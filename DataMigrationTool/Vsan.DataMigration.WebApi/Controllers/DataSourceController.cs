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
using Vsan.DataMigration.Core;
using Vsan.DataMigration.Models;
using Vsan.DataMigration.Models.Param;
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.WebApi.Controllers
{
    //[AuthFilter]
    public class DataSourceController : ApiController
    {
        private DataMigrationEntities db = new DataMigrationEntities();
        [Route("api/dataSource/selectData")]
        [HttpGet]
        public PageIQueryableResult<object> GetSelectData()
        {
            var deletedState = (int)DataState.Deleted;
            var userId = 1;
            var query = db.data_source.AsNoTracking().Where(a => a.State != deletedState&&a.UserId== userId).Select(a=>new 
            {
                a.Id,
                a.DataSourceName,
                a.TableName,
                a.TypeCode,
                a.Link,
            });
            var result=new PageIQueryableResult<object>();
            result.code = 0;
            result.data = query;
            return result;
        }
        // GET: api/DataSource
        [HttpGet]
        public PageIQueryableResult<data_source> GetPageList([FromUri]DataSourceQueryParam param)
        {
            var deletedState = (int)DataState.Deleted;
            var userId = 1;
            var query = db.data_source.AsNoTracking().Where(a=>a.State!= deletedState && a.UserId == userId);

            if (!string.IsNullOrEmpty(param.keyword))
            {
                query = query.Where(a=>a.Host.Contains(param.keyword));
            }
            if (param.startTime.HasValue)
            {
                query = query.Where(a=>a.CreateTime>=param.startTime.Value);
            }
            if (param.endTime.HasValue)
            {
                query = query.Where(a => a.CreateTime <= param.endTime.Value);
            }
            if (!string.IsNullOrEmpty(param.type))
            {
                query = query.Where(a => a.TypeCode == param.type);
            }

            var count = query.Count();

            var skip = (param.page - 1) * param.limit;

            var list = query.OrderByDescending(a => a.CreateTime).Skip(() => skip).Take(param.limit);
            

            var result = new PageIQueryableResult<data_source>
            {
                code=0,
                message="获取数据源成功",
                count=count,
                data=list,
            };
            return  result ;
        }

        // GET: api/DataSource/5
        [HttpGet]
        [Route("api/DataSource/IsNameExist/{name}")]
        public bool IsNameExist(string name)
        {
           return db.data_source.Any(a=>a.DataSourceName==name);
        }


        // GET: api/DataSource/5
        [ResponseType(typeof(data_source))]
        public IHttpActionResult Getdata_source(long id)
        {
            data_source data_source = db.data_source.Find(id);
            if (data_source == null)
            {
                return NotFound();
            }

            return Ok(data_source);
        }

        // PUT: api/DataSource/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdata_source(long id, data_source data_source)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.data_source.Any(a => a.DataSourceName == data_source.DataSourceName))
            {
                return BadRequest(ModelState);
            }

            if (id != data_source.Id)
            {
                return BadRequest();
            }

            db.Entry(data_source).State = EntityState.Modified;

            try
            {
                var count = db.SaveChanges();

                if (count>0)
                {
                   
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!data_sourceExists(id))
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

        // POST: api/DataSource
        [ResponseType(typeof(data_source))]
        public IHttpActionResult Postdata_source(data_source data_source)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            data_source.CreateTime = DateTime.Now;
            data_source.Creator = "self";
            data_source.Modifier = "self";
            data_source.ModifyTime = DateTime.Now;
            data_source.UserId = 1;
            db.data_source.Add(data_source);

            try
            {
                db.SaveChanges();
                var dbHelper = DbFactory.GetDbHelper(data_source.TypeCode, data_source.Link);

                var nowDate = DateTime.Now;

                var fields = dbHelper.GetFields(data_source.TableName).Select(a => new data_source_fields
                {
                    DataSourceId = data_source.Id,
                    DataSourceName = data_source.DataSourceName,
                    FieldComment = a.FieldComment,
                    FieldLength = a.FieldLength,
                    FieldName = a.FieldName,
                    FieldType = a.FieldType,
                    TableName = a.TableName,
                    UserId = 1,
                    UserName = "vsan",
                });

                db.data_source_fields.AddRange(fields);

                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (data_sourceExists(data_source.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = data_source.Id }, data_source);
        }

        // DELETE: api/DataSource/5
        [ResponseType(typeof(data_source))]
        public IHttpActionResult Deletedata_source(long id)
        {
            data_source data_source = db.data_source.Find(id);
            if (data_source == null)
            {
                return NotFound();
            }
            data_source.State = (int)DataState.Deleted;
            db.SaveChanges();

            return Ok(data_source);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool data_sourceExists(long id)
        {
            return db.data_source.Count(e => e.Id == id) > 0;
        }
    }
}