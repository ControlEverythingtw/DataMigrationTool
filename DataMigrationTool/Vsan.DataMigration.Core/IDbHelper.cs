using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vsan.DataMigration.Models.Param;
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.Core
{
    public interface IDbHelper:IDisposable
    {

        /// <summary>
        /// 设置链接串
        /// </summary>
        /// <param name="link"></param>
        void SetLink(string link);

        /// <summary>
        /// 获取某个数据库下的所有表
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        IEnumerable<TableModel> GetTables(string dbName);


        /// <summary>
        /// 获取某个表的所有字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IEnumerable<FieldModel> GetFields(string tableName);


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        DataTable GetDataTable(string tableName, int index, int size, string orderBy, string where = "1=1");


        string GetInsertSql(DataTable table, string toTable, List<FieldMappingItem> fileMappingItem);


        int ExecuteNonQuery(string sql);


    }
}
