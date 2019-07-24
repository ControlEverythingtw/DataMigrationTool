using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Safety
{
    public class Base64
    {
        /// <summary>
        /// 加密算法
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string text, string key)
        {
            StringBuilder strCode = new StringBuilder();
            string fusionKey = MD5(key);
            int len = text.Length; int d = 0;
            int Index = fusionKey.Length;
            for (int i = 0; i < len; i++)
            {
                if (d == Index)
                {
                    d = 0;
                }
                strCode.Append(fusionKey.Substring(d, 1));
                d++;
            }
            byte[] value = new byte[len];
            for (int i = 0; i < len; i++)
            {
                value[i] = (byte)(System.Text.Encoding.ASCII.GetBytes(text.Substring(i, 1))[0] + System.Text.Encoding.ASCII.GetBytes(strCode.ToString().Substring(i, 1))[0] % 256);
            }

            string data = EncodeBase64(System.Text.Encoding.ASCII, value);
            return data;
        }

        /// <summary>
        /// 解密算法
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string text, string key)
        {
            StringBuilder strCode = new StringBuilder();
            StringBuilder strData = new StringBuilder();
            string fusionKey = MD5(key);
            byte[] data = DecodeBase64(text);
            int len = data.Length; int d = 0;
            int Index = fusionKey.Length;
            for (int i = 0; i < len; i++)
            {
                if (d == Index)
                {
                    d = 0;
                }
                strCode.Append(fusionKey.Substring(d, 1));
                d++;
            }
            for (int i = 0; i < len; i++)
            {
                if (data[i] < System.Text.Encoding.ASCII.GetBytes(strCode.ToString().Substring(i, 1))[0])
                {
                    strData.Append(Convert.ToChar((data[i] + 256) - System.Text.Encoding.ASCII.GetBytes(strCode.ToString().Substring(i, 1))[0]));
                }
                else
                {
                    strData.Append(Convert.ToChar(data[i] - System.Text.Encoding.ASCII.GetBytes(strCode.ToString().Substring(i, 1))[0]));
                }
            }
            return strData.ToString();
        }

        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                return "";
            }
        }

        /// <summary> 
        /// Base64加密 
        /// </summary> 
        /// <param name="codeName">加密采用的编码方式</param> 
        /// <param name="source">待加密的明文</param> 
        /// <returns></returns> 
        public static string EncodeBase64(Encoding encode, string source)
        {
            string strcode = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                strcode = Convert.ToBase64String(bytes);
            }
            catch
            {
                strcode = source;
            }
            return strcode;
        }
        /// <summary> 
        /// Base64解密 
        /// </summary> 
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string EncodeBase64(Encoding encode, byte[] source)
        {
            string strcode = "";
            try
            {
                strcode = Convert.ToBase64String(source);
            }
            catch
            {
                strcode = "";
            }
            return strcode;
        }
        /// <summary> 
        /// Base64解密 
        /// </summary> 
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        /// <summary> 
        /// Base64解密 
        /// </summary> 
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static byte[] DecodeBase64(string result)
        {
            byte[] bytes = Convert.FromBase64String(result);
            return bytes;
        }
    }
}
