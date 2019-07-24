using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Safety
{
    /// <summary>
    /// 微信AES解密
    /// </summary>
    public class WeiXinAES
    {
        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="inputdata">输入的数据encryptedData</param>  
        /// <param name="AesKey">key</param>  
        /// <param name="AesIV">向量128</param>  
        /// <returns name="result">解密后的字符串</returns>  
        public static string AESDecrypt(string AesKey,string AesIV, string inputdata)
        {
            AesIV = AesIV.Replace(" ", "+");
            AesKey = AesKey.Replace(" ", "+");
            inputdata = inputdata.Replace(" ", "+");
            byte[] encryptedData = Convert.FromBase64String(inputdata);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Key = Convert.FromBase64String(AesKey); // Encoding.UTF8.GetBytes(AesKey);  
            rijndaelCipher.IV = Convert.FromBase64String(AesIV);// Encoding.UTF8.GetBytes(AesIV);  
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            string result = Encoding.UTF8.GetString(plainText);
            return result;
        }
    }
}
