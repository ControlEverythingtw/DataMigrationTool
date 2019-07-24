using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.WebApi.Models
{
    public class JsonResult
    {
        public int code { get; set; }
        public string message { get; set; }

        public object data { get; set; }

        public JsonResult(int code, string message, object data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }

        public JsonResult(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public JsonResult()
        {
        }

        public static JsonResult Ok { get; set; } = new JsonResult
        {
            code = 0,
            message = "success"
        };

        public static JsonResult GetOk(object data)
        {
            return new JsonResult
            {
                code = 0,
                message = "success",
                data = data,
            };
        }

        public static JsonResult Fail(object data)
        {
            return new JsonResult
            {
                code = 1,
                message = "Fail",
                data = data,
            };
        }
    }
}
