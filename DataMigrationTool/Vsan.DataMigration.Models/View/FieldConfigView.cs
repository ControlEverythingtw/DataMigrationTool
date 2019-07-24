using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.View
{
    public class FieldConfigView
    {
        public long? Id { get; set; }
        public string FieldIn { get; set; }
        public string FieldOut { get; set; }
        public IQueryable<MethodView> Methods { get; set; }
        public long InportSourceId { get; set; }
        public string FieldComment { get; set; }
        public string FieldType { get; set; }
    }
}
