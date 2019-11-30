using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models
{
    public  enum DataState
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted =-1,

        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal =0,



    }
}
