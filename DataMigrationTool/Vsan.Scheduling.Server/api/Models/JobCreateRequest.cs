using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwy.Service.JobSystem.api.Models
{
    /// <summary>
    /// 作业创建请求
    /// </summary>
    public class JobCreateRequest
    {
        /// <summary>
        /// 作业类型
        /// </summary>
        public string jobType { get; set;}

        /// <summary>
        /// 多少秒后执行
        /// </summary>
        public int seconds { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string param { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        public string contentType { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string token { get; set; }
    }
}
