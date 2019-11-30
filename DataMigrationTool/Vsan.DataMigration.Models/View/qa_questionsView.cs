using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models.View
{
    public class qa_questionsView
    {
        public string id { get; set; }
        public string title { get; set; }
        public string parent { get; set; }
        public bool spread { get; set; } = true;

        public List<qa_questionsView> children { get; set; }


    }
}
