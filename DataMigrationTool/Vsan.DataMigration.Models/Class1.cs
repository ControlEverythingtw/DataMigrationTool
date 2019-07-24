using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Models
{

    public class UserAccount
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        [EmailAddress]
        public string Account { get; set; }

        /// <summary>
        /// 验证码或者密码
        /// </summary>
        [Required]
        [MinLength(length:6)]
        public string Code { get; set; }

    }
}
