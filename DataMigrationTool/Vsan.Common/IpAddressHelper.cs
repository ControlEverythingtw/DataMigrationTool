using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

namespace Vsan.Common
{
    /// <summary>
    /// Ip地址帮助类
    /// </summary>
    public class IpAddressHelper
    {
        /// <summary>
        /// Baidu获取Ip地址的接口
        /// </summary>
        public static readonly string BaiduIpApiUrl = ConfigurationManager.AppSettings["BaiduIpApiUrl"];

        /// <summary>
        /// 通过Ip获取用户所在位置
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetAddress(string ip)
        {
            return GetBaiduIp(ip);
        }
        /// <summary>
        /// 是否是中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
        /// <summary>
        /// 百度api
        /// </summary>
        /// <returns></returns>
        public static string GetBaiduIp(string ip)
        {
            try
            {
                string cs = "";
                string url = $"{BaiduIpApiUrl}&ip={ip}";
                WebClient client = new WebClient();
                var buffer = client.DownloadData(url);
                string jsonText = Encoding.UTF8.GetString(buffer);
                JObject jo = JObject.Parse(jsonText);
                var txt = jo["content"]["address_detail"]["city"];
                JToken st = txt;
                string str = st.ToString();
                if (str == "")
                {
                    cs = GetCS(ip);
                    return cs;

                }
                int s = str.IndexOf('市');
                string css = str.Substring(0, s);
                bool bl = HasChinese(css);

                if (bl)
                {
                    cs = css;
                }
                else
                {
                    cs = GetCS(ip);
                }

                return cs;
            }
            catch
            {
                return GetIPCitys(ip);
            }

        }
        /// <summary>
        /// 新浪api
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetCS(string ip)
        {
            try
            {
                string url = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?ip=" + ip;
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据
                string stt = Encoding.GetEncoding("GBK").GetString(pageData).Trim();
                return stt.Substring(stt.Length - 2, 2);
            }
            catch
            {
                return "未知";
            }

        }
        /// <summary>
        /// 淘宝api
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static string GetIPCitys(string strIP)
        {
            try
            {
                string Url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + strIP + "";

                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                wReq.Timeout = 2000;
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream))
                {
                    string jsonText = reader.ReadToEnd();
                    JObject ja = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText);
                    if (ja["code"].ToString() == "0")
                    {
                        string c = ja["data"]["city"].ToString();
                        int ci = c.IndexOf('市');
                        if (ci != -1)
                        {
                            c = c.Remove(ci, 1);
                        }
                        return c;
                    }
                    else
                    {
                        return "未知";
                    }
                }
            }
            catch (Exception)
            {
                return ("未知");
            }
        }

    }
   
}
