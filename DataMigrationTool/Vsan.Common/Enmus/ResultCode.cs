using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 返回码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error = 1,
        /// <summary>
        /// 参数异常
        /// </summary>

        [Description("参数异常")] ArgumentException = 2,

        /// <summary>
        /// 签名异常
        /// </summary>
        [Description("签名异常")] SignVerifyException = 3,
        /// <summary>
        /// 抛出未处理的异常
        /// </summary>
        [Description("抛出未处理的异常")] Exception = 4,
        /// <summary>
        /// 找不到
        /// </summary>
        [Description("找不到")] NotFind = 5,
        /// <summary>
        /// 已存在
        /// </summary>
        [Description("已存在")] IsExists = 6,
        /// <summary>
        /// 没有值
        /// </summary>
        [Description("没有值")] NoValue = 7,
        /// <summary>
        /// 授权过期
        /// </summary>
        [Description("授权过期")] AuthExpire = 8,
        /// <summary>
        /// 验证码过期
        /// </summary>
        [Description("验证码过期")] VCodeExpire = 9,
        /// <summary>
        /// 验证码不正确
        /// </summary>
        [Description("验证码不正确")] VCodeError = 10,
        /// <summary>
        /// 错误次数超出允许次数
        /// </summary>
        [Description("错误次数超出允许次数")] ErrorCountExceed = 11,
        /// <summary>
        /// 短信平台响应错误
        /// </summary>
        [Description("短信平台响应错误")] Error_In_SMS_Platform = 12,
        /// <summary>
        /// 没有任何更改
        /// </summary>
        [Description("没有任何更改")] NoChange = 13,
        /// <summary>
        /// 微信登录失效
        /// </summary>
        [Description("微信登录失效")] WxLoginExpire = 14,
        /// <summary>
        /// 微信一键登录失败
        /// </summary>
        [Description("微信一键登录失败")] OneLoginError = 15,
        /// <summary>
        /// 微信一键登录-解密失败
        /// </summary>
        [Description("微信一键登录-解密失败")] OneLoginEncryptedDataError = 16,
        /// <summary>
        /// 文件上传失败
        /// </summary>
        [Description("文件上传失败")] FileUploadError = 17,
        /// <summary>
        /// 文件上传限制
        /// </summary>
        [Description("文件上传限制")] UploadLimit = 18,
        /// <summary>
        /// 数量不够
        /// </summary>
        [Description("数量不够")] Lack = 19,
        /// <summary>
        /// 没有使用
        /// </summary>
        [Description("没有使用")] NotUse = 20,
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")] Used = 21,
        /// <summary>
        /// 没有开通
        /// </summary>
        [Description("没有开通")] NotOpen = 22,
        /// <summary>
        /// 超过限制次数(频繁操作)
        /// </summary>
        [Description("超过限制次数")] AllowMaxSendCount = 23,
        /// <summary>
        /// 格式错误
        /// </summary>
        [Description("格式错误")] FormatError = 24,
        /// <summary>
        /// 没有数据
        /// </summary>
        [Description("没有数据")] NotData = 25,
        /// <summary>
        /// 没有入职
        /// </summary>
        [Description("没有入职")] NotJob = 26,
        /// <summary>
        /// 密码错误
        /// </summary>
        [Description("密码错误")] PwdError = 27,
        /// <summary>
        /// 密码错误次数超出
        /// </summary>
        [Description("密码错误次数超出")] PwdErrorCountExceed = 28,
        /// <summary>
        /// 验证码为空
        /// </summary>
        [Description("验证码为空")] VCodeIsNull = 29,
        /// <summary>
        /// 时间间隔之内不予许
        /// </summary>
        [Description("时间间隔之内不予许")] NotAllowedForTimeIntervals = 30,
        /// <summary>
        /// 没有过期
        /// </summary>
        [Description("没有过期")] NotExpire = 31,
        /// <summary>
        /// 加密过期
        /// </summary>
        [Description("加密过期")] EncryptExpire = 32,
        /// <summary>
        /// 账号已被封号
        /// </summary>
        [Description("账号已被封号")] AccountLock = 33,
        /// <summary>
        /// 解密数据失败
        /// </summary>
        [Description("解密数据失败")] DecryptFailed = 34,
        /// <summary>
        /// 执行存储过程失败
        /// </summary>
        [Description("执行存储过程失败")] ExecStoredProcedureError = 35,

        /// <summary>
        /// 手机号未登录
        /// </summary>
        [Description("手机号未注册")] MobileNotLogin = 36,

        /// <summary>
        /// 图片验证码错误
        /// </summary>
        [Description("图片验证码错误")] ImgVerifyCodeError = 37,
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")] InReview = 38,
        /// <summary>
        /// 不完善
        /// </summary>
        [Description("不完善")] Incomplete = 39,

        /// <summary>
        /// 拒绝访问
        /// </summary>
        [Description("拒绝访问")] AccessDenied = 40,

        /// <summary>
        /// 审核不通过
        /// </summary>
        [Description("审核不通过")] Rejected = 41,
        /// <summary>
        /// 用户已在其他设备/浏览器上登录
        /// </summary>
        [Description("用户已在其他设备/浏览器上登录")]
        MultiUserLogin = 42,
        /// <summary>
        ///密文无效
        /// </summary>
        [Description("密文无效")]
        CipherIsInvalid = 43,
        /// <summary>
        ///Api调用顺序出错
        /// </summary>
        [Description("Api调用顺序出错")]
        ApiOrderError = 44,
        /// <summary>
        ///下载限制
        /// </summary>
        [Description("下载限制")]
        DownloadCount = 45,
        /// <summary>
        ///门店封禁
        /// </summary>
        [Description("门店封禁")]
        ShopIsLock = 46,
        /// <summary>
        /// 无操作权限
        /// </summary>
        [Description("无操作权限")]
        NoPermission = 47
    }

}
