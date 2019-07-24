using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwy.Service.JobSystem.api.Models
{
    public class WebApiResult
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }

    public class WebApiResult<T> : WebApiResult where T : class
    {
        public T data { get; set; }
    }
}
