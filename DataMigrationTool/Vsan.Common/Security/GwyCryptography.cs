using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Security
{
    /// <summary>
    /// 工务园加密解密
    /// </summary>
    public static class GwyCryptography
    {
        #region DES加解密Key

        /// <summary>
        /// DES加解密Key
        /// </summary>
        private static readonly string Key = "~@&^%$)(5625741uyudfsdfa";

        #endregion

        #region DES

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        //public static string DESEncrypt(string originalString, string key)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(key);
        //    Security.DesHelper desHelper = new Security.DesHelper(bytes);
        //    return desHelper.Encrypt(originalString);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        //public static string DESEncrypt(string originalString)
        //{
        //    return DESEncrypt(originalString, Key);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        //public static string DESDecrypt(string originalString, string key)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(key);
        //    Security.DesHelper desHelper = new Security.DesHelper(bytes);
        //    return desHelper.Decrypt(originalString);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        //public static string DESDecrypt(string originalString)
        //{
        //    return DESDecrypt(originalString, Key);
        //}

        #endregion

        #region 密码不可逆加密 EncryptPassword

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string EncryptPassword(string originalString)
        {
            originalString = Security.HashHelper.GetSha1("gwy" + originalString + "001");
            originalString = Security.HashHelper.GetMd5("001" + originalString + "gwy");
            return originalString;
        }

        #endregion

        #region MD5加密 GetMd5

        /// <summary>
        /// GetMd5
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string GetMd5(string originalString)
        {
            //byte[] bytes = Encoding.UTF8.GetBytes(originalString);
            //bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    sb.Append(bytes[i].ToString("x").PadLeft(2, '0'));
            //}
            //return sb.ToString();

            return HashHelper.GetMd5(originalString);
        }

        #endregion

        #region SHA1加密 GetSha1

        /// <summary>
        /// GetSha1
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string GetSha1(string originalString)
        {
            //byte[] bytes = Encoding.UTF8.GetBytes(originalString);
            //bytes = new SHA1CryptoServiceProvider().ComputeHash(bytes);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    sb.Append(bytes[i].ToString("x").PadLeft(2, '0'));
            //}
            //return sb.ToString();
            return HashHelper.GetSha1(originalString);
        }

        #endregion

        #region Base64Decrypt

        /// <summary>
        ///  Base64解密
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string Base64Decrypt(string originalString)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(originalString));
        }

        #endregion

        #region Base64Encrypt

        /// <summary>
        ///  Base64加密
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string Base64Encrypt(string originalString)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(originalString));
        }

        #endregion
    }
}
