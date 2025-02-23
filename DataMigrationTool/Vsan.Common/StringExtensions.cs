﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vsan.Common
{
    /// <summary>
    /// 字符串扩展辅助操作类
    /// </summary>
    public static class StringExtensions
    {
        #region 正则表达式

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false</returns>
        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>一个对象，包含有关匹配项的信息</returns>
        public static string Match(this string value, string pattern)
        {
            if (value == null)
            {
                return null;
            }
            return Regex.Match(value, pattern).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项的字符串集合
        /// </summary>
        /// <param name="value"> 要搜索匹配项的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <returns> 一个集合，包含有关匹配项的字符串值 </returns>
        public static IEnumerable<string> Matches(this string value, string pattern)
        {
            if (value == null)
            {
                return new string[] { };
            }
            MatchCollection matches = Regex.Matches(value, pattern);
            return from Match match in matches select match.Value;
        }

        /// <summary>
        /// 是否电子邮件
        /// </summary>
        public static bool IsEmail(this string value)
        {
            const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是IP地址
        /// </summary>
        public static bool IsIpAddress(this string value)
        {
            const string pattern = @"^(\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d\.){3}\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是整数
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            const string pattern = @"^\-?[0-9]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是Unicode字符串
        /// </summary>
        public static bool IsUnicode(this string value)
        {
            const string pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否Url字符串
        /// </summary>
        public static bool IsUrl(this string value)
        {
            const string pattern = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否身份证号，验证如下3种情况：
        /// 1.身份证号码为15位数字；
        /// 2.身份证号码为18位数字；
        /// 3.身份证号码为17位数字+1个字母
        /// </summary>
        public static bool IsIdentityCard(this string value)
        {
            const string pattern = @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        public static bool IsMobileNumber(this string value, bool isRestrict = false)
        {
            string pattern = isRestrict ? @"^[1][3-9]\d{9}$" : @"^[1]\d{10}$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        ///是否固定电话
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFixedPhone(this string value)
        {
            string pattern = @"^(\d{3,4}-)?\d{6,8}$";
            return value.IsMatch(pattern);
        }


        #endregion

        #region 其他操作

        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 为指定格式的字符串填充相应对象来生成字符串
        /// </summary>
        /// <param name="format">字符串格式，占位符以{n}表示</param>
        /// <param name="args">用于填充占位符的参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            //format.CheckNotNull("format");
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// 将字符串反转
        /// </summary>
        /// <param name="value">要反转的字符串</param>
        public static string ReverseString(this string value)
        {
            // value.CheckNotNull("value");
            return new string(value.Reverse().ToArray());
        }

        /// <summary>
        /// 判断指定路径是否图片文件
        /// </summary>
        public static bool IsImageFile(this string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }
            byte[] filedata = File.ReadAllBytes(filename);
            if (filedata.Length == 0)
            {
                return false;
            }
            ushort code = BitConverter.ToUInt16(filedata, 0);
            switch (code)
            {
                case 0x4D42: //bmp
                case 0xD8FF: //jpg
                case 0x4947: //gif
                case 0x5089: //png
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 以指定字符串作为分隔符将指定字符串分隔成数组
        /// </summary>
        /// <param name="value">要分割的字符串</param>
        /// <param name="strSplit">字符串类型的分隔符</param>
        /// <param name="removeEmptyEntries">是否移除数据中元素为空字符串的项</param>
        /// <returns>分割后的数据</returns>
        public static string[] Split(this string value, string strSplit, bool removeEmptyEntries = false)
        {
            return value.Split(new[] { strSplit }, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// 获取字符串的MD5 Hash值
        /// </summary>
        //public static string ToMd5Hash(this string value)
        //{
        //    return HashHelper.GetMd5(value);
        //}

        /// <summary>
        /// 支持汉字的字符串长度，汉字长度计为2
        /// </summary>
        /// <param name="value">参数字符串</param>
        /// <returns>当前字符串的长度，汉字长度为2</returns>
        public static int TextLength(this string value)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] bytes = ascii.GetBytes(value);
            foreach (byte b in bytes)
            {
                if (b == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        ///// <summary>
        ///// 将JSON字符串还原为对象
        ///// </summary>
        ///// <typeparam name="T">要转换的目标类型</typeparam>
        ///// <param name="json">JSON字符串 </param>
        ///// <returns></returns>
        //public static T FromJsonString<T>(this string json)
        //{
        //    json.CheckNotNull("json");
        //    return JsonConvert.DeserializeObject<T>(json);
        //}

        /// <summary>
        /// 将字符串转换为<see cref="byte"/>[]数组，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// 将<see cref="byte"/>[]数组转换为字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(bytes);
        }

        #endregion

        #region Null

        /// <summary>
        /// 判断字符串是否有值.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool HasValue(this string instance)
        {
            return !string.IsNullOrEmpty(instance);
        }

        /// <summary>
        /// 判断字符串是否为空.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string instance)
        {
            return !instance.HasValue();
        }

        /// <summary>
        /// 判断字符串是否非空.
        /// </summary>
        /// <returns></returns>
        public static bool IsNotEmpty(this string instance)
        {
            return instance.HasValue();
        }

        /// <summary>
        /// 将字符串输出(如果为空，则输出默认值).
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ToDefault(this string instance, string defaultValue)
        {
            return instance.HasValue() ? instance : defaultValue;
        }

        #endregion

        #region Is

        /// <summary>
        /// 检测字符串是否是电话.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsPhone(this string instance)
        {
            return !instance.IsEmpty() && Regex.IsMatch(instance, @"^(13|14|15|17|18|19)[0-9]{9}$");
        }

        /// <summary>
        /// 检测日期字符串是否正常的日期.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsDate(this string instance)
        {
            if (instance.IsEmpty()) return false;
            return Regex.IsMatch(instance, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]" +
                                       @"|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|" +
                                       @"1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?" +
                                       @"2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468]" +
                                       @"[048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        /// <summary>
        /// 检测字符串是否中文.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsChinese(this string instance)
        {
            if (instance.IsEmpty()) return false;
            return Regex.IsMatch(instance, @"^[\u4e00-\u9fa5]+$");
        }

        #endregion

        /// <summary>
        /// 如果该字符串实例不为空，则用指定的转换器对字符串作转换.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="converter"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ConvertWith(this string instance, Func<string, string> converter = null, string defaultValue = null)
        {
            if (!instance.HasValue() && defaultValue == null) return "";
            if (!instance.HasValue() && defaultValue != null) return defaultValue;
            if (converter != null) return converter(instance);
            return instance;
        }

        ///// <summary>
        ///// 检查值是否在某个离散范围内.
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <param name="ranges"></param>
        ///// <returns></returns>
        //public static bool In(this string instance, params string[] ranges)
        //{
        //    return ranges.Contains(instance);
        //}

        /// <summary>
        /// 检测该字符串是否出现在另一个字符串中.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool AppearIn(this string instance, string content)
        {
            if (instance.IsEmpty()) return false;
            if (content.IsEmpty()) return false;

            return content.Contains(instance);
        }

        /// <summary>
        /// 获取指定的长度的随机字母和数字字符串
        /// </summary>
        /// <param name="length">要获取随机数长度</param>
        /// <returns>指定长度的随机字母和数字组成字符串</returns>
        public static string GetRandomLetterAndNumberString(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            char[] pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                                        'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = pattern.Length;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }
    }
}
