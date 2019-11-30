using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Core
{
    public class MethodContainer
    {
        private static readonly ConcurrentDictionary<int, MethodInfo> _Cache = new ConcurrentDictionary<int, MethodInfo>();

        public static MethodInfo GetMethodInfo(int id,string dllFileName,string typeFullName,string methodName)
        {
           return  _Cache.GetOrAdd(id, (i) =>
           {
               var filePath = AppDomain.CurrentDomain.BaseDirectory + "App_Data/Upload/user1001/" + Path.GetFileName(dllFileName);
                //加载程序集(dll文件地址)，使用Assembly类   
                Assembly assembly = Assembly.LoadFile(filePath);
                //获取类型，参数（名称空间+类）   
                Type type = assembly.GetType(typeFullName);
                object instance = null;
                var method = type.GetMethod(methodName);
                return method;
            });
        }

    }
}
