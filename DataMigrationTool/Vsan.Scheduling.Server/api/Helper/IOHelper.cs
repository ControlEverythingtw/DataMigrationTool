using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Scheduling.Server.api.Helper
{
    /// <summary>
    /// IO工具
    /// </summary>
    public class IOHelper
    {
        static object obj = new object();
        /// <summary>
        /// 通过路径获取字符串
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetStr(string path)
        {
            lock (obj)
            {
                FileStream fileStream = null;
                StreamReader streamReader = null;
                try
                {
                    fileStream = new FileStream(path, FileMode.OpenOrCreate);
                    streamReader = new StreamReader(fileStream);
                    var str = streamReader.ReadToEnd();
                    return str;
                }
                finally
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }
        }

        public static void SaveObject(object simple,string path=null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = $"{AppDomain.CurrentDomain.BaseDirectory}App_Data\\SaveObject\\{simple.GetType().FullName.Replace(".","_")}\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = $"{path}{DateTime.Now.ToString("yyyy-MM-dd HHmmssfff")}.json";
            }
            var str = JsonConvert.SerializeObject(simple);
            SaveStr(path, str, FileMode.Append);
        }

        public static void WriteLine(string txt)
        {
             SaveStr(AppDomain.CurrentDomain.BaseDirectory+"IOHelper.txt",$"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")} {txt}",FileMode.Append);
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="i"></param>
        public static void ReName(string oldName, string newName, int i = 0)
        {
            FileInfo fi = new FileInfo(oldName);
            if (File.Exists(newName))
            {
                i++;
                newName = newName + i.ToString();
                ReName(oldName, newName, i);
            }
            else
            {
                fi.MoveTo(Path.Combine(newName));
            }
        }
        /// <summary>
        /// 保存字符串到指定路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="str"></param>
        public static void SaveStr(string path, string str, FileMode fileMode = FileMode.Create)
        {
            lock (obj)
            {
                FileStream fileStream = null;
                if (!File.Exists(path)) fileStream = File.Create(path);
                using (fileStream = fileStream ?? new FileStream(path, fileMode))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(str);
                        streamWriter.Flush();
                        streamWriter.Close();
                        fileStream.Close();
                    }
                }
            }
        }

        public static void CreateFile(string v)
        {
           var path=AppDomain.CurrentDomain.BaseDirectory+v;
            FileStream fileStream = null;
            if (!File.Exists(path)) fileStream = File.Create(path);
            using (fileStream = fileStream ?? new FileStream(path, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(v);
                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                }
            }
        }

        /// <summary>
        /// 保存字符串到指定路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="str"></param>
        public static void Log(string path, object obj, string baseDir = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                baseDir = baseDir ?? AppDomain.CurrentDomain.BaseDirectory;
                var dir = baseDir + "Log";
                path = string.Format("{0}\\{1}.txt", dir, DateTime.Now.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            var exTime = DateTime.Now.ToString("yyyy-MM-dd HH mm:ss:fff");
            var str = JsonConvert.SerializeObject(new { 时间 = exTime, 日志 = obj });
            SaveStr(path, str, FileMode.Append);
        }
        /// <summary>
        /// 保存字符串到指定路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="str"></param>
        public static void SerializedToJsonFile(string path, object obj, string baseDir = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                baseDir = baseDir ?? AppDomain.CurrentDomain.BaseDirectory;
                var dir = baseDir + "Log";
                path = string.Format("{0}/exlog_{1}.json", dir, DateTime.Now.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            var exTime = DateTime.Now.ToString("mm:ss:fff");
            var str = JsonConvert.SerializeObject(new { 时间 = exTime, 异常 = obj });
            SaveStr(path, str, FileMode.Append);

        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bytes"></param>
        public static void SaveFile(string filePath, byte[] bytes)
        {
            MemoryStream memoryStream = null;
            FileStream fileStream = null;

            try
            {

                var directoy = Path.GetDirectoryName(filePath);

                if (Directory.Exists(directoy) == false)
                    Directory.CreateDirectory(directoy);

                memoryStream = new MemoryStream(bytes);
                fileStream = new FileStream(filePath, FileMode.Create);
                memoryStream.WriteTo(fileStream);
            }
            finally
            {
                memoryStream.Close();
                memoryStream = null;

                fileStream.Close();
                fileStream = null;
            }
        }

    }
}
