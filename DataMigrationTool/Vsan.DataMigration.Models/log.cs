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
    
    public partial class log
    {
        public long Id { get; set; }
        public string Creator { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string TypeCode { get; set; }
        public string GroupCode { get; set; }
        public string Remake { get; set; }
        public string OperationObjectId { get; set; }
        public long OperationObjectIntId { get; set; }
    }
}