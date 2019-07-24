using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Models
{
    /// <summary>
    /// 兑吧回调返回参数
    /// </summary>
    public  class DuiBaResultModel
    {

        /// <summary>
        /// 状态 (ok=成功  ; fail=失败 )
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string errorMessage { get; set; }
        /// <summary>
        /// 积分(工分)
        /// </summary>
        public long credits { get; set; }
        /// <summary>
        /// 开发者订单号
        /// </summary>
        public string bizId { get; set; }


    }
}
