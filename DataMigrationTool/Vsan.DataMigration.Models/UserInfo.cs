using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models
{
    public class UserInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool IsLock { get; set; }
    }
}
