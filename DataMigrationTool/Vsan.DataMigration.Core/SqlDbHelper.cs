using Dapper;
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
    public class SqlDbHelper : IDbHelper
    {
        private string link;
        private SqlConnection connection;

        public SqlDbHelper(string link)
        {
            SetLink(link);
            connection = new SqlConnection(link);
            Open();
        }
        public void SetLink(string link)
        {
            this.link = link;

        }
        public void Dispose()
        {
            if (connection != null && connection.State == ConnectionState.Open)
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
        public DataTable GetDataTable(string tableName, int index, int size, string orderBy, string where = "1=1")
        {
            where = !string.IsNullOrWhiteSpace(where) ? where : "1=1";

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            var srn = size * (index - 1);
            var ern = size * index;
            var sql = $@"SELECT TOP {size} *  FROM 
                (select row_number()over(order by {orderBy}) as rn,* from {tableName} where  {where} )temTable 
                where rn >= {srn} and rn <={ern} ";
            using (SqlCommand comm = new SqlCommand(sql, connection))
            {
                SqlDataAdapter da = new SqlDataAdapter(comm);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

        }

        public IEnumerable<FieldModel> GetFields(string tableName)
        {
            var sql =
@"SELECT 
    TableName       =d.name,
    FieldName     = a.name,
	FieldComment   = isnull(g.[value],''),
    FieldType       = b.name,
    FieldLength = a.length
FROM  syscolumns a
left join  systypes b on  a.xusertype=b.xusertype
inner join sysobjects d on  a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
left join  syscomments e on  a.cdefault=e.id
left join  sys.extended_properties   g on  a.id=G.major_id and a.colid=g.minor_id  
where  d.name=@tableName  
order by  a.id,a.colorder";


            var result = connection.Query<FieldModel>(sql, new { tableName });

            return result;


        }

        public IEnumerable<TableModel> GetTables(string dbName)
        {

            var sql = @"select a.name TableName,b.value TableComment from sys.tables  a
left join  sys.extended_properties  b  on a.object_id=b.major_id and minor_id = 0 
order by a.create_date desc";

            var result = connection.Query<TableModel>(sql);

            return result;

        }
        public string GetInsertSql(DataTable table, string toTable, List<FieldMappingItem> fileMapping)
        {
            return SqlBuilder.GetInsertSql(table, toTable, fileMapping, "[", "]");
        }

        public int ExecuteNonQuery(string sql)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
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
