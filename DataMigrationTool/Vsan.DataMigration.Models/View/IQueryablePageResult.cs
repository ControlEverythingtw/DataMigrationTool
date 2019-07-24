using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.View
{
    public class PageIQueryableResult<T>
    {

        public int code { get; set; }

        public string message { get; set; }

        public int count { get; set; }

        public IQueryable<T> data { get; set; }

    }
}
