using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Models
{
    public class ShenFenResult
    {
        public int code { get; set; }
        public ShenFenResultData data { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }
    }

    public class ShenFenResultData
    {
        public long Id { get; set; }
        public string orderNo { get; set; }
        public Nullable<System.DateTime> handleTime { get; set; }
        public string result { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string birthday { get; set; }
        public Nullable<int> age { get; set; }
        public Nullable<int> gender { get; set; }
        public string remark { get; set; }
      
    }
}
