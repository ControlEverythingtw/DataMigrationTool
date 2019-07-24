using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vsan.DataMigration.Models.Param;

namespace Vsan.DataMigration.Core
{
    /// <summary>
    ///传输引擎
    /// </summary>
    public class TransferEngine
    {
        private  static readonly ConcurrentDictionary<string,int> _State=new ConcurrentDictionary<string, int>();

        /// <summary>
        /// 开始
        /// </summary>
        public static void Start(TransferParam param)
        {
            Task.Run(() =>
            {
                var state=_State.GetOrAdd(param.OrderId, (id) => 1);

                var pageCount = param.Count % param.Size == 0 ? param.Count / param.Size : param.Count / param.Size + 1;

                if (param.Index <= 0)
                {
                    param.Index = 1;
                }

                var fromDb = DbFactory.GetDbHelper(param.FromTypeCode, param.FromConnString);
                var toDb = DbFactory.GetDbHelper(param.ToTypeCode, param.ToConnString);

                for (int page = param.Index; page <= pageCount; page++)
                {
                    try
                    {
                        var dataTable = fromDb.GetDataTable(param.FromTable, page, param.Size, param.OrderBy, param.Where);
                        var insertSql = toDb.GetInsertSql(dataTable, param.ToTable, param.FieldMapping);
                        toDb.ExecuteNonQuery(insertSql);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                  
                }
                toDb.Dispose();
                fromDb.Dispose();

                _State[param.OrderId] = 2;
            });
        }
    }

    public class TransferParam
    {
        public string OrderId { get; set; }

        public string FromConnString { get; set; }
        public string FromTable { get; set; }
        public  string FromTypeCode { get; set; }
        public string ToConnString { get; set; }
        public string ToTable { get; set; }
        public string ToTypeCode { get; set; }

        public string Where { get; set; }
        public string OrderBy { get; set; }
        public int Count { get; set; }
        public int Size { get; set; }

        public List<FieldMappingItem> FieldMapping { get; set; }
        public int Index { get; set; }
    }


}
