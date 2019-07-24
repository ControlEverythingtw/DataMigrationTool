using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Vsan.Common
{
    public  class Http
    {
        Action<Exception> ErrorAction;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Http Error(Action<Exception>  obj)
        {
            ErrorAction = obj;
         
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public Http Get(string url, Action<string> callBack)
        {
            try
            {
                var task = new HttpClient().GetStringAsync(url);
                var stram = task.Result;
                // var obj = JsonConvert.DeserializeObject<T>(json);
                callBack.Invoke(stram);
            }
            catch (Exception ex)
            {
                if (ErrorAction!=null)
                {
                    ErrorAction.Invoke(ex);
                }
                else
                {
                    throw;
                }
            }
            return this;
        }
        public  void  Get(string url,object data,Action<dynamic> callBack)
        {
            new Thread(() => {
                
                    HttpClient client = new HttpClient();
                    var task = client.GetStreamAsync(url);
                    var stram = task.Result;
                    var json = new System.IO.StreamReader(stram).ReadToEnd();
                    var obj = JsonConvert.DeserializeObject(json);
                    callBack.Invoke(obj);
                
            }).Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="auth"></param>
        /// <param name="reqParams"></param>
        /// <returns></returns>
        public string SendGet(string url, string auth, string reqParams)
        {
            return SendRequest("GET", url, auth, reqParams);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="auth"></param>
        /// <param name="reqParams"></param>
        /// <returns></returns>

        public string SendPut(string url, string auth, string reqParams)
        {
            return SendRequest("PUT", url, auth, reqParams);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="auth"></param>
        /// <param name="reqParams"></param>
        /// <returns></returns>
        public string SendRequest(string method, string url, string auth, string reqParams)
        {
            Console.WriteLine("Send request - " + method.ToString() + " " + url + " " + DateTime.Now);
            if (reqParams != null)
            {
                Console.WriteLine("Request Content - " + reqParams + " " + DateTime.Now);
            }
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = method;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.KeepAlive = false;
                if (!string.IsNullOrEmpty(auth))
                {
                    httpWebRequest.Headers.Add("Authorization", "Basic " + auth);
                }
                if (method == "POST")
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(reqParams);
                    httpWebRequest.ContentLength = bytes.Length;
                    using (Stream stream = httpWebRequest.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Close();
                    }
                }
                if (method == "PUT")
                {
                    byte[] bytes2 = Encoding.UTF8.GetBytes(reqParams);
                    httpWebRequest.ContentLength = bytes2.Length;
                    using (Stream stream2 = httpWebRequest.GetRequestStream())
                    {
                        stream2.Write(bytes2, 0, bytes2.Length);
                        stream2.Close();
                    }
                }
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (object.Equals(httpWebResponse.StatusCode, HttpStatusCode.OK))
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    string statusDescription = ((HttpWebResponse)ex.Response).StatusDescription;
                    using (StreamReader streamReader2 = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream(), Encoding.UTF8))
                    {
                        return streamReader2.ReadToEnd();
                    }
                }
                string text = method + url + auth + reqParams;
                Console.WriteLine(text);
            }
            finally
            {
                httpWebResponse?.Close();
                httpWebRequest?.Abort();
            }
            return string.Empty;
        }
    }
}
