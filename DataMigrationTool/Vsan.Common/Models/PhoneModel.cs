using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Models
{
    /// <summary>
    /// 用户绑定手机号
    /// </summary>
    public class PhoneModel
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        public string phoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        public string purePhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 区号（Senparc注：国别号）
        /// </summary>
        public string countryCode
        {
            get;
            set;
        }
    }
}
