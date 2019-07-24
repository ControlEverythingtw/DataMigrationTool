using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Server;
using Hangfire.SqlServer;
using Owin;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Vsan.Scheduling.Server
{
    /// <summary>
    /// Hangfire配置扩展
    /// </summary>
    public static class HangfireExtensions
    {
        /// <summary>
        /// 使用Owin
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        public static HostConfigurator UseOwin(this HostConfigurator configurator, string baseAddress)
        {
            if (string.IsNullOrEmpty(baseAddress))
                throw new ArgumentNullException(nameof(baseAddress));

            configurator.Service(() => new Bootstrap { Address = baseAddress });

            return configurator;
        }

        /// <summary>
        /// 使用SqlServer
        /// </summary>
        /// <param name="app"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IGlobalConfiguration UseSqlServer(this IAppBuilder app, string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            return Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);
        }

        /// <summary>
        /// 使用Redis
        /// </summary>
        /// <param name="app"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IGlobalConfiguration UseRedis(this IAppBuilder app, string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            return Hangfire.GlobalConfiguration.Configuration.UseRedisStorage(connectionString);
        }

        /// <summary>
        /// 使用NLog记录日志
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IGlobalConfiguration UseNLog(this IAppBuilder app)
        {
            return Hangfire.GlobalConfiguration.Configuration.UseNLogLogProvider();
        }
    }
}
