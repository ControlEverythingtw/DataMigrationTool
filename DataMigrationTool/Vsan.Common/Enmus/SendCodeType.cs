using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Enmus
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum SendCodeType
    {
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login,
        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        Register,
        /// <summary>
        /// 找回密码
        /// </summary>
        [Description("找回密码")]
        FindPwd,
        /// <summary>
        /// 修改密码
        /// </summary>
        [Description("修改密码")]
        ChangePwd,
        /// <summary>
        /// B端注册企业账号 临时使用
        /// </summary>
        [Description("B端注册企业账号（临时使用）")]
        BReg,
        /// <summary>
        ///修改/找回交易密码
        /// </summary>
        [Description("修改/找回交易密码")]
        ChangePayPassword,

        /// <summary>
        /// 一键求职
        /// </summary>
        [Description("一键求职")]
        OneKeyJob,
        /// <summary>
        /// 绑定银行卡
        /// </summary>
        [Description("绑定银行卡")]
        BindBank,
        /// <summary>
        /// 解绑银行卡
        /// </summary>
        [Description("解绑银行卡")]
        UnbindBank,

        /// <summary>
        /// 提现
        /// </summary>
        [Description("提现")]
        Withdraw,


        /// <summary>
        /// 在线离职登记
        /// </summary>
        [Description("在线离职登记")]
        OnlineLeave,

        /// <summary>
        /// PC机构后台登录
        /// </summary>
        [Description("后台登录")]
        AdminLogin,
        /// <summary>
        /// PC机构后台注册
        /// </summary>
        [Description("PC机构后台注册")]
        AdminRegister,
        /// <summary>
        /// PC机构后台找回密码
        /// </summary>
        [Description("PC机构后台找回密码")]
        AdminFindPassword,

        /// <summary>
        /// PC机构后台重新绑定手机号码
        /// </summary>
        [Description("PC机构后台重新绑定手机号码")]
        AdminRestMobile,

        /// <summary>
        /// PC机构后台_重新绑定手机号码_验证新手机号
        /// </summary>
        [Description("PC机构后台_重新绑定手机号码_验证新手机号")]
        AdminRestMobileToNewMobile,

        /// <summary>
        /// H5端注册
        /// </summary>
        [Description("H5端注册")]
        H5Register,

        /// <summary>
        /// 个人端更换手机号码
        /// </summary>
        [Description("个人端更换手机号码")]
        ChangeMobile,

        /// <summary>
        /// 预付款提现
        /// </summary>
        [Description("预付款提现")]
        AdvanceWithdraw
    }
}
