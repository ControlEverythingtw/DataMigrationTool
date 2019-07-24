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
    /// Md5
    /// </summary>
    public  class Md5Hepler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetValue(string str)
        {
            var md5 = MD5.Create();
            var buffer = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] source = md5.ComputeHash(buffer);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("x2"));
            }
            md5.Dispose();
            return sBuilder.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string GetValue(Stream stream)
        {
            var md5 = MD5.Create();
            byte[] source = md5.ComputeHash(stream);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("x2"));
            }
            md5.Dispose();
            return sBuilder.ToString();
        }
    }
}

