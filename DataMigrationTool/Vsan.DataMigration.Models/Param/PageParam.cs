using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.Param
{
    public class PageParam
    {
        public int page { get; set; }
        public int limit { get; set; }
        public string keyword { get; set; }

    }
}
