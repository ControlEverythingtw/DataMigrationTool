using SharePasswordProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Safety
{
    public class Pwd
    {
        public static string Encrypt(string pwd)
        {
            return Convert.ToBase64String(new DESPasswordProvider().Encrypt(pwd));
        }
        public static string Decrypt(string pwd)
        {
            return new DESPasswordProvider().DESDecrypt(pwd);
        }
    }
}
