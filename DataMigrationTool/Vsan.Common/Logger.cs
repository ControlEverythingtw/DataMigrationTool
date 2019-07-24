using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logTxet"></param>
        void Info(string logTxet);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logTxet"></param>
        void Debug(string logTxet);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        void Print<T>(T t);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        void PrintDebugInfo<T>(T t);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        void Error(Exception exception);
    }
    /// <summary>
    /// 日志
    /// </summary>
    public  class Logger: ILogger, IBusinessConfig
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static Logger Instance = new Logger();

        /// <summary>
        /// 日志配置
        /// </summary>
        public static LoggerConfig Config = ConfigHelper.Get<LoggerConfig>();

        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadConfig()
        {
            Config = ConfigHelper.Get<LoggerConfig>();
        }


        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="logTxet"></param>
        public  void Info(string logTxet)
        {
            if ("INFO".Equals(Config.Level))
            {
                Task.Run(() =>
                {
                    Save("INFO",logTxet);
                });
            }

        }
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="exception"></param>
        public  void Error(Exception exception)
        {
            if ("ERROR".Equals(Config.Level)|| "INFO".Equals(Config.Level)|| "DEBUG".Equals(Config.Level))
            {
                Task.Run(() =>
                {
                    if (exception!=null)
                    {
                        var logMsg = $"{exception.Message} \r\n {exception.StackTrace}";
                        Save("ERROR", logMsg);
                    }
                });
            }
        }

        private static void Save(string Level,string logMsg)
        {
            var day = DateTime.Now.ToString("yyyy-MM-dd");
            var dir = Config.Dir + day;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var logText = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t【{Level}】\t{logMsg}";
            var path = $"{dir}/{DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分")}.log";
            IOHelper.SaveStr(path, logText,FileMode.Append);
        }

        /// <summary>
        /// 调试模式下记录日志
        /// </summary>
        /// <param name="logTxet"></param>
        public void Debug(string logTxet)
        {
            #if DEBUG

            if ("DEBUG".Equals(Config.Level)||"INFO".Equals(Config.Level))
            {
                Task.Run(() =>
                {
                    Save("DEBUG",logTxet);
                });
            }

            #endif
        }
        /// <summary>
        /// 打印对象信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Print<T>(T t)
        {
            Task.Run(() =>
            {
                var str = JsonConvert.SerializeObject(t);
                Save("Print", $"对象名:【{typeof(T).FullName}】\n{str}");
            });
        }
        /// <summary>
        /// 打印调试时的对象信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void PrintDebugInfo<T>(T t)
        {
            #if DEBUG
                Task.Run(() =>
                {
                    var str = JsonConvert.SerializeObject(t);
                    Save("Print", $"对象名:【{typeof(T).FullName}】\n{str}");
                });
            #endif
        }

       
    }
    /// <summary>
    /// 日志配置类
    /// </summary>
    public class LoggerConfig
    {
        /// <summary>
        /// 存储日志的目录
        /// </summary>
        public string Dir { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "App_Data/Log/";

        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; } = "INFO";

        /// <summary>
        /// 是否缓冲
        /// </summary>
        public bool IsCache { get; set; } = false;

    }
}
