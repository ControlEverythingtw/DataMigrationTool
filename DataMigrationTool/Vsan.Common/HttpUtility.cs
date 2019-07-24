using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;

namespace Vsan.Common
{
	
	public static class HttpUtility
	{
		const string DEFAULT_USER_AGENT = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
	
		static readonly Encoding ENCODING = Encoding.UTF8;
	
		//请求连接限时
		const int CON_TIMEOUT = 8 * 1000;
	
		//数据传输限时
		const int READ_WRITE_TIMEOUT = 8 * 1000;
	
		static HttpUtility()
		{
			ServicePointManager.DefaultConnectionLimit = 512;
			ServicePointManager.Expect100Continue = false;
		}
  
		/// <summary>    
		/// 创建GET方式的HTTP请求    
		/// </summary>    
		/// <param name="url">请求的URL</param>    
		/// <param name="parameters">随同请求POST的参数名称及参数值字典</param>    
		/// <returns>返回的字符串</returns>    
		public static string Get(string url, IDictionary<string,string> parameters)
		{  
			if (string.IsNullOrEmpty(url)) {  
				throw new ArgumentNullException("url");  
			}  
			if (null == parameters) {  
				throw new ArgumentNullException("parameters");  
			}  
		
			string strParmater = getStrParameter(parameters);
			url += parameters;

			HttpWebRequest request = getRequest(url, "GET");
			HttpWebResponse response = null;
			if (null != strParmater) {
				try {
					response = request.GetResponse() as HttpWebResponse;
				} catch (Exception ex) {
					Console.WriteLine(ex); //正式项目中请改为log打印
					request.Abort();
				}
			}

			return OpenReadWithHttps(response);
		}
  
  
		/// <summary>    
		/// 创建POST方式的HTTP请求  
		/// </summary>    
		/// <param name="url">请求的URL</param>    
		/// <param name="parameters">随同请求POST的参数名称及参数值字典</param>    
		/// <returns>返回的字符串</returns>  
		public static string Post(string url, IDictionary<string,string> parameters)
		{  
			if (string.IsNullOrEmpty(url)) {  
				throw new ArgumentNullException("url");  
			}  
		
			if (null == parameters) {  
				throw new ArgumentNullException("parameters");  
			}  
		
			string strParmater = getStrParameter(parameters);
			HttpWebRequest request = getRequest(url, "POST");
			HttpWebResponse response = null;
		
			if (null != strParmater) {
				try {
					byte[] data = ENCODING.GetBytes(strParmater);
					using (Stream stream = request.GetRequestStream()) {  
						stream.Write(data, 0, data.Length);  
					}
					response = request.GetResponse() as HttpWebResponse;
				} catch (Exception ex) {
					Console.WriteLine(ex);//正式项目中请改为log打印
					request.Abort();
				}
			}
  
			return OpenReadWithHttps(response);
		}
	
		private static HttpWebRequest getRequest(string url, string method,string contentType = "application/x-www-form-urlencoded")
		{
			HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
			request.Method = method;//传输方式  
			request.ContentType = contentType;//协议                  
			request.UserAgent = DEFAULT_USER_AGENT;//请求的客户端浏览器信息,默认IE                  
			request.Timeout = CON_TIMEOUT;//连接超时时间	
			request.ReadWriteTimeout = READ_WRITE_TIMEOUT;//读写超时时间
		
			//如果是发送HTTPS请求    
			if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase)) {  
				ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;  
				request.ProtocolVersion = HttpVersion.Version10;  
			} 
			return request;
		}
	
		private static string getStrParameter(IDictionary<string,string> parameters)
		{
			if (!(parameters == null || parameters.Count == 0)) {  
				StringBuilder buffer = new StringBuilder();  
				int i = 0;  
				foreach (string key in parameters.Keys) {  
					if (i > 0) {  
						buffer.AppendFormat("&{0}={1}", key, parameters[key]);  
					} else {  
						buffer.AppendFormat("{0}={1}", key, parameters[key]);  
					}  
					i++;  
				}  
				return buffer.ToString();
			}
			return null;
		}
  
		//验证服务器证书回调自动验证
		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{  
			return true; //总是接受    
		}
  
		/// <summary>  
		/// 获取数据  
		/// </summary>  
		/// <param name="httpWebResponse">响应对象</param>  
		/// <returns>返回字符串</returns>  
		private static string OpenReadWithHttps(HttpWebResponse httpWebResponse)
		{  
			if (httpWebResponse == null) {
				return null;
			}
		
			Stream responseStream = null;  
			StreamReader sReader = null;  
			string value = null;  
			
			try { 	
				if (HttpStatusCode.OK.Equals(httpWebResponse.StatusCode)) {
					// 获取响应流  
					responseStream = httpWebResponse.GetResponseStream();  
					// 对接响应流(以"utf-8"字符集)  
					sReader = new StreamReader(responseStream, ENCODING);  
					// 开始读取数据  
					value = sReader.ReadToEnd();  
				}
			} catch (Exception ex) {  
				Console.WriteLine(ex); //正式项目中请改为log打印
			} finally {  
				//强制关闭  
				if (sReader != null) {  
					sReader.Close();  
				}  
				if (responseStream != null) {  
					responseStream.Close();  
				}  
				if (httpWebResponse != null) {  
					httpWebResponse.Close();  
				}  
			}  
  
			return value;  
		}
	}
}