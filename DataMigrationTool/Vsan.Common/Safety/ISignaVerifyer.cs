using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Safety
{
    /// <summary>
    /// 验证签名接口
    /// </summary>
    public interface ISignaVerifyer
    {



        CommonParamModel Sign(List<string> paramArr);
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="time">时间戳</param>
        /// <param name="token"></param>
        /// <param name="signa"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        bool Verify(string time, string token, string signa, params string[] paramArr);

        /// <summary>
        /// 旧版本验证方法
        /// </summary>
        /// <param name="actionArguments"></param>
        /// <returns></returns>
        bool VerifyOld(Dictionary<string, object> actionArguments);

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="time">时间戳</param>
        /// <param name="token"></param>
        /// <param name="signa"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        void VerifyEx(string time, string token, string signa, params string[] paramArr);

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="token"></param>
        /// <param name="sign"></param>
        /// <param name="model">模型</param>
        void VerifyEx<T>(string timestamp, string token, string sign, T model);

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="actionArguments">参数</param>
        /// <returns></returns>
        bool Verify(Dictionary<string, object> actionArguments);
        /// <summary>
        /// 空值需要签名
        /// </summary>
        /// <param name="actionArguments">参数</param>
        /// <returns></returns>
        bool VerifyOldNull(Dictionary<string, object> actionArguments);
    }
}
