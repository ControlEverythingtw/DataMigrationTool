using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.View
{
    public class FieldModel
    {
       
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldComment { get; set; }
        public string FieldType { get; set; }
        public long FieldLength { get; set; }
    }
}
