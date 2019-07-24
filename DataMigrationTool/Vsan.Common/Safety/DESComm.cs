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
    /// 和苹果端匹配的加密算法
    /// </summary>
    public class DESComm
    {

        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <param name="source">明文</param>
        /// <param name="_DESKey">密钥</param>
        /// <param name="strDESIV">向量</param>
        /// <returns>密文</returns>
        public static string Encode(string source, string _DESKey,string strDESIV)
            {
                StringBuilder sb = new StringBuilder();
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    byte[] key = ASCIIEncoding.ASCII.GetBytes(_DESKey);
                    byte[] iv = ASCIIEncoding.ASCII.GetBytes(strDESIV);
                    byte[] dataByteArray = Encoding.UTF8.GetBytes(source);
                    des.Mode = System.Security.Cryptography.CipherMode.CBC;
                    des.Key = key;
                    des.IV = iv;
                    string encrypt = "";
                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(ms.ToArray());
                    }
                    return encrypt;
                }
            }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="source">要解密的base64串</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <param name="strDESIV">IV，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decode(string source, string sKey, string strDESIV)
            {
                byte[] inputByteArray = System.Convert.FromBase64String(source);//Encoding.UTF8.GetBytes(source);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = Encoding.UTF8.GetString(ms.ToArray());
                    ms.Close();
                    return str;
                }
            }
    }
}
