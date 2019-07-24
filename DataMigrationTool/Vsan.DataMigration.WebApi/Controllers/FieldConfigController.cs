using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vsan.DataMigration.Models;
using Vsan.DataMigration.Models.Param;
using Vsan.DataMigration.Models.View;
using System.Data.Entity;

namespace Vsan.DataMigration.WebApi.Controllers
{

    /// <summary>
    /// 字段配置
    /// </summary>
    public class FieldConfigController : ApiController
    {

        private DataMigrationEntities db = new DataMigrationEntities();


        #region 获取字段配置

        public PageIQueryableResult<FieldConfigView> GetFieldConfigViews([FromUri]FieldsQueryParam param)
        {

            var queryable =
                from e in db.data_source_fields
                join d in db.work_order on e.DataSourceId equals d.InportSourceId into dd
                from d in dd.DefaultIfEmpty()
                join a in db.field_mapping on d.OrderId equals a.OrderId into aa
                from a1 in aa.DefaultIfEmpty()
                select new FieldConfigView
                {
                    Id = a1.Id,
                    FieldIn = a1.FieldIn ?? e.FieldName,
                    FieldComment = e.FieldComment,
                    FieldType = e.FieldType,
                    FieldOut = a1.FieldOut,
                    InportSourceId = e.DataSourceId,
                    Methods = (
                     from b in db.field_method_mapping
                     join c in db.method on b.MethodId equals c.Id
                     where b.FieldConfigId == a1.Id && a1 != null
                     select new MethodView
                     {
                         Id = c.Id,
                         AssemblyPath = c.AssemblyPath,
                         Description = c.Description,
                         IsStatic = c.IsStatic,
                         MethodName = c.MethodName,
                         Params = c.Params,
                         ReturnType = c.ReturnType,
                         TypeCode = c.TypeCode,
                         TypeFillName = c.TypeFillName
                     }
                    )
                };

            if (param.dataSource.HasValue)
            {
                queryable = queryable.Where(a => a.InportSourceId == param.dataSource);
            }

            var count = queryable.Count();
            var skip = (param.page - 1) * param.limit;

            var pageList = queryable.OrderByDescending(a => a.Id).Skip(() => skip).Take(param.limit);


            var result = new PageIQueryableResult<FieldConfigView>
            {
                code = 0,
                message = "获取工单成功",
                count = count,
                data = pageList,
            };
            return result;


        }

        #endregion


        #region 添加字段配置


        // POST: api/Methods
        [ResponseType(typeof(field_mapping))]
        public IHttpActionResult Postmethod(AddFieldMappingParam param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var field_Mappings = new List<field_mapping>();

            foreach (var item in param.fieldMapping)
            {
                var field_mapping = new field_mapping()
                {
                    OrderId = param.orderId,
                    Creator = "1",
                    CreateTime = DateTime.Now,
                    TypeCode = string.Empty,
                    MethodCount = 1,
                    MethodId = item.method,
                    ModifyTime = DateTime.Now,
                    Modifier = "1",
                    FieldIn = item.field,
                    FieldOut = item.toField,
                };
                field_Mappings.Add(field_mapping);
            }

            db.field_mapping.AddRange(field_Mappings);
            db.SaveChanges();

            return Ok();
        }



        #endregion


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
