using Vsan.Scheduling.Server.api;
using Hangfire.Logging;
using Microsoft.Owin.Hosting;
using System;
using Vsan.Scheduling.Server.api.Models;
using Topshelf;

namespace Vsan.Scheduling.Server
{
    public class Bootstrap : ServiceControl
    {
        private static readonly ILog _logger = LogProvider.For<Bootstrap>();
        private IDisposable webApp;
        public string Address { get; set; }
        public bool Start(HostControl hostControl)
        {
            try
            {
                webApp = WebApp.Start<Startup>(Address);
                return true;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Topshelf starting occured errors.", ex);
                return false;
            }

        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                webApp?.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                _logger.ErrorException($"Topshelf stopping occured errors.", ex);
                return false;
            }

        }
    }
}
