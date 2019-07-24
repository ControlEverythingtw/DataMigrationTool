using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.Param
{

    /// <summary>
    /// 批量操作参数
    /// </summary>
    public class BatchOptionParam
    {
        public long[] Ids { get; set; }

        public int Option { get; set; }

    }
}
