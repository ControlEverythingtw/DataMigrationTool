using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vsan.DataMigration.Models.Param;
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.Core
{
    public class MySqlDbHelper : IDbHelper
    {
        private string link;
        private MySqlConnection connection;

        public MySqlDbHelper(string link)
        {
            SetLink(link);
        }

        public void SetLink(string link)
        {
            this.link = link;
            connection = new MySqlConnection(link);
            Open();
        }
        public void Dispose()
        {
            if (connection!=null&&connection.State== ConnectionState.Open)
            {
                connection?.Close();
            }
            connection?.Dispose();
        }

        public void Open()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }


        public IEnumerable<FieldModel> GetFields(string tableName)
        {
           
            var table_schema = link.Split(';').Where(a=>a.Contains("Database")).FirstOrDefault().Split('=')[1].Trim();

            var sql = @"select Table_Name TableName,Column_Name FieldName , Column_COMMENT FieldComment, Column_Type  FieldType ,CHARACTER_OCTET_LENGTH FieldLength from information_schema.COLUMNS where table_name=@table_name and table_schema=@table_schema";

            var result = connection.Query<FieldModel>(sql, new { table_name=tableName,table_schema = table_schema });

            return result;

        }

        public IEnumerable<TableModel> GetTables(string dbName)
        {
            
            var sql = "select Table_Name TableName,Table_Comment TableComment from information_schema.tables where table_schema=@table_schema";

            var result = connection.Query<TableModel>(sql, new { table_schema = dbName });

            return result;
        }
        public DataTable GetDataTable(string tableName, int index, int size, string orderBy, string where = "1=1")
        {
           
            var srn = size * (index - 1);
            var ern = size * index;
            var sql = $@"select * from {tableName} where {where} order by {orderBy} LIMIT {srn},{ern};";

            using (MySqlCommand comm = new MySqlCommand(sql, connection))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(comm);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public string GetInsertSql(DataTable table, string toTable, List<FieldMappingItem> fileMappingItem)
        {
            return SqlBuilder.GetInsertSql(table, toTable, fileMappingItem, "`", "`");
        }

        public int ExecuteNonQuery(string sql)
        {
            try
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(sql, connection))
                {
                    var count = sqlCommand.ExecuteNonQuery();
                    // connection.Execute(($" Update [{dBTable.MapToTable}] Set ImportCount=2 Where dataid in ('{string.Join("','", ids)}')");
                    return count;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
