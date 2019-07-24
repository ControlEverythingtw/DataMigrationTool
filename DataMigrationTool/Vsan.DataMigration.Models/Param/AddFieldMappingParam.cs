using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.Param
{
    public class AddFieldMappingParam
    {
        public string orderId { get; set; }
        public List<FieldMappingItem> fieldMapping { get; set; }
}

    public class FieldMappingItem
    {
        public string field { get; set; }
        public int method { get; set; }
        public string methodDll { get; set; }
        public string methodClassName { get; set; }
        public string methodName { get; set; }

        public string toField { get; set; }
        public int Index { get; set; }
    }
}

