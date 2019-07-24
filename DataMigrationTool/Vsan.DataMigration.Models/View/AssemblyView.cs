using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.View
{
    public class AssemblyView
    {
        public string Assembly { get; set; }

        public IEnumerable<TypeView> Types { get; set; }
      

    }

    public class TypeView
    {
        public string FullName { get; set; }

        public IEnumerable<MethodSelectView>  Methods{ get; set; }

}

    public class MethodSelectView
    {

        public string MethodName { get; set; }

        public bool IsStatic { get; set; }

    }
}
