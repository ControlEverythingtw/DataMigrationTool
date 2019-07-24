using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.Param
{
    public class FieldsQueryParam:PageParam
    {
        /// <summary>
        /// 数据源编号
        /// </summary>
        public int? dataSource { get; set; }

    }
}
