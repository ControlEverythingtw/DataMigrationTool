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
    /// 哈希算法
    /// </summary>
    public class SHA256Helper
    {
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetValue(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256.Create().ComputeHash(bytes);
            return GetValue(hash);
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetValue(Stream data)
        {
            byte[] hash = SHA256.Create().ComputeHash(data);
            return GetValue(hash);
        }
        private static string GetValue(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2"));
            }
            return builder.ToString();
        }
    }
}
