using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vsan.Scheduling.Server.api.Helper
{
    public class HttpHelper
    {

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <param name="token"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Send(string url, string method = "GET", string param = null, string token = null, string contentType = "application/json")
        {
            using (var httpClient = new HttpClient())
            {
                //设置请求头
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = null;
                method = method.ToUpper();

                switch (method)
                {
                    case "POST":
                        HttpContent content = new StringContent(param);
                        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                        response = httpClient.PostAsync(url, content).Result;
                        break;
                    case "DELETE":
                        response = httpClient.DeleteAsync(url).Result;
                        break;
                    case "PUT":
                        HttpContent putContent = new StringContent(param);
                        putContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                        response = httpClient.PutAsync(url, putContent).Result;
                        break;
                    case "GET":
                        response = httpClient.GetAsync(url).Result;
                        break;
                }
                if (response==null)
                {
                    return "response==null";
                }
                //分析结果
                if (!response.IsSuccessStatusCode)
                {
                    return $"{url}接口返回错误响应:StatusCode={response.StatusCode};Content={response.Content}";
                }
                return response.Content.ReadAsStringAsync().Result;
            }

        }
    }
}
