using System;
using System.Threading.Tasks;
using System.Web.Http;
using Vsan.Scheduling.Server.api.Models;
using Hangfire;
using Hangfire.Logging;
using Microsoft.Owin;
using Owin;
using Topshelf;
[assembly: OwinStartup(typeof(Vsan.Scheduling.Server.api.Startup))]

namespace Vsan.Scheduling.Server.api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{Controller}/{id}",
               defaults: new { id = RouteParameter.Optional });

            app.UseWebApi(config);

            app.UseRedis(ServerConfig.Config.DataBase.ConnectionString);

            app.UseNLog();

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Queues = new[] { "critical", "default" },
            });

            app.UseHangfireDashboard();

        }
    }
}
