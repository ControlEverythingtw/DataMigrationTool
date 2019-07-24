using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vsan.Common
{
    
    /// <summary>
    /// 字符串扩展一些验证方法
    /// </summary>
     public static class StringVerify
    {

        /// <summary>
        /// 检查值是否在某个离散范围内.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        public static bool In(this string instance, params string[] ranges)
        {
            return ranges.Contains(instance);
        }

        /// <summary>
        /// 验证电话号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTelephone(this string str)
        {
            return Regex.IsMatch(str, @"^(\d{3,4}-)?\d{6,8}$");
        }
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(this string str)
        {
            return Regex.IsMatch(str, @"^[1]+[3-9]+\d{9}");
        }
        /// <summary>
        /// 验证生份证
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIDcard(this string str)
        {
            return Regex.IsMatch(str, @"(^\d{18}$)|(^\d{15}$)");
        }
        /// <summary>
        /// 是不是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            return Regex.IsMatch(str, @"^[0-9]*$");
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPostalcode(this string str)
        {
            return Regex.IsMatch(str, @"^\d{6}$");
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="str_Email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string src)
        {
            return Regex.IsMatch(src, @"\\w{1,}@\\w{1,}\\.\\w{1,}");
        }
    }

    /// <summary>
    /// 图片验证码配置
    /// </summary>
    public class ImgVCodeConfig
    {

        /// <summary>
        /// 是否随机长度
        /// </summary>
        public bool IsRandomLength { get; set; } = false;

        /// <summary>
        /// 随机字串
        /// </summary>
        public string RandomString { get; set; } = "2356782356789ZXCVBNMASDFGHJKQWERTYUP2356789";


    }

    /// <summary>
    /// 
    /// </summary>
    public  class StringHelper:IBusinessConfig
    {

        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadConfig()
        {
            _Config = ConfigHelper.Get<ImgVCodeConfig>();
        }

        /// <summary>
        /// 配置
        /// </summary>
        private static  ImgVCodeConfig _Config = ConfigHelper.Get<ImgVCodeConfig>();

        /// <summary>
        /// 
        /// </summary>
        private static Random _random = new Random();

        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetVCodeStr(int len = 4)
        {
            if (_Config.IsRandomLength)
            {
                //随机长度
                len = _random.Next(len - 1, len + 1);
            }
            char[] chars = _Config.RandomString.ToArray();
            char[] vCodes = new char[len];
            for (int i = 0; i < vCodes.Length; i++)
            {
                vCodes[i] = chars[_random.Next(0, chars.Length - 1)];
            }
            string VCode = new String(vCodes);
            return VCode;
        }
      
        /// <summary>
        /// 批量判断是否为空
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static bool IsNull(params string[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(strs[i]))
                {
                    return false;
                }
            }
            return true;
        }

      
    }

    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtends
    {
        /// <summary>
        /// 获取s开头e结尾中间的值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetValue(this string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
    }


    /// <summary>
    /// 语言切换
    /// </summary>
    public static class StringLanguage
    {
        public static string L(this string key)
        {
            return key;

        }
    }
    /// <summary>
    /// 语言枚举
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// 中文
        /// </summary>
        ZH_CN,
        /// <summary>
        /// 英文
        /// </summary>
        EN,

    }
    /// <summary>
    /// 语言配置
    /// </summary>
    public class LanguageConfig
    {

    }
}
