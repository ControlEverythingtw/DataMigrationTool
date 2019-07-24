using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Scheduling.Server.api.Models
{
    /// <summary>
    /// 数据库模型
    /// </summary>
    public class DataBaseModel
    {
        /// <summary>
        /// 数据库类型编号
        /// </summary>
        public int TypeId { get; set; } = 0;
        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public string TypeName { get; set; } = "Redis";
        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnectionString { get; set; } = "127.0.0.1:6379,defaultDatabase=1,password=496988878";

    }
}
