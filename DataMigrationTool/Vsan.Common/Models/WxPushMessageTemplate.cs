using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Models
{
    /// <summary>
    /// 微信推送消息模板
    /// </summary>
    public class WxMessageTemplate
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 模板编号
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// 跳转的页面
        /// </summary>
        public string Page { get; set; }
        public int ParamCount { get; internal set; }
    }
}
