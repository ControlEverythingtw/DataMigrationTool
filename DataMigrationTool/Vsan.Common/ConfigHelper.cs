using Newtonsoft.Json;
using System;
using System.IO;

namespace Vsan.Common
{
    /// <summary>
    /// 配置管理工具
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Get<T>(string path = null) where T : new()
        {
            path = GetPath<T>(path);
            var str = IOHelper.GetStr(path);
            if (!string.IsNullOrWhiteSpace(str))
            {
                var t = JsonConvert.DeserializeObject<T>(str);
                if (t != null)
                {
                    return t;
                }
            }
            var t2 = new T();
            Save(t2, path);
            return t2;
        }

        private static string GetPath<T>(string path) where T : new()
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Config/";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                path = $"{dir}{typeof(T).FullName}.json";
            }

            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Save<T>(T t, string path = null) where T : new()
        {
            try
            {
                path = GetPath<T>(path);
                IOHelper.SaveStr(path, JsonConvert.SerializeObject(t));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
