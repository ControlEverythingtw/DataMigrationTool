using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models
{
    /// <summary>
    /// 返回结果模型
    /// </summary>
    public class ReturnResult
    {
        public int code { get; set; }
        public string message { get; set; }

        public object data { get; set; }

        public ReturnResult(int code, string message, object data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }

        public ReturnResult(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public ReturnResult()
        {
        }

        public static ReturnResult Ok { get; set; } = new ReturnResult
        {
            code = 0,
            message = "success"
        };

        public static ReturnResult GetOk(object data)
        {
            return new ReturnResult
            {
                code = 0,
                message = "success",
                data = data,
            };
        }

        public static ReturnResult Fail(object data)
        {
            return new ReturnResult
            {
                code = 1,
                message = "Fail",
                data = data,
            };
        }
    }
}
