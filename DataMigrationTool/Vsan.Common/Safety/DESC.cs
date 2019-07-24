using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Safety
{
    /// <summary>
    /// 
    /// </summary>
    public class DESEncrypt
    {
        //35AE87F2-DE65-40D8-B4A6-1344813F3939
        public static string _securityKey = "1344813F";
        public static string _iv = "1344813F";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string EncryptParameter(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            try
            {
                string str = obj.ToString();
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                provider.Key = Encoding.UTF8.GetBytes(_securityKey);

                provider.IV = Encoding.UTF8.GetBytes(_securityKey);

                byte[] bytes = Encoding.UTF8.GetBytes(str);

                MemoryStream stream = new MemoryStream();

                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);

                stream2.Write(bytes, 0, bytes.Length);

                stream2.FlushFinalBlock();

                foreach (byte num in stream.ToArray())
                {

                    builder.AppendFormat("{0:X2}", num);
                }

                stream.Close();

            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
            return builder.ToString();

        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecryptParameter(string str)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                provider.Key = Encoding.ASCII.GetBytes(_securityKey);

                provider.IV = Encoding.ASCII.GetBytes(_securityKey);

                byte[] buffer = new byte[str.Length / 2];

                for (int i = 0; i < (str.Length / 2); i++)
                {

                    int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);

                    buffer[i] = (byte)num2;

                }

                MemoryStream stream = new MemoryStream();

                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);

                stream2.Write(buffer, 0, buffer.Length);

                stream2.FlushFinalBlock();

                stream.Close();

                return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
            }
            catch(Exception ex)
            {
                Logger.Instance.Error(ex);
                return string.Empty;
            }
        }

    }
    /// <summary>
    /// DES加解密
    /// </summary>
    public class DES
    {
        //35AE87F2-DE65-40D8-B4A6-1344813F3939
        private readonly string _securityKey ;
        private readonly string _iv ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityKey"></param>
        /// <param name="iv"></param>
        public DES(string securityKey = "35AE87F2", string iv = "13448139")
        {
            _securityKey = securityKey;
            _iv = iv;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public  string EncryptParameter(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            try
            {
                string str = obj.ToString();
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                provider.Key = Encoding.UTF8.GetBytes(_securityKey);

                provider.IV = Encoding.UTF8.GetBytes(_securityKey);

                byte[] bytes = Encoding.UTF8.GetBytes(str);

                MemoryStream stream = new MemoryStream();

                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);

                stream2.Write(bytes, 0, bytes.Length);

                stream2.FlushFinalBlock();

                foreach (byte num in stream.ToArray())
                {

                    builder.AppendFormat("{0:X2}", num);
                }

                stream.Close();

            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
            return builder.ToString();

        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public  string DecryptParameter(string str)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                provider.Key = Encoding.ASCII.GetBytes(_securityKey);

                provider.IV = Encoding.ASCII.GetBytes(_iv);

                byte[] buffer = new byte[str.Length / 2];

                for (int i = 0; i < (str.Length / 2); i++)
                {

                    int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);

                    buffer[i] = (byte)num2;

                }

                MemoryStream stream = new MemoryStream();

                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);

                stream2.Write(buffer, 0, buffer.Length);

                stream2.FlushFinalBlock();

                stream.Close();

                return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
                return string.Empty;
            }
        }

    }
}
