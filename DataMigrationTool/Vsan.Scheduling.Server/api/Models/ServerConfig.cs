using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vsan.Scheduling.Server.api.Helper;

namespace Vsan.Scheduling.Server.api.Models
{
    /// <summary>
    /// 服务配置模型
    /// </summary>
    public class ServerConfig
    {
        public static ServerConfig Config = ConfigHelper.Get<ServerConfig>();
        public static void LoadConfig()
        {
            Config = ConfigHelper.Get<ServerConfig>();
        }
        public static void Save()
        {
            ConfigHelper.Save(Config);
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServerName { get; set; } = "GwyJobSvc";

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; } = "Gwy Job Server ";

        /// <summary>
        /// 服务的描述
        /// </summary>
        public string Description { get; set; } = "工务园作业调度服务";


        /// <summary>
        /// 是否自动启动
        /// </summary>
        public bool IsAuto { get; set; } = true;

        /// <summary>
        /// Api地址
        /// </summary>
        public string ApiUrl { get; set; } = "http://localhost:9005";
        /// <summary>
        /// 接口访问令牌
        /// </summary>
        public string ApiToken { get; set; } = "40CB348F7EF54449A63EC909074EA14B";



        /// <summary>
        /// 数据库配置
        /// </summary>

        public DataBaseModel DataBase = new DataBaseModel();

    }

}
