//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vsan.DataMigration.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class qa_score
    {
        public string Id { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Creater { get; set; }
        public string CreaterName { get; set; }
        public System.DateTime LastUpdateTime { get; set; }
        public string LastUpdateUser { get; set; }
        public string LastUpdateUserName { get; set; }
        public sbyte Status { get; set; }
        public string StatusDesc { get; set; }
        public long Sort { get; set; }
        public string tz_id { get; set; }
        public string tz_name { get; set; }
        public int score { get; set; }
        public string dtr_id { get; set; }
    }
}