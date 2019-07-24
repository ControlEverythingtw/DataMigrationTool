using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Safety
{
    /// <summary>
    /// 工博签名验证器
    /// </summary>
    public class GbSignaVerifyer : ISignaVerifyer
    {
        public CommonParamModel Sign(List<string> paramArr)
        {
            CommonParamModel commonParam = new CommonParamModel();
            string key = System.Configuration.ConfigurationManager.AppSettings["WS_Pass"];
            commonParam.timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string str = string.Format("timestamp={0}&key={1}", commonParam.timestamp, key);
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            string newvalue = sb.ToString().ToUpper();
            newvalue = newvalue.Substring(newvalue.Length - 1, 1) + newvalue.Substring(1, newvalue.Length - 2) + newvalue.Substring(0, 1);
            var token = commonParam.token = newvalue;
            StringBuilder sbvalue = new StringBuilder();
            sbvalue.AppendFormat("token={0}", token);
            for (int i = 0; i < paramArr.Count; i++)
            {
                sbvalue.AppendFormat("&{0}", paramArr[i]);
            }
            //if (sbvalue.ToString().IndexOf("from=miniprogram") >= 0 && sbvalue.ToString().IndexOf("[{") >= 0) //针对小程序特殊处理
            //    return true;
            //MD5加密
            var bs2 = md5.ComputeHash(Encoding.UTF8.GetBytes(sbvalue.ToString()));
            var sb2 = new StringBuilder();
            foreach (byte b in bs2)
            {
                sb2.Append(b.ToString("x2"));
            }
            string newvalue2 = sb2.ToString().ToUpper();
            newvalue2 = newvalue2.Substring(newvalue2.Length - 1, 1) + newvalue2.Substring(1, newvalue2.Length - 2) + newvalue2.Substring(0, 1);
            commonParam.sign = newvalue2;
            return commonParam;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="time"></param>
        /// <param name="token"></param>
        /// <param name="signa"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public bool Verify(string time, string token, string signa, params string[] paramArr)
        {
            if (!StringHelper.IsNull(time,token,signa))
            {
                throw new ArgumentException("签名失败 time、token、signa 都不能为空");
            }
            var timestamp = DateTime.Parse(time);
            TimeSpan ts = DateTime.Now - timestamp;
            if (ts.TotalSeconds > 120)
                throw new ArgumentException("签名时间过期");

            string key = System.Configuration.ConfigurationManager.AppSettings["WS_Pass"];
            //转url格式
            string str = string.Format("timestamp={0}&key={1}", timestamp.ToString("yyyy/MM/dd HH:mm:ss"), key);
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            string newvalue = sb.ToString().ToUpper();
            newvalue = newvalue.Substring(newvalue.Length - 1, 1) + newvalue.Substring(1, newvalue.Length - 2) + newvalue.Substring(0, 1);
            if (newvalue.Equals(token))
            {
                StringBuilder sbvalue = new StringBuilder();
                sbvalue.AppendFormat("token={0}", token);
                for (int i = 0; i < paramArr.Length; i++)
                {
                    if (paramArr[i] == null)
                    {
                        continue;
                    }
                    sbvalue.AppendFormat("&{0}", paramArr[i]);
                }
                if (sbvalue.ToString().IndexOf("from=miniprogram") >= 0 && sbvalue.ToString().IndexOf("[{") >= 0) //针对小程序特殊处理
                    return true;
                //MD5加密
                var bs2 = md5.ComputeHash(Encoding.UTF8.GetBytes(sbvalue.ToString()));
                var sb2 = new StringBuilder();
                foreach (byte b in bs2)
                {
                    sb2.Append(b.ToString("x2"));
                }
                string newvalue2 = sb2.ToString().ToUpper();
                newvalue2 = newvalue2.Substring(newvalue2.Length - 1, 1) + newvalue2.Substring(1, newvalue2.Length - 2) + newvalue2.Substring(0, 1);
                return newvalue2.Equals(signa);
            }
            return false;
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="actionArguments"></param>
        /// <returns></returns>
        public bool Verify(Dictionary<string, object> actionArguments)
        {
            object paramObj = actionArguments.Values.FirstOrDefault();
            if (paramObj == null) return false;
            var prop = paramObj.GetType().GetProperties();
            CommonParamModel common = prop[1].GetValue(paramObj) as CommonParamModel;
            if (common == null) return false;
            object data = prop[0].GetValue(paramObj);
            var type = data.GetType();
            if (type.IsPrimitive || type.Name.Equals("String"))
            {
                return Verify(common.timestamp, common.token, common.sign, data.ToString());
            }
            if (type.Name.Equals("JObject"))
            {
                var jobj = data as JObject;
                var strArr = new List<string>();
                foreach (var item in jobj)
                {
                    strArr.Add(item.Value.ToString());
                }
                return Verify(common.timestamp, common.token, common.sign, strArr.ToArray());
            }
            var paramArr = type.GetProperties().Select(a => a.GetValue(data)?.ToString()).ToArray();
            return Verify(common.timestamp, common.token, common.sign, paramArr);

        }

        public void VerifyEx(string time, string token, string signa, params string[] paramArr)
        {
            var isOk = Verify(time, token, signa, paramArr);
            if (!isOk)
            {
                Logger.Instance.Info($"签名验证失败 time={time},token={token},signa={signa} \n参数:");
                Logger.Instance.Print(paramArr);
                throw new Exception("签名验证失败");
            }
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timestamp"></param>
        /// <param name="token"></param>
        /// <param name="sign"></param>
        /// <param name="model"></param>
        public void VerifyEx<T>(string timestamp, string token, string sign, T model)
        {
            var type = model.GetType();
            var prop = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            var values = prop.Select(a => a.GetValue(model)?.ToString()).ToArray();
            var isOk = Verify(timestamp, token, sign, values);
            if (!isOk)
            {
                Logger.Instance.Info($"签名验证失败 time={timestamp},token={token},signa={sign} \n参数:");
                Logger.Instance.Print(values);
                throw new Exception("签名验证失败");
            }
        }

        public bool VerifyOld(Dictionary<string, object> actionArguments)
        {
            object paramObj = actionArguments.Values.FirstOrDefault();
            if (paramObj == null) return false;
            var prop = paramObj.GetType().GetProperties();
            string timestamp = null, token = null, sign = null;
            var values = new string[prop.Length - 3];
            var valuesIndex = 0;
            for (int i = 0; i < prop.Length; i++)
            {
                if (prop[i].Name.Equals("timestamp")) timestamp = prop[i].GetValue(paramObj)?.ToString();
                else if (prop[i].Name.Equals("token")) token = prop[i].GetValue(paramObj)?.ToString();
                else if (prop[i].Name.Equals("sign")) sign = prop[i].GetValue(paramObj)?.ToString();
                else
                {
                    if (prop[i].PropertyType.BaseType.Name.Equals("Enum"))
                    {
                        values[valuesIndex] = ((int)prop[i].GetValue(paramObj)).ToString();
                    }
                    else
                    {
                        values[valuesIndex] = prop[i].GetValue(paramObj)?.ToString();
                    }

                    valuesIndex++;
                }
            }
            var isOk = Verify(timestamp, token, sign, values);
            if (!isOk)
            {
                Logger.Instance.Info($"【VerifyOld】 签名验证失败  time={timestamp},token={token},signa={sign} \n参数:");
                Logger.Instance.Print(values);
            }
            return isOk;
        }

        public bool VerifyOldNull(Dictionary<string, object> actionArguments)
        {
            object paramObj = actionArguments.Values.FirstOrDefault();
            if (paramObj == null) return false;
            // Logger.Instance.Print(paramObj);
            var prop = paramObj.GetType().GetProperties();
            string timestamp = null, token = null, sign = null;
            var values = new string[prop.Length - 3];
            var valuesIndex = 0;
            for (int i = 0; i < prop.Length; i++)
            {
                if (prop[i].Name.Equals("timestamp")) timestamp = prop[i].GetValue(paramObj)?.ToString();
                else if (prop[i].Name.Equals("token")) token = prop[i].GetValue(paramObj)?.ToString();
                else if (prop[i].Name.Equals("sign")) sign = prop[i].GetValue(paramObj)?.ToString();
                else
                {
                    if (prop[i].PropertyType.BaseType.Name.Equals("Enum"))
                    {
                        values[valuesIndex] = ((int)prop[i].GetValue(paramObj)).ToString();
                    }
                    else
                    {
                        values[valuesIndex] = prop[i].GetValue(paramObj)?.ToString();
                    }

                    valuesIndex++;
                }
            }
            var isOk = VerifyNull(timestamp, token, sign, values);
            if (!isOk)
            {
                Logger.Instance.Info($"【VerifyOldNull】 签名验证失败  time={timestamp},token={token},signa={sign} \n参数:");
                Logger.Instance.Print(values);
                throw new Exception("签名验证失败");
            }
            return isOk;
        }
        public static bool verifytoken(DateTime timestamp, string token)
        {
            TimeSpan ts = DateTime.Now - timestamp;
            if (ts.TotalSeconds > 120)
                throw new ArgumentException("签名过期，请重新签名");
            string key = System.Configuration.ConfigurationManager.AppSettings["WS_Pass"];
            //转url格式
            string str = string.Format("timestamp={0}&key={1}", timestamp.ToString("yyyy/MM/dd HH:mm:ss"), key);
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            string newvalue = sb.ToString().ToUpper();
            newvalue = newvalue.Substring(newvalue.Length - 1, 1) + newvalue.Substring(1, newvalue.Length - 2) + newvalue.Substring(0, 1);
            return newvalue.Equals(token);
        }
        public static bool verifysign(string token, string sign, params string[] paras)
        {
            if (paras.Length == 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(sign))
                return false;
            StringBuilder sbvalue = new StringBuilder();
            sbvalue.AppendFormat("token={0}", token);
            for (int i = 0; i < paras.Length; i++)
            {
                sbvalue.AppendFormat("&{0}", paras[i]);
            }
            var sbStr = sbvalue.ToString();
            if (sbStr.IndexOf("from=miniprogram") >= 0 && sbStr.IndexOf("[{") >= 0) //针对小程序特殊处理
                return true;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(sbvalue.ToString()));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            string newvalue = sb.ToString().ToUpper();
            newvalue = newvalue.Substring(newvalue.Length - 1, 1) + newvalue.Substring(1, newvalue.Length - 2) + newvalue.Substring(0, 1);
            var isok = newvalue.Equals(sign);
            if (!isok)
            {
                Logger.Instance.Info("拼接的字符串\t" + sbStr);
                Logger.Instance.Info("验签的签名\t" + newvalue);
                Logger.Instance.Info("传过来的签名\t" + sign);
            }
            return isok;
        }
        private bool VerifyNull(string time, string token, string signa, string[] paramArr)
        {
            if (!verifytoken(Convert.ToDateTime(time),token))
            {
                throw new ArgumentException("Token 验证失败");
            }
            return verifysign(token, signa, paramArr);
        }

        ///// <summary>
        ///// 验证签名
        ///// </summary>
        ///// <param name="actionArguments"></param>
        ///// <returns></returns>
        //public bool Verify(Dictionary<string, object> actionArguments)
        //{
        //    object paramObj = null;
        //    var hasParam = actionArguments.TryGetValue("param", out paramObj);
        //    object obj = null;
        //    var hasComm = actionArguments.TryGetValue("common", out obj);
        //    if (!hasComm) return false;
        //    CommonParamModel common = obj as CommonParamModel;
        //    object obj2 = null;
        //    var hasData = actionArguments.TryGetValue("data", out obj2);
        //    if (!hasData) return false;
        //    var paramArr = obj2.GetType().GetProperties().Select(a => a.GetValue(obj2)?.ToString()).ToArray();
        //    return Verify(common.timestamp, common.token, common.sign, paramArr);
        //}
    }
}
