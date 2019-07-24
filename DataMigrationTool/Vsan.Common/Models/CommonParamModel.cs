using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vsan.Common
{
    /// <summary>
    /// 基础的传参模型
    /// </summary>
    public class CommonParamModel
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        [Required(ErrorMessage = "timestamp是必须的")]
        public string timestamp { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        [Required(ErrorMessage = "token是必须的")]
        public string token { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [Required(ErrorMessage = "sign是必须的")]
        public string sign { get; set; }

    }
}