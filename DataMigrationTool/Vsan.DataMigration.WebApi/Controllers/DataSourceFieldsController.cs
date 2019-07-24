using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using Vsan.DataMigration.Models;
using Vsan.DataMigration.Models.Param;
using Vsan.DataMigration.Models.View;
using System.Web.Http.Description;

namespace Vsan.DataMigration.WebApi.Controllers
{
    public class DataSourceFieldsController : ApiController
    {
        private DataMigrationEntities db = new DataMigrationEntities();



        // GET: api/DataSource
        [HttpGet]
        public IQueryable<data_source_fields> GetByDataSourceId(int id)
        {
            var query = db.data_source_fields.AsQueryable().Where(a => a.UserId == 1 && a.DataSourceId == id);

            return query;
        }

        [Route("api/Fields/{id}")]
        [HttpGet]
        public PageIQueryableResult<object> GetFields(int id)
        {
            var query = db.data_source_fields.AsNoTracking()
                .Where(a => a.UserId == 1 && a.DataSourceId == id).Select(a=>new
                {
                    Id= a.FieldName,
                    Text= a.FieldComment,
                    Type= a.FieldType,
                });

            var result = new PageIQueryableResult<object>
            {
                data = query
            };
            return result;
        }



        [HttpPost]
        [Route("api/DataSourceFields/AddComment")]
        public bool AddComment(AddCommentParam param)
        {
            var fields = db.data_source_fields.FirstOrDefault(a => param.FieldId==a.Id);
            if (fields == null)
            {
                return false;
            }

            fields.FieldComment = param.Comment;

            db.SaveChanges();

            return true;
        }



        [HttpPost]
        [Route("api/DataSourceFields/DeleteBatch")]
        public bool DeleteBatch(BatchOptionParam param)
        {
            var fields = db.data_source_fields.Where(a=> param.Ids.Any(b=>b==a.Id));
            if (fields == null)
            {
                return false;
            }
            db.data_source_fields.RemoveRange(fields);
            db.SaveChanges();

            return true;
        }

        // DELETE: api/DataSource/5
        [ResponseType(typeof(data_source_fields))]
        public IHttpActionResult Deletedata_source(long id)
        {
            data_source_fields data_source = db.data_source_fields.Find(id);
            if (data_source == null)
            {
                return NotFound();
            }
            db.data_source_fields.Remove(data_source);
            db.SaveChanges();

            return Ok(data_source);
        }

        // GET: api/DataSource
        [HttpGet]
        public PageIQueryableResult<data_source_fields> GetPageList([FromUri]FieldsQueryParam param)
        {
            var query = db.data_source_fields.AsQueryable().Where(a => a.UserId == 1);

            if (!string.IsNullOrEmpty(param.keyword))
            {
                query = query.Where(a => a.FieldName.Contains(param.keyword) || a.FieldName.Contains(param.keyword));
            }
            if (param.dataSource.HasValue)
            {
                query = query.Where(a => a.DataSourceId == param.dataSource.Value);
            }

            var count = query.Count();

            var skip = (param.page - 1) * param.limit;

            var list = query.OrderByDescending(a => a.Id).Skip(() => skip).Take(param.limit);


            var result = new PageIQueryableResult<data_source_fields>
            {
                code = 0,
                message = "获取数据源成功",
                count = count,
                data = list,
            };
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
