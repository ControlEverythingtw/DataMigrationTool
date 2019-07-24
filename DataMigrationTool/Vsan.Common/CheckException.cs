using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 检查异常
    /// </summary>
    public class CheckException : Exception
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public ResultCode ResultCode { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="message">消息</param>
        public CheckException(string message) : base(message)
        {
            this.ResultCode = ResultCode.ArgumentException;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="resultCode">返回码</param>
        /// <param name="message">消息</param>
        public CheckException(ResultCode resultCode,string message):base(message)
        {
            this.ResultCode = resultCode;
           
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="resultCode">返回码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public CheckException(ResultCode resultCode, string message, object data) : base(message)
        {
            this.ResultCode = resultCode;
            this.data = data;
        }

    }
}
