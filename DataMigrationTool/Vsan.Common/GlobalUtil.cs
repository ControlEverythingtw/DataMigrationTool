using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 全局工具类
    /// </summary>
    public class GlobalUtil
    {
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 投影
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="entity"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static V Select<T, V>(T entity, Func<T, V> mapping)
            where T : class
            where V : class
        {
            if (entity == null || mapping == null) return null;
            return mapping(entity);
        }

        /// <summary>
        /// 投影
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="entities"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static List<V> SelectList<T, V>(IList<T> entities, Func<T, V> mapping)
            where T : class
            where V : class
        {
            if (entities == null || entities.Count == 0 || mapping == null) return null;
            return entities.Select(x => x != null ? mapping(x) : null).ToList();
        }

        /// <summary>
        /// 打码
        /// </summary>
        /// <param name="value">要被打码的字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string Mask(string value, int start, int length)
        {
            if (value == null)
                return null;

            //如果打码参数有误，则按原样返回
            if (start + length > value.Length)
                return value;

            var builder = new StringBuilder();

            for (var i = 0; i < value.Length; i++)
            {
                if (i >= start && i < start + length)
                    builder.Append('*');
                else
                    builder.Append(value[i]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 打码手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string MaskMobile(string mobile)
        {
            return Mask(mobile, 3, 5);
        }

        /// <summary>
        /// 打码身份证号
        /// </summary>
        /// <param name="identityCard"></param>
        /// <returns></returns>
        public static string MaskIdentityCard(string identityCard)
        {
            return Mask(identityCard, 6, 8);
        }

        /// <summary>
        /// 异步安全地进行某个操作.
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="action"></param>
        public static void ProcessAsyncSafe(string desc, Action action)
        {
            Task.Run(() =>
            {
                action();
            }).ContinueWith(task =>
            {
                var ex = task.Exception.GetBaseException();
                var message = GetExceptionMessage(ex);

                logger.Error($"在{desc}时发生了异常，详细信息为：{message}");
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// 获取异常详细信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception ex)
        {
            var builder = new StringBuilder();
            var count = 0;
            var appString = "";

            while (ex != null)
            {
                if (count > 0)
                    appString += "    ";

                builder.AppendLine(appString + "异常信息(Message): " + ex.Message);
                builder.AppendLine(appString + "异常类型(Type): " + ex.GetType().FullName);
                builder.AppendLine(appString + "激发异常的方法(Method): " + (ex.TargetSite == null ? null : ex.TargetSite.Name));
                builder.AppendLine(appString + "异常来源(Source): " + ex.Source);
                if (ex.StackTrace != null)
                {
                    builder.AppendLine(appString + "详细错误(StackTrace): " + ex.StackTrace);
                }
                if (ex.InnerException != null)
                {
                    builder.AppendLine(appString + "内部错误(InnerException): ");
                    count++;
                }
                ex = ex.InnerException;
            }

            return builder.ToString();
        }

        /// <summary>
        /// 获取配置路径
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConfigPath(string configName)
        {
            //获取到的路径如‘D:\Gwy_API\Gwy.WebApi.Api\’
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var environment = ConfigurationManager.AppSettings["Environment"];
            return $"{root}configs\\{environment}\\{configName}";
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitNLog()
        {
            var fileName = GlobalUtil.GetConfigPath("NLog.config");
            if (File.Exists(fileName) == true)
                NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(fileName, true);
        }
    }
}
