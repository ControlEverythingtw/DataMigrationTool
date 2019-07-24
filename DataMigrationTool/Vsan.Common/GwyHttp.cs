using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GwyHttp
    {
        /// <summary>
        /// 
        /// </summary>
        public GwyHttp()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        public GwyHttp(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="token"></param>
        public GwyHttp(string baseUrl, string token)
        {
            BaseUrl = baseUrl;
            Token = token;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T Get<T>(string url, object param = null)
        {
            return SendRequest<T>("GET", url, param);
        }
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="auth"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public T Post<T>(string url, object param = null,string auth=null,string contentType = "application/json")
        {
            return SendRequest<T>("POST", url, param, auth, contentType);
        }

        /// <summary>
        /// Post 流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Stream PostStream(string url,object param)
        {
            HttpWebRequest httpWebRequest = null;
            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "Post";
            httpWebRequest.ContentType = "application/json";
           // httpWebRequest.KeepAlive = true;
            httpWebRequest.Timeout = 60000;
            var reqParam = JsonConvert.SerializeObject(param);
            byte[] bytes = Encoding.UTF8.GetBytes(reqParam);
            httpWebRequest.ContentLength = bytes.Length;
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
            var repos = httpWebRequest.GetResponse();
            if (repos.ContentType.Contains("application/json"))
            {
                using (StreamReader streamReader = new StreamReader(repos.GetResponseStream()))
                {
                    var text = streamReader.ReadToEnd();
                    throw new Exception("微信服务器返回错误消息:"+text);
                };
            }
            return repos.GetResponseStream();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="paramData">参数数据</param>
        /// <param name="stream">数据流</param>
        /// <returns></returns>
        public string UploadFile(string url, string paramData,Stream stream)
        {

            return string.Empty;

        }

        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="paramData">参数</param>
        /// <returns></returns>
        public string PostWebRequest(string postUrl, string paramData)
        {
            string res = string.Empty;
            Stream newStream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            try
            {
                if (!postUrl.StartsWith("http"))
                    return "";
                byte[] byteArray = Encoding.Default.GetBytes(paramData); //转化 /

                //设置request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                //写入参数
                newStream = request.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);

                //获取响应
                response = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                newStream.Close();
                sr.Close();
                response.Close();
            }
            return res;

        }

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SendRequest<T>(string method, string url, object param = null, string auth = null,string contentType = "application/json")
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("参数 \"url\" 不能为空或者WhiteSpace");
            }
            if (!string.IsNullOrWhiteSpace(auth))
            {
                Token = auth;
            }
            var reqUrl = string.Empty;
            var reqParam = string.Empty;
            if (!url.ToUpper().StartsWith("HTTP") && !string.IsNullOrWhiteSpace(BaseUrl))
            {
                reqUrl = $"{BaseUrl}{url}";
            }
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            httpWebRequest = (HttpWebRequest)WebRequest.Create(reqUrl);
            httpWebRequest.Method = method;
            httpWebRequest.ContentType = contentType;
            httpWebRequest.KeepAlive = false;
            if (!string.IsNullOrEmpty(Token))
            {
                httpWebRequest.Headers.Add("Authorization", Token);
            }
            if (param != null && method == "GET")
            {
                var pArr = param.GetType().GetProperties().Select(a => $"{a.Name}={a.GetValue(param)}");
                reqParam = string.Join("&", pArr);
                reqUrl = $"{reqUrl}?{reqParam}";
            }
            if (method == "POST")
            {
                reqParam = JsonConvert.SerializeObject(param);
                byte[] bytes = Encoding.UTF8.GetBytes(reqParam);
                httpWebRequest.ContentLength = bytes.Length;
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
            }
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
            {
                var str = streamReader.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<T>(str);
                return obj;
            }
        }
    }
}

