using System;
using Topshelf;
using static Vsan.Scheduling.Server.api.Models.ServerConfig;

namespace Vsan.Scheduling.Server
{
    class Program
    {
        static int Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();

                x.SetServiceName(Config.ServerName);
                x.SetDisplayName(Config.DisplayName);
                x.SetDescription(Config.Description);

                if (Config.IsAuto)
                {
                    x.StartAutomatically();
                }

                x.UseNLog();
                x.UseOwin(baseAddress: Config.ApiUrl);

                x.SetStartTimeout(TimeSpan.FromMinutes(5));
                //https://github.com/Topshelf/Topshelf/issues/165
                x.SetStopTimeout(TimeSpan.FromMinutes(35));

                x.EnableServiceRecovery(r => { r.RestartService(1); });
            });
        }
    }
}
