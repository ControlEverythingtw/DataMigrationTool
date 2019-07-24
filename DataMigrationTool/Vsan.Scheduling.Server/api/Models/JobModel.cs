using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Scheduling.Server.api.Models
{

    public enum JobType
    {
        /// <summary>
        /// 一次
        /// </summary>
        once,

        /// <summary>
        /// 循环周期
        /// </summary>
        period

    }
    /// <summary>
    /// 方法模型
    /// </summary>
    public  class JobModel
    {
        /// <summary>
        /// 职位编号
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// 工作类型 （once=一次,）
        /// </summary>
        public JobType JobType { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 多少秒后执行
        /// </summary>
        public double Seconds { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 请求的地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string Token { get; set; } = null;

        /// <summary>
        /// 请求内容类型
        /// </summary>
        public string ContentType { get; set; } = "application/json";

       

    }
}
