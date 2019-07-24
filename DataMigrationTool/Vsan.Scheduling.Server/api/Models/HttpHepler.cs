using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gwy.Service.JobSystem.api.Models
{
    public class HttpHepler
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方法</param>
        /// <param name="param">参数</param>
        /// <param name="token">访问令牌</param>
        /// <param name="contentType">内容类型</param>
        public static string SendHttpRequest(string url, string method = "GET", string param = null, string token = null, string contentType = "application/json")
        {
            logger.Debug($"{method} {url} {contentType}\r\n{token} {param}");

            try
            {

                if (string.IsNullOrWhiteSpace(url))
                {
                    throw new ArgumentException("参数 \"url\" 不能为空或者WhiteSpace");
                }
                var reqParam = string.Empty;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = null;

                if (!string.IsNullOrEmpty(token))
                {
                    httpWebRequest.Headers.Add("Authorization", token);
                }
                if (method == "POST" || method == "PUT")
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(reqParam);
                    httpWebRequest.ContentLength = bytes.Length;
                    using (Stream stream = httpWebRequest.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Close();
                    }
                }

                httpWebRequest.Method = method;
                httpWebRequest.ContentType = contentType;
                httpWebRequest.KeepAlive = false;
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
                {
                    var str = streamReader.ReadToEnd();

                    logger.Debug($"{str}");

                    return str;
                }
            }
            catch (Exception ex)
            {
                //IOHelper.WriteLine(ex.Message + ex.StackTrace);
                return ex.Message;
            }
        }
    }
}
