using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.View
{
    public class MethodView
    {
        public long Id { get; set; }
        public string TypeCode { get; set; }
        public string AssemblyPath { get; set; }
        public string TypeFillName { get; set; }
        public string MethodName { get; set; }
        public bool IsStatic { get; set; }
        public string Params { get; set; }
        public string ReturnType { get; set; }
        public string Description { get; set; }
       
    }
}
