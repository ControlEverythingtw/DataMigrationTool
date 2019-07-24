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
using Vsan.DataMigration.Core;
using Vsan.DataMigration.Models;
using Vsan.DataMigration.Models.Param;
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.WebApi.Controllers
{

    /// <summary>
    /// 工单
    /// </summary>
    //[AuthFilter]
    public class WorkOrderController : ApiController
    {
        private DataMigrationEntities db = new DataMigrationEntities();

        // GET: api/WorkOrder
        [HttpGet]
        public PageIQueryableResult<work_order> GetPageList([FromUri]PageParam param)
        {

            var deletedState = (int)DataState.Deleted;

            var query = db.work_order.AsNoTracking().Where(a => a.State != deletedState);

            if (!string.IsNullOrEmpty(param.keyword))
            {
                query = query.Where(a => a.Remake.Contains(param.keyword) || a.InportSourceName.Contains(param.keyword) || a.ExportSourceName.Contains(param.keyword));
            }

            var count = query.Count();

            var skip = (param.page - 1) * param.limit;

            var list = query.OrderByDescending(a => a.CreateTime).Skip(() => skip).Take(param.limit);


            var result = new PageIQueryableResult<work_order>
            {
                code = 0,
                message = "获取工单成功",
                count = count,
                data = list,
            };
            return result;

        }

        // GET: api/WorkOrder/5
        [ResponseType(typeof(work_order))]
        public IHttpActionResult Getwork_order(long id)
        {
            work_order work_order = db.work_order.Find(id);
            if (work_order == null)
            {
                return NotFound();
            }

            return Ok(work_order);
        }

        // PUT: api/WorkOrder/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putwork_order(long id, work_order work_order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != work_order.Id)
            {
                return BadRequest();
            }

            db.Entry(work_order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!work_orderExists(id))
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


        [Route("api/work_order/restart")]
        [HttpGet]
        public string ReStart(string id,int index=1,int size=100, int count=1000)
        {
            var work_order = db.work_order.FirstOrDefault(a => a.OrderId == id);

            var from = db.data_source.Find(work_order.InportSourceId);
            var to = db.data_source.Find(work_order.ExportSourceId);
            var fieldMapping =
                (from a in db.field_mapping
                    join b in db.method on a.MethodId equals b.Id
                    where a.OrderId == work_order.OrderId && a.Creator == "1"
                    select new FieldMappingItem()
                    {
                        field = a.FieldIn,
                        toField = a.FieldOut,
                        method = (int)b.Id,
                        methodClassName = b.TypeFillName,
                        methodDll = b.AssemblyPath,
                        methodName = b.MethodName,
                    }).ToList();


            if (from == null || to == null || fieldMapping.Count <= 0)
            {
                throw new CheckException("配置不完善");
            }

            var param = new TransferParam
            {
                OrderId = work_order.OrderId,
                FromConnString = @from.Link,
                FromTable = @from.TableName,
                FromTypeCode = @from.TypeCode,
                Where = work_order.InportWhere,
                OrderBy = work_order.InportOrderByField,
                ToConnString = to.Link,
                ToTypeCode = to.TypeCode,
                ToTable = to.TableName,
                Index= index,
                Size = size,
                Count = count,
                FieldMapping = fieldMapping
            };

            TransferEngine.Start(param);


            return "Ok";

        }


        // POST: api/WorkOrder
        [ResponseType(typeof(work_order))]
        public IHttpActionResult Postwork_order(work_order work_order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.field_mapping.Any(a=>a.OrderId==work_order.OrderId&&a.State==0))
            {
                throw  new Exception("请配置字段映射关系");
            }

            var runState = 1;
            if (db.work_order.Any(a=>a.InportSourceId==work_order.InportSourceId&&a.ExportSourceId==work_order.ExportSourceId&&a.State== runState))
            {
                throw new Exception("已存在正在运行的迁移策略.");
            }

            if (work_order.StartTime==default(DateTime))
            {
                work_order.StartTime=DateTime.Now;
            }

            if (work_order.PageIndex<=0)
            {
                work_order.PageIndex = 1;
            }
            work_order.CreateTime=DateTime.Now;
            work_order.UserId = 1;
            work_order.Creator = "1";
            work_order.EndTime=DateTime.Now;
            work_order.InportWhere = work_order.InportWhere ?? string.Empty;
            work_order.Remake = work_order.Remake?? string.Empty ;
            work_order.InportOrderByField = work_order.InportOrderByField ?? string.Empty ;

            work_order.Modifier = "1";
            work_order.ModifyTime=DateTime.Now;
            work_order.Exception = string.Empty;

            db.work_order.Add(work_order);
            db.SaveChanges();

            var from = db.data_source.Find(work_order.InportSourceId);
            var to = db.data_source.Find(work_order.ExportSourceId);
            var fieldMapping =
               ( from a in db.field_mapping
                join b in db.method on a.MethodId equals b.Id
                where a.OrderId == work_order.OrderId && a.Creator == "1"
                select new FieldMappingItem()
                {
                    field=a.FieldIn,
                    toField = a.FieldOut,
                    method=(int)b.Id,
                    methodClassName =b.TypeFillName,
                    methodDll = b.AssemblyPath,
                    methodName = b.MethodName,
                }).ToList();
               

            if (from==null||to==null||fieldMapping.Count<=0)
            {
                throw new CheckException("配置不完善");
            }

            var param = new TransferParam
            {
                OrderId = work_order.OrderId,
                FromConnString = @from.Link,
                FromTable = @from.TableName,
                FromTypeCode = @from.TypeCode,
                Where = work_order.InportWhere,
                OrderBy = work_order.InportOrderByField,
                ToConnString = to.Link,
                ToTypeCode = to.TypeCode,
                ToTable = to.TableName,
                Size = work_order.PageSize,
                Count = (int) work_order.DataCount,
                FieldMapping = fieldMapping
            };

            TransferEngine.Start(param);

            return CreatedAtRoute("DefaultApi", new { id = work_order.Id }, work_order);
        }

        // DELETE: api/WorkOrder/5
        [ResponseType(typeof(work_order))]
        public IHttpActionResult Deletework_order(long id)
        {
            work_order work_order = db.work_order.Find(id);
            if (work_order == null)
            {
                return NotFound();
            }

            db.work_order.Remove(work_order);
            db.SaveChanges();

            return Ok(work_order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool work_orderExists(long id)
        {
            return db.work_order.Count(e => e.Id == id) > 0;
        }
    }
}