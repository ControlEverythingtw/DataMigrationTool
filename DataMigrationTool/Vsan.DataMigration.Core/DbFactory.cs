using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Core
{
    public class DbFactory
    {

        public static IDbHelper GetDbHelper(string typeCode,string link)
        {
            IDbHelper db = null;
            switch (typeCode)
            {
                case "SqlServer":
                    db = new SqlDbHelper(link);
                    break;
                case "MySql":
                    db = new MySqlDbHelper(link);
                    break;
                default:
                    db = new SqlDbHelper(link);
                    break;
            }
            return db;
        }

    }
}
