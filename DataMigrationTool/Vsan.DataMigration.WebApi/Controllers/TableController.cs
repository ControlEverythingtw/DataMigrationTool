using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vsan.DataMigration.Core;
using Vsan.DataMigration.Models;
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.WebApi.Controllers
{
    public class TableController : ApiController
    {

        IDbHelper db = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db?.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// 获取Table集合根据数据源
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public IEnumerable<TableModel> GetTables([FromUri]data_source data_source)
        {

            db=DbFactory.GetDbHelper(data_source.TypeCode, data_source.Link);

            var table= db.GetTables(data_source.DbName);

            return table;

        }


    }
}
