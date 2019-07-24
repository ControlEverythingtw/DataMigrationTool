using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vsan.DataMigration.Models.Param;

namespace Vsan.DataMigration.Core
{
    public class SqlBuilder
    {

        public static string GetInsertSql(DataTable table, string toTable, List<FieldMappingItem> fileMapping,string mark1,string mark2)
        {
            var rows = table.Rows;
            var cols = table.Columns;
            var sb = new StringBuilder();
            sb.Append("Insert Into ");
            sb.Append(mark1);
            sb.Append(toTable);
            sb.Append(mark2);
            sb.Append("(");
            //设置字段在row objectArray中的位置
            for (int i = 0; i < fileMapping.Count; i++)
            {
                var item = fileMapping[i];

                for (int j = 0; j < cols.Count; j++)
                {
                    if (cols[j].ColumnName.ToUpper().Equals(item.field?.ToUpper()))
                    {
                        item.Index = j;
                    }
                }
                sb.Append(mark1);
                sb.Append(item.toField);
                sb.Append(mark2);
                if (i < fileMapping.Count - 1)
                {
                    sb.Append(',');
                }
            }

            sb.Append(") Values ");

            for (int i = 0; i < rows.Count; i++)
            {
                var objs = rows[i].ItemArray;
                sb.Append("(");
                for (int j = 0; j < fileMapping.Count; j++)
                {
                    var field = fileMapping[j];
                    var value = objs[field.Index];

                    if (field.method != default(int))
                    {
                        var method = MethodContainer.GetMethodInfo(field.method, field.methodDll, field.methodClassName,
                            field.methodName);
                        object instance = null;
                        //是否为静态方法(静态方法不需要实例调用)
                        if (!method.IsStatic)
                        {
                            //创建该对象的实例，object类型，参数（名称空间 + 类）   
                            if (method.ReflectedType != null)
                                instance = method.ReflectedType.Assembly.CreateInstance(field.methodClassName);
                        }
                        value = method.Invoke(instance, new object[] { value });
                    }
                    sb.Append('\'');
                    sb.Append(value);
                    sb.Append('\'');
                    if (j < fileMapping.Count - 1)
                    {
                        sb.Append(',');
                    }
                }
                sb.Append(")");
                if (i < rows.Count - 1)
                {
                    sb.Append(',');
                }
            }

            return sb.ToString();
        }


    }
}
