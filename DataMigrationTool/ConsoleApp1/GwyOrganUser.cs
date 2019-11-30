using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ConsoleApp1
{
    /// <summary>
    /// 机构用户 
    /// 2019-10-09 15:46:47 062
    /// </summary>
    public class GwyOrganUser
    {
        /// <summary>
        /// 前端注册的直接用cms_shop的uuid,后台添加的用户newId()
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime createDate { get; set; }

        /// <summary>
        /// 用户名（前台审核的直接从cms_shop带入）
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 手机号（登录账号）
        /// </summary>
        public string userTel { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string userPwd { get; set; }

        /// <summary>
        /// 存入用户所属的角色json,所有直接为{\"all\"}
        /// </summary>
        public string roleContent { get; set; }

        /// <summary>
        /// 所属企业ID
        /// </summary>
      
        public Guid? enterpriseId { get; set; }

        /// <summary>
        /// 用户状态（0：可用  1：停用）
        /// </summary>
      
        public int? status { get; set; }

        /// <summary>
        /// 角色代码  (B=企业;AB=劳务;AP=合伙人)
        /// </summary>
        public string roleType { get; set; }

        /// <summary>
        /// 添加人用户Id
        /// </summary>
        public Guid? createUserId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 0：男  1：女
        /// </summary>
        public int? sex { get; set; }


    }
    public class CMS_SHOP
    {

        /// <summary>
        /// 前端注册的直接用cms_shop的uuid,后台添加的用户newId()
        /// </summary>
        public string uid { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 用户名（前台审核的直接从cms_shop带入）
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 手机号（登录账号）
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户状态（0：可用  1：停用）
        /// </summary>
        public bool? status { get; set; }

        /// <summary>
        /// 角色代码  (B=企业;AB=劳务;AP=合伙人)
        /// </summary>
        public string ShopType { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string E_Mail { get; set; }

        /// <summary>
        /// 0：男  1：女
        /// </summary>
        public string sex { get; set; }
    }
}
