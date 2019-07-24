using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Safety
{
    /// <summary>
    /// 签名类型
    /// </summary>
    public enum SignType
    {
        /// <summary>
        /// 工博
        /// </summary>
        GB,
        /// <summary>
        /// 工务园
        /// </summary>
        GWY,
        /// <summary>
        /// 微信
        /// </summary>
        WX,
        /// <summary>
        /// Auth1.0
        /// </summary>
        Auth,
        /// <summary>
        /// Auth2.0
        /// </summary>
        Auth2,
    }
    /// <summary>
    /// 验签器简单工厂
    /// </summary>
    public class SignVerFactory
    {
        /// <summary>
        /// 获取验签器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ISignaVerifyer GetSignaVerifyer(SignType type)
        {
            switch (type)
            {
                case SignType.GB:
                    return new GbSignaVerifyer();
                default:
                    return new GbSignaVerifyer();
            }
        }
    }
}
