using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Models
{
    /// <summary>
    /// 缓存的Key
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// 错误次数
        /// </summary>
        public static readonly string VCodeErrorCount = "error_count_{0}_{1}";
        /// <summary>
        /// 是否验证过
        /// </summary>
        public static readonly string VCodeIsOk = "VCodeIsOk_{0}_{1}_{2}";
        /// <summary>
        /// 短信平台返回的消息ID
        /// </summary>
        public static readonly string VCode = "VCode_{0}_{1}";
        /// <summary>
        /// 使用次数
        /// </summary>
        public static string VCodeUseCount = "VCodeUseCount_{0}_{1}";

        /// <summary>
        /// TestAPI 记录
        /// </summary>
        public static string TestRecord = "TestRecord_{0}";
        /// <summary>
        /// TestAPI 记录所有键
        /// </summary>
        public static string TestRecordKeyList = "TestRecordKeyList";

        /// <summary>
        /// 上传次数限制
        /// </summary>
        public static string UploadCount = "UploadCount_{0}";

        /// <summary>
        /// 允许发送间隔 单位秒
        /// </summary>
        public static string AllowSendInterval = "AllowSendInterval_{0}_{1}";

        /// <summary>
        /// 用户小程序FromId
        /// </summary>
        public static string UserFromId = "UserFromId_{0}";

        /// <summary>
        /// 微信的访问令牌
        /// </summary>
        public static string  Wx_AccessToken = "wxAccessToken";

        /// <summary>
        /// 下载限制
        /// </summary>
        public static string DownloadCount = "DownloadCount_{0}";
    }
}
