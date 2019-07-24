using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.Param
{
    public class DataSourceQueryParam:PageParam
    {

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
        public string type { get; set; }

    }
}
