using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vsan.Scheduling.Server.api.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Logo { get; set; }
        public bool IsLock { get; internal set; }
    }
}