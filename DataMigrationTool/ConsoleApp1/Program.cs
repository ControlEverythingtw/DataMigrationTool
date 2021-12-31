using Dapper;
using DynamicMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vsan.DataMigration.Core;

namespace ConsoleApp1
{
    class Program
    {
        static Class2 class2 = new Class2();

        static void Main(string[] args)
        {
            //UpdateField();


            class2.Start("x");
            class2.Dispose();
            class2 = new Class2();
            class2.Start("y");



            Console.WriteLine("完成");
            Console.ReadLine();
        }

        private static void Class1_OnDataUpdate()
        {
            Console.WriteLine("Class1_OnDataUpdate");
        }

        private static void UpdateField()
        {
            var database = "IPTS_PermissionDB";
            var oldValue = "ATL_MTW20_INJ";
            var newValue = "ATL_MTW60_INJ";
            var fieldName = "Factory";
            //var ip = "127.0.0.1";
            var ip = "192.168.50.250";

            var dbType = "MySql";
            var connString = $"server={ip};database=IPTS_PermissionDB;uid=root;pwd=Supercncy.;Persist Security Info=True;Charset=utf8;";
            var db1 = DbFactory.GetDbHelper(dbType, connString);
            UpdateAllTableField(database, oldValue, newValue, fieldName, db1);

            var count = db1.ExecuteNonQuery($"update Sys_Factory set FCode='{newValue}',FName='{newValue}' where iden='1'");
            if (count > 0)
            {
                Console.WriteLine($"表Sys_Factory更新成功,影响行数:{count}");
            }

            var connString2 = $"server={ip};database=IPTSDB;uid=root;pwd=Supercncy.;Persist Security Info=True;Charset=utf8;";
            var db2 = DbFactory.GetDbHelper(dbType, connString2);
            UpdateAllTableField("IPTSDB", oldValue, newValue, fieldName, db2);
        }

        private static void UpdateAllTableField(string database, string oldValue, string newValue, string fieldName, IDbHelper db)
        {
            var tables = db.GetTables(database).Where(a=>!a.TableName.StartsWith("v_"));
            foreach (var table in tables)
            {
                var fields = db.GetFields(table.TableName);
                if (fields.Any(a => a.FieldName == fieldName))
                {
                    var count = db.ExecuteNonQuery($"Update {table.TableName} set {fieldName}='{newValue}' where {fieldName}='{oldValue}'");
                    if (count > 0)
                    {
                        Console.WriteLine($"表{table.TableName}更新成功,影响行数:{count}");
                    }
                }
            }
        }

        private static void NewMethod()
        {
            var gwyAdmin = DbFactory.GetDbHelper("SqlServer", "Data Source = 119.23.68.66,1433,1433; Initial Catalog =Gwy_Admin; User Id = sa; Password = IbdP16; ");
            var testDb = DbFactory.GetDbHelper("SqlServer", "Data Source = 119.23.68.66,1433,1433; Initial Catalog =test; User Id = sa; Password = IbdP16; ");
            //var gwy = DbFactory.GetDbHelper("SqlServer", "Data Source = rm-wz902vgj9f1d7apg4bo.sqlserver.rds.aliyuncs.com,3433; Initial Catalog =gwy; User Id = gwy_admin888; Password = Un719vDV; ");







            //UpdateCmsshop(gwyAdmin, testDb);
        }

        private static void UpdateCmsshop(IDbHelper gwyAdmin, IDbHelper testDb)
        {
            var oldUser = testDb.Query<CMS_SHOP>(" 1=1 and (ShopType='B' or ShopType='AB' or ShopType='AP' )  ");

            var errorCount = 0;
            var successCount = 0;
            var allCount = oldUser.Count();
            foreach (var item in oldUser)
            {
                if (!Guid.TryParse(item.uid, out Guid id))
                {
                    id = Guid.NewGuid();
                }
                var user = new GwyOrganUser()
                {
                    id = id,
                    createDate = item.CREATE_DATE,
                    createUserId = id,
                    enterpriseId = id,
                    email = item.E_Mail ?? string.Empty,
                    roleContent = string.Empty,
                    roleType = item.ShopType,
                    sex = item.ConvertToSex(),
                    status = item.ConvertToState(),
                    userName = item.ShopName ?? string.Empty,
                    userPwd = item.Password ?? string.Empty,
                    userTel = item.Mobile ?? string.Empty,
                };
                var count = gwyAdmin.GetDataCount("GwyOrganUser", $" roleType='{item.ShopType}' and userTel='{item.Mobile}' ");
                if (count == 0)
                {
                    count = gwyAdmin.Insert(user);
                    successCount++;
                }
                else
                {
                    errorCount++;
                }
                Console.Clear();
                Console.WriteLine($"成功{successCount}/{allCount},失败{errorCount}");
            }
        }

        private static void Test1()
        {
            var fromDb = DbFactory.GetDbHelper("SqlServer", "Data Source = 119.23.68.66,1433,1433; Initial Catalog =Gwy_Admin; User Id = sa; Password = IbdP16; ");
            var toDb = DbFactory.GetDbHelper("SqlServer", "Data Source = 119.23.68.66,1433,1433; Initial Catalog =test; User Id = sa; Password = IbdP16; ");
            var count = fromDb.GetDataCount("GwyOrganUser");
            var gwyOrganUsers = fromDb.Query<GwyOrganUser>();

            foreach (var item in gwyOrganUsers)
            {
                var sql = $"update CMS_Shop set uid = '{item.id.ToString()}' where shopType='{item.roleType}' and mobile='{item.userTel}'";
                var fist = toDb.GetConn().Execute(sql);
            }
        }

     
    }
    public static class Convert
    {

        public static int? ConvertToSex(this CMS_SHOP user)
        {
            if (user == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(user.sex))
            {
                return null;
            }
            if (user.sex == "女")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static int? ConvertToState(this CMS_SHOP user)
        {
            if (user == null)
            {
                return null;
            }
            if (user.status==null)
            {
                return null;
            }
            if (user.status.Value)
            {
                return 0;
            }
            return 1;
        }
    }
}
