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
        /// 邮箱验证码
        /// </summary>
        [MinLength(length:4)]
        public string VerifyCode_Email { get; set; }
        /// <summary>
        /// 图形验证码
        /// </summary>
        [Required]
        [MinLength(length: 4)]
        public string VerifyCode_Image { get; set; }
        /// <summary>   
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        public int UserId { get; set; }
    }
}
