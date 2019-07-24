using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web;

namespace Vsan.Common.Security
{
    /// <summary>
    /// WEBRequest
    /// </summary>
    public class WEBRequest
    {


        #region 判断当前页面是否接收到了Post请求 IsPost

        /// <summary>
        /// 判断当前页面是否接收到了Post请求 IsPost
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        #endregion

        #region 判断当前页面是否接收到了Get请求 IsGet

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        #endregion

        #region 返回指定的服务器变量信息 GetServerString

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }

        #endregion

        #region 返回上一个页面的地址 GetUrlReferrer

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;

        }

        #endregion

        #region 获取当前请求的原始 URL GetRawUrl

        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        #endregion

        #region 判断当前访问是否来自浏览器软件 IsBrowserGet

        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 判断是否来自搜索引擎链接 IsSearchEnginesGet

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return false;
            }
            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 获得当前完整Url地址 GetUrl

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        #endregion

        #region 获得指定Url参数的值 GetQueryString

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        #endregion

        #region 获得当前页面的名称 GetPageName

        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        #endregion

        #region 返回表单或Url参数的总个数 GetParamCount

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }

        #endregion

        #region 获得指定表单参数的值 GetFormString

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        #endregion

        #region 获得Url或表单参数的值 GetString

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName);
            }
            else
            {
                return GetQueryString(strName);
            }
        }

        #endregion

        #region 获得Url或表单参数的值 GetString

        /// <summary>
        /// 获得Url或表单参数的值
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetString(string strName, string defValue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return defValue;
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        #endregion

        #region 获得指定Url参数的int类型值 GetQueryInt

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        #endregion

        #region 获得指定表单参数的int类型值 GetFormInt

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        #endregion

        #region 获得指定Url或表单参数的int类型值 GetInt

        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            else
            {
                return GetQueryInt(strName, defValue);
            }
        }

        #endregion

        #region 获得指定Url或表单参数的Int64类型值 GetInt64

        /// <summary>
        /// 获得指定Url或表单参数的Int64类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的Int64类型值</returns>
        public static Int64 GetInt64(string strName, Int64 defValue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return defValue;
            }
            return Convert.ToInt64(HttpContext.Current.Request.QueryString[strName]);
        }

        #endregion

        #region 获得指定Url或表单参数的decimal类型值 decimal

        /// <summary>
        /// 获得指定Url或表单参数的decimal类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的decimal类型值</returns>
        public static decimal GetDecimal(string strName, decimal defValue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return defValue;
            }
            return Convert.ToDecimal(HttpContext.Current.Request.QueryString[strName]);
        }

        #endregion

        #region 获得指定Url或表单参数的GUID类型值 GetGuid

        public static Guid GetGuid(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return Guid.Empty;
            }
            return new Guid(HttpContext.Current.Request.QueryString[strName]);
        }

        public static Guid GetGuid(string strName, string defValue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return new Guid(defValue);
            }
            return new Guid(HttpContext.Current.Request.QueryString[strName]);
        }

        #endregion


        #region 获得指定Url参数的float类型值 GetQueryFloat

        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return Utils.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        #endregion

        #region 获得指定表单参数的float类型值 GetFormFloat

        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return Utils.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }

        #endregion

        #region 获得指定Url或表单参数的float类型值 GetFloat

        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(string strName, float defValue)
        {
            if (GetQueryFloat(strName, defValue) == defValue)
            {
                return GetFormFloat(strName, defValue);
            }
            else
            {
                return GetQueryFloat(strName, defValue);
            }
        }

        #endregion

        #region 获得当前页面客户端的IP

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
            {
                return "127.0.0.1";
            }

            return result;

        }

        #endregion

        #region 保存用户上传的文件 SaveRequestFile

        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }

        #endregion

        #region 得到当前完整主机头 GetCurrentFullHost

        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }

        #endregion

        #region 得到主机头 GetHost

        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        #endregion


        #region 输出字符串+回车 WriteLine

        /// <summary>
        /// 输出字符串+回车 WriteLine
        /// </summary>
        /// <param name="obj"></param>
        public static void WriteLine(object obj)
        {
            System.Web.HttpContext.Current.Response.Write(obj.ToString() + "<br />");
        }

        #endregion


        #region  获取网站Url GetWebSiteUrl

        /// <summary>
        /// 获取网站Url GetWebSiteUrl
        /// </summary>
        /// <returns></returns>
        public static string GetWebSiteUrl()
        {
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            string host = HttpContext.Current.Request.Url.Host;
            string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port != "80")
                host += ":" + port;

            if (applicationPath == "/")
            {
                return @"http://" + host;
            }
            else
            {
                return @"http://" + host + applicationPath;
            }
        }

        #endregion

        #region 获取网站根目录 GetApplicationPath

        /// <summary>
        /// 获取网站根目录 GetApplicationPath
        /// </summary>
        public static string GetApplicationPath()
        {
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            if (applicationPath.Length == 1)
                return "";
            else
                return applicationPath;
        }

        #endregion

        #region 网站根目录 ApplicationPath

        /// <summary>
        /// 网站根目录 ApplicationPath
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                //string applicationPath = "/";
                //if (HttpContext.Current != null)
                //{
                //    applicationPath = HttpContext.Current.Request.ApplicationPath;
                //}
                //if (applicationPath == "/")
                //{
                //    return string.Empty;
                //}
                //return applicationPath.ToLower(CultureInfo.InvariantCulture);
                string applicationPath = HttpContext.Current.Request.ApplicationPath;
                if (applicationPath.Length == 1)
                    return "";
                else
                    return applicationPath;
            }
        }

        #endregion

        #region 网站域名 DomainName

        public static string DomainName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                return HttpContext.Current.Request.Url.Host;
            }
        }
        #endregion

        #region URL追加参数 AppendQuerystring

        public static string AppendQuerystring(string url, string querystring)
        {
            return AppendQuerystring(url, querystring, false);
        }

        public static string AppendQuerystring(string url, string querystring, bool urlEncoded)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            string str = "?";
            if (url.IndexOf('?') > -1)
            {
                if (!urlEncoded)
                {
                    str = "&";
                }
                else
                {
                    str = "&amp;";
                }
            }
            return (url + str + querystring);
        }

        #endregion


        //#region 添加CSS样式文件 AddStyleSheet

        ///// <summary>
        ///// 添加CSS样式文件 AddStyleSheet
        ///// </summary>
        ///// <param name="cssFile"></param>
        //public static void AddStyleSheet(string cssURL)
        //{
        //    HtmlLink link = new HtmlLink();
        //    link.Href = cssURL;
        //    link.Attributes["rel"] = "stylesheet";
        //    link.Attributes["type"] = "text/css";
        //    ((Page)HttpContext.Current.Handler).Header.Controls.Add(link);
        //}

        //#endregion

        #region 下载文件 DownloadFile
        /// <summary>
        /// 下载文件　DownloadFile
        /// </summary>
        /// <param name="file"></param>
        public static void DownloadFile(string fileUrl)
        {
            //try
            //{
            //    string fileName = HttpContext.Current.Server.MapPath(fileUrl);
            //    fileName = HttpContext.Current.Server.UrlDecode(fileName);

            //    if (File.Exists(fileName))
            //    {
            //        FileInfo fi = new FileInfo(fileName);
            //        HttpContext.Current.Response.Clear();
            //        HttpContext.Current.Response.ClearHeaders();
            //        HttpContext.Current.Response.Buffer = false;
            //        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(fileName), System.Text.Encoding.UTF8));
            //        HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString());
            //        HttpContext.Current.Response.ContentType = "application/octet-stream";
            //        HttpContext.Current.Response.WriteFile(fileName);
            //        HttpContext.Current.Response.Flush();
            //        HttpContext.Current.Response.End();
            //    }
            //    else
            //    {
            //        HttpContext.Current.Response.Write("<script language='javascript'><alert('下载文件失败!')></script>");
            //        HttpContext.Current.Response.End();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Response.Write(ex.ToString());
            //}
        }
        #endregion

        #region 判断远程文件是否存在 ExistsRemoteFile

        /// <summary>
        /// 判断远程文件是否存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool ExistsRemoteFile(string fileUrl)
        {
            bool result = false;
            WebResponse response = null;
            try
            {
                WebRequest req = WebRequest.Create(fileUrl);
                response = req.GetResponse();
                result = response == null ? false : true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        #endregion

        ///// 2.通过传入的url获取远程文件数据(二进制流)，大家可以通过Response.BinaryWrite()方法将获取的数据流输出
        ///// </summary>
        ///// <param name="url">图片的URL</param>
        ///// <param name="ProxyServer">代理服务器</param>
        ///// <returns>图片内容</returns>
        //public byte[] GetFile(string url, string proxyServer)
        //{
        //    WebResponse rsp = null;
        //    byte[] retBytes = null;
        //    try
        //    {
        //        Uri uri = new Uri(url);
        //        WebRequest req = WebRequest.Create(uri);
        //        rsp = req.GetResponse();
        //        Stream stream = rsp.GetResponseStream();
        //        if (!string.IsNullOrEmpty(proxyServer))
        //        {
        //            req.Proxy = new WebProxy(proxyServer);
        //        }
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            int b;
        //            while ((b = stream.ReadByte()) != -1)
        //            {
        //                ms.WriteByte((byte)b);
        //            }
        //            retBytes = ms.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retBytes = null;
        //    }
        //    finally
        //    {
        //        if (rsp != null)
        //        {
        //            rsp.Close();
        //        }

        //    }
        //    return retBytes;
        //}


        //          ///将本地文件通过httpwebrequest上传到服务器
        //          ///localFile:本地文件路径及文件名称,uploadUrl:远程路径(虚拟目录)
        //          ///在远程路径中需要把文件保存在的文件夹打开权限设置，否则上传会失败
        //         public bool UploadFileBinary(string localFile, string uploadUrl)
        //        {
        //            bool result = false;
        //            HttpWebRequest req = null;
        //            Stream reqStream = null;
        //            FileStream rdr = null;
        //            try
        //            {
        //                req = (HttpWebRequest)WebRequest.Create(uploadUrl);
        //                req.Method = "PUT";
        //                req.AllowWriteStreamBuffering = true;
        //                // Retrieve request stream 
        //                reqStream = req.GetRequestStream();

        //                // Open the local file
        //                rdr = new FileStream(localFile, FileMode.Open);
        //                byte[] inData = new byte[4096];
        //                int bytesRead = rdr.Read(inData, 0, inData.Length);
        //                while (bytesRead > 0)
        //                {
        //                    reqStream.Write(inData, 0, bytesRead);
        //                    bytesRead = rdr.Read(inData, 0, inData.Length);
        //                }
        //                rdr.Close();
        //                reqStream.Close();
        //                req.GetResponse();
        //                result = true;
        //            }
        //            catch (Exception e)
        //            {
        //                result = false;
        //            }
        //            finally
        //            {
        //                if (reqStream != null)
        //                {
        //                    reqStream.Close();
        //                }
        //                if (rdr != null)
        //                {
        //                    rdr.Close();
        //                }
        //            }
        //            return result;
        //        }

        //        string Cookie = String.Empty;
        //String url = "http://search.patentstar.com.cn/cprs2010/Docdb/GetBns.aspx?PNo=APP201180002436";
        //String refer = url.Substring(0, url.LastIndexOf("/") + 1);
        //System.Net.HttpWebRequest req = System.Net.HttpWebRequest.Create(url) as System.Net.HttpWebRequest;
        //req.AllowAutoRedirect = false;
        //req.Referer = refer;
        //req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
        //System.Net.HttpWebResponse res = req.GetResponse() as System.Net.HttpWebResponse;
        //System.Net.WebHeaderCollection headers = res.Headers;
        //String newUrl = "";
        //if ((res.StatusCode == System.Net.HttpStatusCode.Found) ||
        //  (res.StatusCode == System.Net.HttpStatusCode.Redirect) ||
        //  (res.StatusCode == System.Net.HttpStatusCode.Moved) ||
        //  (res.StatusCode == System.Net.HttpStatusCode.MovedPermanently))
        //{
        //  newUrl = headers["Location"];
        //  newUrl = newUrl.Trim();
        //}

        //if (headers["Set-Cookie"] != null)
        //{
        //  Cookie = headers["Set-Cookie"];
        //}

        //NameValueCollection collHeader = new NameValueCollection();
        //if (Cookie.Length > 0)
        //{
        //  collHeader.Add("Cookie", Cookie);
        //}
        //res.Close();
        //req = null;

        //String fileName = newUrl.Substring(newUrl.LastIndexOf("/") + 1);
        //req = System.Net.HttpWebRequest.Create(newUrl) as System.Net.HttpWebRequest;
        //req.AllowAutoRedirect = true;
        //req.Referer = url;
        //req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
        //res = req.GetResponse() as System.Net.HttpWebResponse;

        //System.IO.Stream stream = res.GetResponseStream();
        //byte[] buffer = new byte[32 * 1024];
        //int bytesProcessed = 0;
        //System.IO.FileStream fs = System.IO.File.Create(Server.MapPath(fileName));
        //int bytesRead;
        //do
        //{
        //  bytesRead = stream.Read(buffer, 0, buffer.Length);
        //  fs.Write(buffer, 0, bytesRead);
        //  bytesProcessed += bytesRead;
        //}
        //while (bytesRead > 0);
        //fs.Flush();
        //fs.Close();
        //res.Close();
        //Response.Write("文件 " + fileName +  " 已经下载完成。");

    }
}
