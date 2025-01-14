﻿using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Collections;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace Vsan.Common.Security
{
    /// <summary>
    /// Utils
    /// </summary>
    public class Utils
    {
        private static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        private static string TemplateCookieName = string.Format("Kamsofttemplateid_{0}_{1}_{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);

        #region 获取记录模板id的Cookie名称

        /// <summary>
        /// 获取记录模板id的cookie名称
        /// </summary>
        /// <returns></returns>
        public static string GetTemplateCookieName()
        {
            return TemplateCookieName;
        }

        #endregion

        #region 得到正则编译参数设置 GetRegexCompiledOptions

        /// <summary>
        /// 得到正则编译参数设置 GetRegexCompiledOptions
        /// </summary>
        /// <returns>参数设置</returns>
        public static RegexOptions GetRegexCompiledOptions()
        {
#if NET1
                        return RegexOptions.Compiled;
#else
            return RegexOptions.None;
#endif
        }

        #endregion

        #region 返回字符串真实长度, 1个汉字长度为2 GetStringLength

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2 GetStringLength
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        #endregion

        #region 判断字符串是否在字符串数组中 IsCompriseStr

        /// <summary>
        /// 判断字符串是否在字符串数组中 IsCompriseStr
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringarray"></param>
        /// <param name="strsplit"></param>
        /// <returns></returns>
        public static bool IsCompriseStr(string str, string stringarray, string strsplit)
        {
            if (stringarray == "" || stringarray == null)
            {
                return false;
            }

            str = str.ToLower();
            string[] stringArray = Utils.SplitString(stringarray.ToLower(), strsplit);
            for (int i = 0; i < stringArray.Length; i++)
            {
                //string t1 = str;
                //string t2 = stringArray[i];
                if (str.IndexOf(stringArray[i]) > -1)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 字符串数组操作

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        }


        #endregion

        #region 删除字符串尾部的回车/换行/空格 RTrim

        /// <summary>RTrim
        /// 删除字符串尾部的回车/换行/空格 RTrim
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }

        #endregion

        #region 清除给定字符串中的回车及换行符 ClearBR

        /// <summary>
        /// 清除给定字符串中的回车及换行符 ClearBR
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ClearBR(string str)
        {
            Regex r = null;
            r = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);

            Match m = null;

            for (m = r.Match(str); m.Success; m = m.NextMatch())
            {
                str = str.Replace(m.Groups[0].ToString(), "");
            }


            return str;
        }

        #endregion

        #region 截取字符串

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        #endregion

        #region 获得当前绝对路径 GetMapPath

        /// <summary>
        /// 获得当前绝对路径 GetMapPath
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        #endregion

        #region 返回文件是否存在 FileExists

        /// <summary>
        /// 返回文件是否存在 FileExists
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        #endregion

        #region 以指定的ContentType输出指定文件文件 ResponseFile

        /// <summary>
        /// 以指定的ContentType输出指定文件文件 ResponseFile
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="filename">输出的文件名</param>
        /// <param name="filetype">将文件输出时设置的ContentType</param>
        public static void ResponseFile(string filepath, string filename, string filetype)
        {
            Stream iStream = null;

            // 缓冲区为10k
            byte[] buffer = new Byte[10000];

            // 文件长度
            int length;

            // 需要读的数据长度
            long dataToRead;

            try
            {
                // 打开文件
                iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);


                // 需要读的数据长度
                dataToRead = iStream.Length;

                HttpContext.Current.Response.ContentType = filetype;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Utils.UrlEncode(filename.Trim()).Replace("+", " "));

                while (dataToRead > 0)
                {
                    // 检查客户端是否还处于连接状态
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        length = iStream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        // 如果不再连接则跳出死循环
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    // 关闭文件
                    iStream.Close();
                }
            }
            HttpContext.Current.Response.End();
        }

        #endregion

        #region 判断文件名是否为浏览器可以直接显示的图片文件名 IsImgFilename

        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名 IsImgFilename
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFileName(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                return false;
            }
            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }

        #endregion

        #region int型转换为string型 IntToStr

        /// <summary>
        /// int型转换为string型 IntToStr
        /// </summary>
        /// <returns>转换后的string类型结果</returns>
        public static string IntToStr(int intValue)
        {
            //
            return Convert.ToString(intValue);
        }

        #endregion

        #region 字符串如果操过指定长度则将超出的部分用指定字符串代替 GetSubString

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替 GetSubString
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
            //return GetSubStrings(p_SrcString, p_Length*2, p_TailString);
        }

        #endregion

        #region 字符串如果操过指定长度则将超出的部分用指定字符串代替 GetUnicodeSubString

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替 GetUnicodeSubString
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <param name="p_TailString"></param>
        /// <returns></returns>
        public static string GetUnicodeSubString(string str, int len, string p_TailString)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = str.Substring(0, pos) + p_TailString;
            }
            else
                result = str;

            return result;
        }

        #endregion

        #region 取指定长度的字符串 GetSubString

        /// <summary>
        /// 取指定长度的字符串 GetSubString
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                    {
                        return "";
                    }
                    else
                    {
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                    }
                }
            }


            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }


        #endregion

        #region 自定义的替换字符串函数 ReplaceString

        /// <summary>
        /// 自定义的替换字符串函数 ReplaceString
        /// </summary>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        #endregion

        #region 生成指定数量的html空格符号 GetSpacesString

        /// <summary>
        /// 生成指定数量的html空格符号 GetSpacesString
        /// </summary>
        public static string GetSpacesString(int spacesCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < spacesCount; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }

        #endregion

        #region 检测是否符合email格式 IsValidEmail

        /// <summary>
        /// 检测是否符合email格式 IsValidEmail
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        #endregion

        #region 检测是否是正确的Url IsURL

        /// <summary>
        /// 检测是否是正确的Url IsURL
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        #endregion

        #region 获取邮箱主机名 GetEmailHostName

        /// <summary>
        /// 获取邮箱主机名 GetEmailHostName
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static string GetEmailHostName(string strEmail)
        {
            if (strEmail.IndexOf("@") < 0)
            {
                return "";
            }
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }

        #endregion

        #region 判断是否为base64字符串 IsBase64String

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }

        #endregion

        #region 检测是否有Sql危险字符 IsSafeSqlString

        /// <summary>
        /// 检测是否有Sql危险字符 IsSafeSqlString
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        #endregion

        #region 检测是否有危险的可能用于链接的字符串 IsSafeUserInfoString

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串 IsSafeUserInfoString
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }

        #endregion

        #region 清理字符串 CleanInput

        /// <summary>
        /// 清理字符串 CleanInput
        /// </summary>
        public static string CleanInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }

        #endregion

        #region 返回URL中结尾的文件名 GetFilename

        /// <summary>
        /// 返回URL中结尾的文件名 GetFilename
        /// </summary>		
        public static string GetFilename(string url)
        {
            if (url == null)
            {
                return "";
            }
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }

        #endregion

        #region 根据阿拉伯数字返回月份的名称(可更改为某种语言)

        /// <summary>
        /// 根据阿拉伯数字返回月份的名称(可更改为某种语言)
        /// </summary>	
        public static string[] Monthes
        {
            get
            {
                return new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            }
        }

        #endregion

        #region 替换回车换行符为html换行符 StrFormat

        /// <summary>
        /// 替换回车换行符为html换行符 StrFormat
        /// </summary>
        public static string StrFormat(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                str2 = str;
            }
            return str2;
        }

        #endregion

        #region 时间操作

        /// <summary>
        /// 返回标准日期格式string
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }

            if (datetimestr.Equals(""))
            {
                return replacestr;
            }

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;

        }


        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回相对于当前时间的相对天数
        /// </summary>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }

        /// <summary>
        /// 返回标准时间 
        /// </summary>
        public static string GetStandardDateTime(string fDateTime, string formatStr)
        {
            if (fDateTime == "0000-0-0 0:00:00")
            {
                return fDateTime;
            }
            DateTime s = Convert.ToDateTime(fDateTime);
            return s.ToString(formatStr);
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string GetStandardDateTime(string fDateTime)
        {
            return GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd
        /// </sumary>
        public static string GetStandardDate(string fDate)
        {
            return GetStandardDateTime(fDate, "yyyy-MM-dd");
        }

        #endregion

        #region 判断是否为时间

        /// <summary>
        /// 判断是否为时间
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        #endregion

        #region 获取IP

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetRealIP()
        {
            string ip = WEBRequest.GetIP();

            return ip;
        }

        #endregion

        #region 改正sql语句中的转义字符

        /// <summary>
        /// 改正sql语句中的转义字符
        /// </summary>
        public static string mashSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\'", "'");
                str2 = str;
            }
            return str2;
        }

        #endregion

        #region 替换sql语句中的有问题符号

        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        public static string ChkSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("'", "''");
                str2 = str;
            }
            return str2;
        }

        #endregion

        #region 转换为静态html TransHtml

        /// <summary>
        /// 转换为静态html TransHtml
        /// </summary>
        public void TransHtml(string path, string outpath)
        {
            Page page = new Page();
            StringWriter writer = new StringWriter();
            page.Server.Execute(path, writer);
            FileStream fs;
            if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
            {
                File.Delete(page.Server.MapPath("") + "\\" + outpath);
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            else
            {
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            byte[] bt = Encoding.Default.GetBytes(writer.ToString());
            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }

        #endregion

        #region 繁简体转换

        /// <summary>
        /// 转换为简体中文
        /// </summary>
        public static string ToSChinese(string str)
        {
            return "";
            //return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
        }

        /// <summary>
        /// 转换为繁体中文
        /// </summary>
        public static string ToTChinese(string str)
        {
            return "";
            //return Strings.StrConv(str, VbStrConv.TraditionalChinese, 0);
        }

        #endregion

        #region 过滤字符串数组中每个元素为合适的大小

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strArray"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string[] PadStringArray(string[] strArray, int minLength, int maxLength)
        {
            if (minLength > maxLength)
            {
                int t = maxLength;
                maxLength = minLength;
                minLength = t;
            }

            int iMiniStringCount = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (minLength > -1 && strArray[i].Length < minLength)
                {
                    strArray[i] = null;
                    continue;
                }
                if (strArray[i].Length > maxLength)
                {
                    strArray[i] = strArray[i].Substring(0, maxLength);
                }
                iMiniStringCount++;
            }

            string[] result = new string[iMiniStringCount];
            for (int i = 0, j = 0; i < strArray.Length && j < result.Length; i++)
            {
                if (strArray[i] != null && strArray[i] != string.Empty)
                {
                    result[j] = strArray[i];
                    j++;
                }
            }


            return result;
        }

        #endregion

        #region 分割字符串

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!Utils.StrIsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    string[] tmp = { strContent };
                    return tmp;
                }
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                return new string[0] { };
            }
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">被分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <param name="maxElementLength">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int maxElementLength)
        {
            string[] result = SplitString(strContent, strSplit);

            return ignoreRepeatItem ? DistinctStringArray(result, maxElementLength) : result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strSplit"></param>
        /// <param name="ignoreRepeatItem"></param>
        /// <param name="minElementLength"></param>
        /// <param name="maxElementLength"></param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int minElementLength, int maxElementLength)
        {
            string[] result = SplitString(strContent, strSplit);

            if (ignoreRepeatItem)
            {
                result = DistinctStringArray(result);
            }
            return PadStringArray(result, minElementLength, maxElementLength);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">被分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem)
        {
            return SplitString(strContent, strSplit, ignoreRepeatItem, 0);
        }

        #endregion

        #region 清除字符串数组中的重复项

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <param name="maxElementLength">字符串数组中单个元素的最大长度</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] strArray, int maxElementLength)
        {
            Hashtable h = new Hashtable();

            foreach (string s in strArray)
            {
                string k = s;
                if (maxElementLength > 0 && k.Length > maxElementLength)
                {
                    k = k.Substring(0, maxElementLength);
                }
                h[k.Trim()] = s;
            }

            string[] result = new string[h.Count];

            h.Keys.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] strArray)
        {
            return DistinctStringArray(strArray, 0);
        }

        #endregion

        #region 替换html字符 EncodeHtml

        /// <summary>
        /// 替换html字符 EncodeHtml
        /// </summary>
        public static string EncodeHtml(string strHtml)
        {
            if (strHtml != "")
            {
                strHtml = strHtml.Replace(",", "&def");
                strHtml = strHtml.Replace("'", "&dot");
                strHtml = strHtml.Replace(";", "&dec");
                return strHtml;
            }
            return "";
        }

        #endregion

        #region ClearHtml

        /// <summary>
        /// ClearHtml
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string ClearHtml(string strHtml)
        {
            if (!string.IsNullOrEmpty(strHtml))
            {
                Regex r = null;
                Match m = null;

                r = new Regex(@"<\/?[^>]*>", RegexOptions.IgnoreCase);
                for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
                {
                    strHtml = strHtml.Replace(m.Groups[0].ToString(), "");
                }
            }
            return strHtml;
        }

        #endregion



        #region 进行指定的替换(脏字过滤) StrFilter

        /// <summary>
        /// 进行指定的替换(脏字过滤) StrFilter
        /// </summary>
        public static string StrFilter(string str, string bantext)
        {
            string text1 = "";
            string text2 = "";
            string[] textArray1 = SplitString(bantext, "\r\n");
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                text1 = textArray1[num1].Substring(0, textArray1[num1].IndexOf("="));
                text2 = textArray1[num1].Substring(textArray1[num1].IndexOf("=") + 1);
                str = str.Replace(text1, text2);
            }
            return str;
        }

        #endregion

        #region 获得伪静态页码显示链接

        /// <summary>
        /// 获得伪静态页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetStaticPageNumbers(int curPage, int countPage, string url, string expname, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;

            string t1 = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>";
            string t2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>";

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    s.Append("-");
                    s.Append(i);
                    s.Append(expname);
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }

        /// <summary>
        /// 获得帖子的伪静态页码显示链接
        /// </summary>
        /// <param name="expname"></param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetPostPageNumbers(int countPage, string url, string expname, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;
            int curPage = 1;

            string t1 = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>";
            string t2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>";

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                s.Append("<a href=\"");
                s.Append(url);
                s.Append("-");
                s.Append(i);
                s.Append(expname);
                s.Append("\">");
                s.Append(i);
                s.Append("</a>");
            }
            s.Append(t2);

            return s.ToString();
        }

        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage)
        {
            return GetPageNumbers(curPage, countPage, url, extendPage, "page");
        }

        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <param name="pagetag">页码标记</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag)
        {
            return GetPageNumbers(curPage, countPage, url, extendPage, pagetag, null);
        }

        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <param name="pagetag">页码标记</param>
        /// <param name="anchor">锚点</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag, string anchor)
        {
            if (pagetag == "")
                pagetag = "page";
            int startPage = 1;
            int endPage = 1;

            if (url.IndexOf("?") > 0)
            {
                url = url + "&";
            }
            else
            {
                url = url + "?";
            }

            string t1 = "<a href=\"" + url + "&" + pagetag + "=1";
            string t2 = "<a href=\"" + url + "&" + pagetag + "=" + countPage;
            if (anchor != null)
            {
                t1 += anchor;
                t2 += anchor;
            }
            t1 += "\">&laquo;</a>";
            t2 += "\">&raquo;</a>";

            if (countPage < 1)
                countPage = 1;
            if (extendPage < 3)
                extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    s.Append(pagetag);
                    s.Append("=");
                    s.Append(i);
                    if (anchor != null)
                    {
                        s.Append(anchor);
                    }
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }

        #endregion

        #region 返回 HTML 字符串的编码结果 HtmlEncode

        /// <summary>
        /// 返回 HTML 字符串的编码结果 HtmlEncode
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string str)
        {
            return "";
            //return HttpUtility.HtmlEncode(str);
        }

        #endregion

        #region 返回 HTML 字符串的解码结果 HtmlDecode

        /// <summary>
        /// 返回 HTML 字符串的解码结果 HtmlDecode
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string str)
        {
            return "";
            // return HttpUtility.HtmlDecode(str);
        }

        #endregion

        #region 返回 URL 字符串的编码结果 UrlEncode

        /// <summary>
        /// 返回 URL 字符串的编码结果 UrlEncode
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return "";
            //return HttpUtility.UrlEncode(str);
        }

        #endregion

        #region 返回 URL 字符串的编码结果 UrlDecode

        /// <summary>
        /// 返回 URL 字符串的编码结果 UrlDecode
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return "";
            // return HttpUtility.UrlDecode(str);
        }

        #endregion


        #region 返回指定目录下的非 UTF8 字符集文件 FindNoUTF8File

        /// <summary>
        /// 返回指定目录下的非 UTF8 字符集文件 FindNoUTF8File
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>文件名的字符串数组</returns>
        public static string[] FindNoUTF8File(string Path)
        {
            //System.IO.StreamReader reader = null;
            StringBuilder filelist = new StringBuilder();
            DirectoryInfo Folder = new DirectoryInfo(Path);
            //System.IO.DirectoryInfo[] subFolders = Folder.GetDirectories(); 
            /*
            for (int i=0;i<subFolders.Length;i++) 
            { 
                FindNoUTF8File(subFolders[i].FullName); 
            }
            */
            FileInfo[] subFiles = Folder.GetFiles();
            for (int j = 0; j < subFiles.Length; j++)
            {
                if (subFiles[j].Extension.ToLower().Equals(".htm"))
                {
                    FileStream fs = new FileStream(subFiles[j].FullName, FileMode.Open, FileAccess.Read);
                    bool bUtf8 = IsUTF8(fs);
                    fs.Close();
                    if (!bUtf8)
                    {
                        filelist.Append(subFiles[j].FullName);
                        filelist.Append("\r\n");
                    }
                }
            }
            return Utils.SplitString(filelist.ToString(), "\r\n");

        }

        #endregion

        #region 判断文件流是否为UTF8字符集 IsUTF8

        //0000 0000-0000 007F - 0xxxxxxx  (ascii converts to 1 octet!)
        //0000 0080-0000 07FF - 110xxxxx 10xxxxxx    ( 2 octet format)
        //0000 0800-0000 FFFF - 1110xxxx 10xxxxxx 10xxxxxx (3 octet format)

        /// <summary>
        /// 判断文件流是否为UTF8字符集 IsUTF8
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        private static bool IsUTF8(FileStream sbInputStream)
        {
            int i;
            byte cOctets;  // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;

            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();

                if ((chr & 0x80) != 0) bAllAscii = false;

                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);

                        cOctets--;
                        if (cOctets == 0) return false;
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    cOctets--;
                }
            }

            if (cOctets > 0)
            {
                return false;
            }

            if (bAllAscii)
            {
                return false;
            }

            return true;

        }

        #endregion

        #region 格式化字节数字符串 FormatBytesStr

        /// <summary>
        /// 格式化字节数字符串 FormatBytesStr
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString() + "Bytes";
        }

        #endregion

        #region 将long型数值转换为Int32类型 SafeInt32

        /// <summary>
        /// 将long型数值转换为Int32类型 SafeInt32
        /// </summary>
        /// <param name="objNum"></param>
        /// <returns></returns>
        public static int SafeInt32(object objNum)
        {
            if (objNum == null)
            {
                return 0;
            }
            string strNum = objNum.ToString();
            if (IsNumeric(strNum))
            {

                if (strNum.ToString().Length > 9)
                {
                    if (strNum.StartsWith("-"))
                    {
                        return int.MinValue;
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                }
                return Int32.Parse(strNum);
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region 返回相差的秒数 StrDateDiffSeconds

        /// <summary>
        /// 返回相差的秒数 StrDateDiffSeconds
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Sec"></param>
        /// <returns></returns>
        public static int StrDateDiffSeconds(string Time, int Sec)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddSeconds(Sec);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }

        #endregion

        #region 返回相差的分钟数 StrDateDiffMinutes

        /// <summary>
        /// 返回相差的分钟数 StrDateDiffMinutes
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int StrDateDiffMinutes(string time, int minutes)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }

        #endregion


        #region 返回相差的小时数 StrDateDiffHours

        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int StrDateDiffHours(string time, int hours)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }

        #endregion

        #region 建立文件夹 CreateDir

        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            return Utils.MakeSureDirectoryPathExists(name);
        }

        #endregion

        #region 为脚本替换特殊字符串

        /// <summary>
        /// 为脚本替换特殊字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceStrToScript(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\\"");
            return str;
        }

        #endregion


        #region IP操作

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");

        }

        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {

            string[] userip = Utils.SplitString(ip, @".");
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = Utils.SplitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                if (r == 4)
                {
                    return true;
                }


            }
            return false;

        }

        #endregion

        #region 获得Assembly产品信息

        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            return string.Format("{0}.{1}.{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);
        }

        /// <summary>
        /// 获得Assembly产品名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyProductName()
        {
            return AssemblyFileVersion.ProductName;
        }

        /// <summary>
        /// 获得Assembly产品版权
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCopyright()
        {
            return AssemblyFileVersion.LegalCopyright;
        }

        #endregion

        #region 创建目录

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>创建是否成功</returns>
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);

        #endregion


        #region 得到论坛的真实路径 GetTrueForumPath

        /// <summary>
        /// 得到论坛的真实路径
        /// </summary>
        /// <returns></returns>
        public static string GetTrueForumPath()
        {
            string forumPath = HttpContext.Current.Request.Path;
            if (forumPath.LastIndexOf("/") != forumPath.IndexOf("/"))
            {
                forumPath = forumPath.Substring(forumPath.IndexOf("/"), forumPath.LastIndexOf("/") + 1);
            }
            else
            {
                forumPath = "/";
            }
            return forumPath;

        }

        #endregion

        #region 判断字符串是否是时间格式 IsDateString

        /// <summary>
        /// 判断字符串是否是yy-mm-dd字符串
        /// </summary>
        /// <param name="str">待判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsDateString(string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }

        #endregion

        #region 移除Html标记 RemoveHtml

        /// <summary>
        /// 移除Html标记 RemoveHtml
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        #endregion

        #region 过滤HTML中的不安全标签 RemoveUnsafeHtml

        /// <summary>
        /// 过滤HTML中的不安全标签 RemoveUnsafeHtml
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }
        #endregion

        #region 将用户组Title中的font标签去掉

        /// <summary>
        /// 将用户组Title中的font标签去掉
        /// </summary>
        /// <param name="title">用户组Title</param>
        /// <returns></returns>
        public static string RemoveFontTag(string title)
        {
            Regex RegexFont = new Regex(@"<font color=" + "\".*?\"" + @">([\s\S]+?)</font>", Utils.GetRegexCompiledOptions());
            Match m = RegexFont.Match(title);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return title;
        }

        #endregion

        #region 判断对象是否为Int32类型的数字

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            return true;
            //return Validator.IsNumeric(Expression);
        }

        #endregion

        #region 从HTML中获取文本,保留br,p,img

        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string HTML)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(HTML, "");
        }

        #endregion

        #region 判断是否为Double

        /// <summary>
        /// 判断是否为Double
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object Expression)
        {
            return true;
            // return Validator.IsDouble(Expression);
        }

        #endregion

        #region object型转换为bool型

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            return true;
            //return TypeConverter.StrToBool(expression, defValue);
        }

        #endregion

        #region string型转换为bool型

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            return true;
            //return TypeConverter.StrToBool(expression, defValue);
        }

        #endregion

        #region 将对象转换为Int32类型

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object expression, int defValue)
        {
            return 0;
            // return TypeConverter.ObjectToInt(expression, defValue);
        }

        #endregion

        #region 将字符串转换为Int32类型

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            return 0;
            //return TypeConverter.StrToInt(expression, defValue);
        }

        #endregion

        #region Object型转换为float型

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            return 0;
            //return TypeConverter.StrToFloat(strValue, defValue);
        }

        #endregion

        #region string型转换为float型

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            return 0;
            //return TypeConverter.StrToFloat(strValue, defValue);
        }

        #endregion

        #region 判断给定的字符串数组(strNumber)中的数据是不是都为数值型

        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            return true;
            //return Validator.IsNumericArray(strNumber);
        }

        #endregion

        #region AdDeTime

        /// <summary>
        /// 
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string AdDeTime(int times)
        {
            string newtime = (DateTime.Now).AddMinutes(times).ToString();
            return newtime;

        }

        #endregion

        #region 验证是否为正整数 IsInt

        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {

            return Regex.IsMatch(str, @"^[0-9]*$");
        }

        #endregion

        #region IsRuleTip

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NewHash"></param>
        /// <param name="ruletype"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsRuleTip(Hashtable NewHash, string ruletype, out string key)
        {
            key = "";
            foreach (DictionaryEntry str in NewHash)
            {

                try
                {
                    string[] single = SplitString(str.Value.ToString(), "\r\n");

                    foreach (string strs in single)
                    {
                        if (strs != "")


                            switch (ruletype.Trim().ToLower())
                            {
                                case "email":
                                    if (IsValidDoEmail(strs.ToString()) == false)
                                        throw new Exception();
                                    break;

                                case "ip":
                                    if (IsIPSect(strs.ToString()) == false)
                                        throw new Exception();
                                    break;

                                case "timesect":
                                    string[] splitetime = strs.Split('-');
                                    if (Utils.IsTime(splitetime[1].ToString()) == false || Utils.IsTime(splitetime[0].ToString()) == false)
                                        throw new Exception();
                                    break;

                            }

                    }


                }
                catch
                {
                    key = str.Key.ToString();
                    return false;
                }
            }
            return true;

        }

        #endregion

        #region 删除最后一个字符 RemoveLastChar

        /// <summary>
        /// 删除最后一个字符 RemoveLastChar
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveLastChar(string str)
        {
            if (str == "")
                return "";
            else
                return str.Substring(0, str.Length - 1);
        }

        #endregion

        #region 备份恢复文件

        /// <summary>
        /// 备份文件 BackupFile
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && System.IO.File.Exists(destFileName))
            {
                return false;
            }
            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 备份文件,当目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }


        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称,如果为null,则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "文件不存在！");
                }
                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    }
                    else
                    {
                        System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                    }
                }
                System.IO.File.Delete(targetFileName);
                System.IO.File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }

        #endregion

        #region 将全角数字转换为数字

        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }

        #endregion

        #region 将字符串转换Color ToColor

        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(string color)
        {
            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);

            }
        }

        #endregion

        #region 转换长文件名为短文件名

        /// <summary>
        /// 转换长文件名为短文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="repstring"></param>
        /// <param name="leftnum"></param>
        /// <param name="rightnum"></param>
        /// <param name="charnum"></param>
        /// <returns></returns>
        public static string ConvertSimpleFileName(string fullname, string repstring, int leftnum, int rightnum, int charnum)
        {
            string simplefilename = "", leftstring = "", rightstring = "", filename = "";

            string extname = GetFileExtName(fullname);
            if (extname == "" || extname == null)
            {

                throw new Exception("字符串不含有扩展名信息");
            }
            int filelength = 0, dotindex = 0;

            dotindex = fullname.LastIndexOf('.');
            filename = fullname.Substring(0, dotindex);
            filelength = filename.Length;
            if (dotindex > charnum)
            {
                leftstring = filename.Substring(0, leftnum);
                rightstring = filename.Substring(filelength - rightnum, rightnum);
                if (repstring == "" || repstring == null)
                {
                    simplefilename = leftstring + rightstring + "." + extname;
                }
                else
                {
                    simplefilename = leftstring + repstring + rightstring + "." + extname;
                }
            }
            else
            {

                simplefilename = fullname;
            }
            return simplefilename;

        }

        #endregion

        #region 获取文件扩展名 GetFileExtName

        ///// <summary>
        ///// 获取文件扩展名 GetFileExtName
        ///// </summary>
        ///// <param name="filename"></param>
        ///// <returns></returns>
        //public static string GetFileExtName(string filename)
        //{
        //    string[] array = filename.Trim().Split('.');
        //    Array.Reverse(array);
        //    return array[0].ToString();
        //}

        #endregion

        #region 将数据表转换成JSON类型串

        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">要转换的数据表</param>
        /// <returns></returns>
        public static StringBuilder DataTableToJSON(System.Data.DataTable dt)
        {
            return DataTableToJson(dt, true);
        }

        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">要转换的数据表</param>
        /// <param name="dispose">数据表转换结束后是否dispose掉</param>
        /// <returns></returns>
        public static StringBuilder DataTableToJson(System.Data.DataTable dt, bool dt_dispose)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[\r\n");

            //数据表字段名和类型数组
            string[] dt_field = new string[dt.Columns.Count];
            int i = 0;
            string formatStr = "{{";
            string fieldtype = "";
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                dt_field[i] = dc.Caption.ToLower().Trim();
                formatStr += "'" + dc.Caption.ToLower().Trim() + "':";
                fieldtype = dc.DataType.ToString().Trim().ToLower();
                if (fieldtype.IndexOf("int") > 0 || fieldtype.IndexOf("deci") > 0 ||
                    fieldtype.IndexOf("floa") > 0 || fieldtype.IndexOf("doub") > 0 ||
                    fieldtype.IndexOf("bool") > 0)
                {
                    formatStr += "{" + i + "}";
                }
                else
                {
                    formatStr += "'{" + i + "}'";
                }
                formatStr += ",";
                i++;
            }

            if (formatStr.EndsWith(","))
            {
                formatStr = formatStr.Substring(0, formatStr.Length - 1);//去掉尾部","号
            }
            formatStr += "}},";

            i = 0;
            object[] objectArray = new object[dt_field.Length];
            foreach (System.Data.DataRow dr in dt.Rows)
            {

                foreach (string fieldname in dt_field)
                {   //对 \ , ' 符号进行转换 
                    objectArray[i] = dr[dt_field[i]].ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'");
                    switch (objectArray[i].ToString())
                    {
                        case "True":
                            {
                                objectArray[i] = "true"; break;
                            }
                        case "False":
                            {
                                objectArray[i] = "false"; break;
                            }
                        default: break;
                    }
                    i++;
                }
                i = 0;
                stringBuilder.Append(string.Format(formatStr, objectArray));
            }
            if (stringBuilder.ToString().EndsWith(","))
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);//去掉尾部","号
            }

            if (dt_dispose)
            {
                dt.Dispose();
            }
            return stringBuilder.Append("\r\n];");
        }

        #endregion

        #region 字段串是否为Null或为""(空)

        /// <summary>
        /// 字段串是否为Null或为""(空)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StrIsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == "")
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 是否为数值串列表，各数值间用","间隔 IsNumericList

        /// <summary>
        /// 是否为数值串列表，各数值间用","间隔 IsNumericList
        /// </summary>
        /// <param name="numList"></param>
        /// <returns></returns>
        public static bool IsNumericList(string numList)
        {
            if (numList == "")
                return false;
            foreach (string num in numList.Split(','))
            {
                if (!IsNumeric(num))
                    return false;
            }
            return true;
        }

        #endregion

        #region 检查颜色值是否为3/6位的合法颜色 CheckColorValue

        /// <summary>
        /// 检查颜色值是否为3/6位的合法颜色 CheckColorValue
        /// </summary>
        /// <param name="color">待检查的颜色</param>
        /// <returns></returns>
        public static bool CheckColorValue(string color)
        {
            if (StrIsNullOrEmpty(color))
            {
                return false;
            }

            color = color.Trim().Trim('#');

            if (color.Length != 3 && color.Length != 6)
            {
                return false;
            }
            //不包含0-9  a-f以外的字符
            if (!Regex.IsMatch(color, "[^0-9a-f]", RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 获取Ajax形式的分页链接 GetAjaxPageNumbers

        /// <summary>
        /// 获取ajax形式的分页链接 GetAjaxPageNumbers
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="callback">回调函数</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns></returns>
        public static string GetAjaxPageNumbers(int curPage, int countPage, string callback, int extendPage)
        {
            string pagetag = "page";
            int startPage = 1;
            int endPage = 1;

            string t1 = "<a href=\"###\" onclick=\"" + string.Format(callback, "&" + pagetag + "=1");
            string t2 = "<a href=\"###\" onclick=\"" + string.Format(callback, "&" + pagetag + "=" + countPage);

            t1 += "\">&laquo;</a>";
            t2 += "\">&raquo;</a>";

            if (countPage < 1)
                countPage = 1;
            if (extendPage < 3)
                extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"###\" onclick=\"");
                    s.Append(string.Format(callback, pagetag + "=" + i));
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }

        #endregion

        #region 根据Url获得源文件内容 GetSourceTextByUrl

        /// <summary>
        /// 根据Url获得源文件内容 GetSourceTextByUrl
        /// </summary>
        /// <param name="url">合法的Url地址</param>
        /// <returns></returns>
        public static string GetSourceTextByUrl(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 20000;//20秒超时
            WebResponse response = request.GetResponse();

            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream);
            return sr.ReadToEnd();
        }

        #endregion

        /// <summary>
        /// 转换时间为unix时间戳
        /// </summary>
        /// <param name="date">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }


        /// <summary>
        /// Json特符字符过滤，参见http://www.json.org/
        /// </summary>
        /// <param name="sourceStr">要过滤的源字符串</param>
        /// <returns>返回过滤的字符串</returns>
        public static string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "\\\b");
            sourceStr = sourceStr.Replace("\t", "\\\t");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\f", "\\\f");
            sourceStr = sourceStr.Replace("\r", "\\\r");
            return sourceStr.Replace("\"", "\\\"");
        }


        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergechar">合并符</param>
        /// <returns>合并到的目的字符串</returns>
        public static string MergeString(string source, string target)
        {
            return MergeString(source, target, ",");
        }

        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergechar">合并符</param>
        /// <returns>并到字符串</returns>
        public static string MergeString(string source, string target, string mergechar)
        {
            if (Utils.StrIsNullOrEmpty(target))
                target = source;
            else
                target += mergechar + source;

            return target;
        }


        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="sDetail">帖子内容</param>
        /// <returns>帖子内容</returns>
        public static string ClearUBB(string sDetail)
        {
            return Regex.Replace(sDetail, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 获取站点根目录URL
        /// </summary>
        /// <returns></returns>
        public static string GetRootUrl(string forumPath)
        {
            int port = HttpContext.Current.Request.Url.Port;
            return string.Format("{0}://{1}{2}{3}",
                                 HttpContext.Current.Request.Url.Scheme,
                                 HttpContext.Current.Request.Url.Host.ToString(),
                                 (port == 80 || port == 0) ? "" : ":" + port,
                                 forumPath);
        }


        /// <summary>
        /// 获取指定文件的扩展名
        /// </summary>
        /// <param name="fileName">指定文件名</param>
        /// <returns>扩展名</returns>
        public static string GetFileExtName(string fileName)
        {
            if (Utils.StrIsNullOrEmpty(fileName) || fileName.IndexOf('.') <= 0)
                return "";

            fileName = fileName.ToLower().Trim();

            return fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
        }

        public static string GetHttpWebResponse(string url)
        {
            return GetHttpWebResponse(url, string.Empty);
        }

        /// <summary>
        /// http POST请求url
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="method_name"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string GetHttpWebResponse(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.Timeout = 20000;

            HttpWebResponse response = null;

            try
            {
                StreamWriter swRequestWriter = new StreamWriter(request.GetRequestStream());
                swRequestWriter.Write(postData);

                if (swRequestWriter != null)
                    swRequestWriter.Close();

                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }

        /// <summary>
        /// 根据字符串获取枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">字符串枚举值</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static T GetEnum<T>(string value, T defValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return defValue;
            }
        }


        /// <summary>
        /// 将8位日期型整型数据转换为日期字符串数据
        /// </summary>
        /// <param name="date">整型日期</param>
        /// <param name="chnType">是否以中文年月日输出</param>
        /// <returns></returns>
        public static string FormatDate(int date, bool chnType)
        {
            string dateStr = date.ToString();

            if (date <= 0 || dateStr.Length != 8)
                return dateStr;

            if (chnType)
                return dateStr.Substring(0, 4) + "年" + dateStr.Substring(4, 2) + "月" + dateStr.Substring(6) + "日";
            return dateStr.Substring(0, 4) + "-" + dateStr.Substring(4, 2) + "-" + dateStr.Substring(6);
        }

        public static string FormatDate(int date)
        {
            return FormatDate(date, false);
        }

        #region Private Methods
        private static string[] browerNames = { "MSIE", "Firefox", "Opera", "Netscape", "Safari", "Lynx", "Konqueror", "Mozilla" };
        //private const string[] osNames = { "Win", "Mac", "Linux", "FreeBSD", "SunOS", "OS/2", "AIX", "Bot", "Crawl", "Spider" };

        /// <summary>
        /// 获得浏览器信息
        /// </summary>
        /// <returns></returns>
        public static string GetClientBrower()
        {
            string agent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (!string.IsNullOrEmpty(agent))
            {
                foreach (string name in browerNames)
                {
                    if (agent.Contains(name))
                        return name;
                }
            }
            return "Other";
        }

        /// <summary>
        /// 获得操作系统信息
        /// </summary>
        /// <returns></returns>
        public static string GetClientOS()
        {
            string os = string.Empty;
            string agent = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (agent == null)
                return "Other";

            if (agent.IndexOf("Win") > -1)
                os = "Windows";
            else if (agent.IndexOf("Mac") > -1)
                os = "Mac";
            else if (agent.IndexOf("Linux") > -1)
                os = "Linux";
            else if (agent.IndexOf("FreeBSD") > -1)
                os = "FreeBSD";
            else if (agent.IndexOf("SunOS") > -1)
                os = "SunOS";
            else if (agent.IndexOf("OS/2") > -1)
                os = "OS/2";
            else if (agent.IndexOf("AIX") > -1)
                os = "AIX";
            else if (System.Text.RegularExpressions.Regex.IsMatch(agent, @"(Bot|Crawl|Spider)"))
                os = "Spiders";
            else
                os = "Other";
            return os;
        }
        #endregion


        /// <summary>
        /// hack tip:通过更新web.config文件方式来重启IIS进程池（注：iis中web园数量须大于1,且为非虚拟主机用户才可调用该方法）
        /// </summary>
        public static void RestartIISProcess()
        {
            try
            {
                System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
                xmldoc.Load(Utils.GetMapPath("~/web.config"));
                System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(Utils.GetMapPath("~/web.config"), null);
                writer.Formatting = System.Xml.Formatting.Indented;
                xmldoc.WriteTo(writer);
                writer.Flush();
                writer.Close();
            }
            catch
            {; }
        }

        /// <summary>
        /// 判断当前客户端请求是否为IE
        /// </summary>
        /// <returns></returns>
        public static bool IsIE()
        {
            return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MSIE") >= 0;
        }




        #region utf8转换为gb2312 Utf8ToGb2312

        /// <summary>
        ///  utf8转换为gb2312 Utf8ToGb2312
        /// </summary>
        /// <param name="utfStr"></param>
        /// <returns></returns>
        public string Utf8ToGb2312(string utfStr)
        {
            Encoding srcEncoding = Encoding.UTF8;
            Encoding dstEncoding = Encoding.GetEncoding("gb2312");
            byte[] bytes = srcEncoding.GetBytes(utfStr);
            byte[] buffer2 = Encoding.Convert(srcEncoding, dstEncoding, bytes);
            char[] chars = new char[dstEncoding.GetCharCount(buffer2, 0, buffer2.Length)];
            dstEncoding.GetChars(buffer2, 0, buffer2.Length, chars, 0);
            return new string(chars);
        }

        #endregion

        #region 从左开始截取指定长度字符串 LeftSubString

        #region #######

        ///// <summary>
        ///// LeftSubString
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="length"></param>
        ///// <returns></returns>
        //public static string SubString(object obj, int length)
        //{
        //    return LeftSubString(obj.ToString(), length);
        //}

        ///// <summary>
        ///// LeftSubString
        ///// </summary>
        ///// <param name="stringToSub"></param>
        ///// <param name="length"></param>
        ///// <returns></returns>
        //public static string SubString(string stringToSub, int length)
        //{
        //    Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
        //    char[] stringChar = stringToSub.ToCharArray();
        //    StringBuilder sb = new StringBuilder();
        //    int nLength = 0;
        //    bool isCut = false;
        //    for (int i = 0; i < stringChar.Length; i++)
        //    {
        //        if (regex.IsMatch((stringChar[i]).ToString()))
        //        {
        //            sb.Append(stringChar[i]);
        //            nLength += 2;
        //        }
        //        else
        //        {
        //            sb.Append(stringChar[i]);
        //            nLength = nLength + 1;
        //        }

        //        if (nLength > length)
        //        {
        //            isCut = true;
        //            break;
        //        }
        //    }
        //    if (isCut)
        //        return sb.ToString() + "…";
        //    else
        //        return sb.ToString();
        //}

        #endregion

        /// <summary>
        /// SubString
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string LeftSubString(object obj, int len)
        {
            return LeftSubString(obj.ToString(), len, "...");
        }

        /// <summary>
        /// SubString
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string LeftSubString(object obj, int len, string customerStr)
        {
            return LeftSubString(obj.ToString(), len, customerStr);
        }

        /// <summary>
        /// SubString
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string LeftSubString(string inputString, int len, string customerStr)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += customerStr;

            return tempString;
        }

        /// <summary>
        /// 获取字符串字节长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetLengthOfString(string str)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
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

        #endregion

        #region 获取文本文件内容 GetStringFromFile

        ///// <summary>
        ///// 获取文本文件内容 GetStringFromFile
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //public static string GetStringFromFile(string filePath)
        //{
        //    filePath = HttpContext.Current.Server.MapPath(filePath);
        //    StreamReader streamReader = new StreamReader(filePath);
        //    string result = streamReader.ReadToEnd();
        //    streamReader.Close();
        //    return result;
        //}

        #endregion

        #region 创建内容文件 CreateFileByString

        ///// <summary>
        ///// 创建内容文件 CreateFileByString
        ///// </summary>
        ///// <param name="content"></param>
        ///// <param name="filePath"></param>
        ///// <param name="isDOM"></param>
        ///// <returns></returns>
        //public static bool CreateFileByString(string content, string fileUrl, bool isDOM)
        //{
        //    //文件路径
        //    string filePath = HttpContext.Current.Server.MapPath(fileUrl);

        //    bool isChangeAttrs = false;
        //    FileAttributes attrs1 = FileAttributes.Normal;

        //    if (File.Exists(filePath))
        //    {
        //        //去掉文件只读属性，先把文件的属性读取出来
        //        attrs1 = File.GetAttributes(filePath);

        //        //下面表达式中的 1 是 FileAttributes.ReadOnly 的值
        //        //此表达式是把 ReadOnly 所在的位改成 0,
        //        //FileAttributes attrs2 = (FileAttributes)((int)attrs1 & ~(1));
        //        File.SetAttributes(filePath, FileAttributes.Normal);

        //        isChangeAttrs = true;
        //    }

        //    //如果不存在目录则先创建
        //    string dirPath = Path.GetDirectoryName(filePath);
        //    if (!Directory.Exists(dirPath))
        //        Directory.CreateDirectory(dirPath);

        //    StreamWriter sw = null;

        //    //带签名
        //    if (isDOM)
        //    {
        //        sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
        //    }
        //    else
        //    {
        //        UTF8Encoding utf8 = new UTF8Encoding(false);
        //        sw = new StreamWriter(filePath, false, utf8);
        //    }

        //    sw.WriteLine(content);
        //    sw.Close();

        //    //还原为原来的属性
        //    if (isChangeAttrs)
        //    {
        //        File.SetAttributes(filePath, attrs1);
        //    }

        //    return true;
        //}

        //public static bool CreateFileByString(string content, string fileUrl)
        //{
        //    return CreateFileByString(content, fileUrl, true);
        //}

        //#region 创建内容文件 CreateFileByString

        /////// <summary>
        /////// 创建内容文件 CreateFileByString
        /////// </summary>
        /////// <param name="content"></param>
        /////// <param name="filePath"></param>
        /////// <returns></returns>
        ////public static bool CreateFileByString(string content, string fileUrl)
        ////{
        ////    //文件路径
        ////    string filePath = HttpContext.Current.Server.MapPath(fileUrl);

        ////    bool isChangeAttrs = false;
        ////    FileAttributes attrs1 = FileAttributes.Normal;

        ////    if (File.Exists(filePath))
        ////    {
        ////        //去掉文件只读属性，先把文件的属性读取出来
        ////        attrs1 = File.GetAttributes(filePath);

        ////        //下面表达式中的 1 是 FileAttributes.ReadOnly 的值
        ////        //此表达式是把 ReadOnly 所在的位改成 0,
        ////        //FileAttributes attrs2 = (FileAttributes)((int)attrs1 & ~(1));
        ////        File.SetAttributes(filePath, FileAttributes.Normal);

        ////        isChangeAttrs = true;
        ////    }

        ////    //如果不存在目录则先创建
        ////    string dirPath = Path.GetDirectoryName(filePath);
        ////    if (!Directory.Exists(dirPath))
        ////        Directory.CreateDirectory(dirPath);

        ////    //StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);

        ////    UTF8Encoding utf8 = new UTF8Encoding(false);
        ////    StreamWriter sw = new StreamWriter(filePath, false, utf8);

        ////    sw.WriteLine(content);
        ////    sw.Close();

        ////    //还原为原来的属性
        ////    if (isChangeAttrs)
        ////    {
        ////        File.SetAttributes(filePath, attrs1);
        ////    }

        ////    return true;
        ////}

        //#endregion

        #endregion


        #region 获取IP所在地 GetIPLocation

        //public string GetIPLocation(string ip)
        //{
        //    WebClient webClient = new WebClient();
        //    Stream stream = webClient.OpenRead(string.Format("http://www.ip138.com/ips.asp?ip={0}&action=2", ip));
        //    StreamReader streamReader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));
        //    string html = streamReader.ReadToEnd();
        //    string s = "<LI>本站主数据：广东省东莞市 电信</LI>";

        //    Regex regex = new Regex("本站主数据：.*\n", RegexOptions.Compiled);
        //    regex.IsMatch(
        //    streamReader.Close();
        //    return html;
        //}

        public static string GetIPLocation(string ip)
        {
            string postString = string.Format("ip={0}&action=2", System.Web.HttpUtility.UrlEncode(ip));

            string url = "http://ip138.com/ips.asp";

            HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;

            byte[] postData = new ASCIIEncoding().GetBytes(postString);

            request.Method = "POST";
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1; CIBA)";
            request.Timeout = 1800000;

            request.GetRequestStream().Write(postData, 0, postData.Length);

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            System.IO.Stream responseStream = response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, System.Text.Encoding.GetEncoding("gb2312"));
            string _html = reader.ReadToEnd();

            responseStream.Close();
            reader.Close();
            response.Close();

            string ipdesc = "";
            if (_html != "")
            {
                MatchCollection mc_1 = Regex.Matches(_html, "<ul class=\"ul1[^>]*>([\\s\\S]*?)<\\/ul>");
                foreach (Match m1 in mc_1)
                {
                    MatchCollection mc_2 = Regex.Matches(m1.Value, "<li>([\\s\\S]*?)<\\/li>");
                    if (mc_2.Count > 0)
                        ipdesc = mc_2[0].Value.Replace("<li>", "").Replace("</li>", "");
                }
            }

            string ipLocation = ipdesc.Replace("本站主数据：", "");
            return ipLocation;
        }

        #endregion

        #region 杀掉不怀好意的字符 InputText

        /// <summary>
        /// Method to make sure that user's inputs are not malicious
        /// </summary>
        /// <param name="text">User's Input</param>
        /// <param name="maxLength">Maximum length of input</param>
        /// <returns>The cleaned up version of the input</returns>
        public static string InputText(string text, int maxLength)
        {
            //text = text.Trim();
            //if (string.IsNullOrEmpty(text))
            //    return string.Empty;
            //if (text.Length > maxLength)
            //    text = text.Substring(0, maxLength);
            //text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            //text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            //text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            //text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            //text = text.Replace("'", "''");
            //return text;

            text = text.ToLower().Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);

            text = Regex.Replace(text, "[\\s]{2,{", " ");
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = Regex.Replace(text, "=", "");
            text = Regex.Replace(text, "%", "");
            text = Regex.Replace(text, "'", "");
            text = Regex.Replace(text, "select", "");
            text = Regex.Replace(text, "insert", "");
            text = Regex.Replace(text, "delete", "");
            text = Regex.Replace(text, "or", "");
            text = Regex.Replace(text, "exec", "");
            text = Regex.Replace(text, "--", "");
            text = Regex.Replace(text, "and", "");
            text = Regex.Replace(text, "where", "");
            text = Regex.Replace(text, "update", "");
            text = Regex.Replace(text, "script", "");
            text = Regex.Replace(text, "iframe", "");
            text = Regex.Replace(text, "master", "");
            text = Regex.Replace(text, "exec", "");
            text = Regex.Replace(text, "<", "");
            text = Regex.Replace(text, ">", "");
            text = Regex.Replace(text, "\r\n", "");

            return text;
        }


        /// <summary>
        /// Method to make sure that user's inputs are not malicious
        /// </summary>
        /// <param name="text">User's Input</param>
        /// <returns>The cleaned up version of the input</returns>
        public static string InputText(string text)
        {
            text = text.ToLower().Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, "[\\s]{2,{", " ");
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = Regex.Replace(text, "=", "");
            text = Regex.Replace(text, "%", "");
            text = Regex.Replace(text, "'", "");
            text = Regex.Replace(text, "select", "");
            text = Regex.Replace(text, "insert", "");
            text = Regex.Replace(text, "delete", "");
            text = Regex.Replace(text, "or", "");
            text = Regex.Replace(text, "exec", "");
            text = Regex.Replace(text, "--", "");
            text = Regex.Replace(text, "and", "");
            text = Regex.Replace(text, "where", "");
            text = Regex.Replace(text, "update", "");
            text = Regex.Replace(text, "script", "");
            text = Regex.Replace(text, "iframe", "");
            text = Regex.Replace(text, "master", "");
            text = Regex.Replace(text, "exec", "");
            text = Regex.Replace(text, "<", "");
            text = Regex.Replace(text, ">", "");
            text = Regex.Replace(text, "\r\n", "");

            return text;
        }

        #endregion

        #region 清除Word格式 CleanNonWord

        /// <summary>
        /// Method to check whether input has other characters than numbers
        /// </summary>
        public static string CleanNonWord(string text)
        {
            return Regex.Replace(text, "\\W", "");
        }

        #endregion

        #region 替换换行和空格成HTML

        /// <summary>
        /// ReplaceLineSpace
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceLineSpace(string text)
        {
            text = text.Replace("\n", "<br>");
            text = text.Replace(" ", "&nbsp;");
            return text;
        }

        #endregion

        #region 生成商品货号 BuildSKU

        /// <summary>
        /// 生成商品货号 BuildSKU
        /// </summary>
        /// <returns></returns>
        public static string BuildSKU()
        {
            Random random = new Random();
            return string.Format("{0}{1}{2}{3}",
                new object[] { DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, random.Next(100) });
        }

        #endregion

        #region 生成订单号 BuildOrderNo

        /// <summary>
        /// 生成訂單號
        /// </summary>
        /// <returns></returns>
        public static string BuildOrderNo()
        {
            Random r = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
            string r2 = r.Next().ToString().Substring(0, 6);
            DateTime now = DateTime.Now;
            string year = now.Year.ToString().Substring(2, 2);
            return string.Format(
                "{0}{1}{2}{3}{4}",
                year,
                r2.Substring(0, 3),
                now.Month.ToString("D2"),
                r2.Substring(3, 3),
                now.Day.ToString("D2")
            );
        }

        #endregion

        #region 根据邮箱地址获取邮箱登录URL GetEmailDomainUrl
        /// <summary>
        /// 根据邮箱地址获取邮箱登录URL GetEmailDomainUrl
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetEmailDomainUrl(string email)
        {
            Match m = Regex.Match(email, @"@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            return string.Concat("mail.", m.Value.Substring(1, m.Value.Length - 1));
        }
        #endregion

        #region 产生不重复随机Int数组 GetRandomUnrepeatIntArray

        /// <summary>
        /// 产生不重复随机Int数组 GetRandomUnrepeatIntArray
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="count">共产生多少随机数</param>
        /// <returns>int[]数组</returns>
        public static int[] GetRandomUnrepeatIntArray(int minValue, int maxValue, int count)
        {
            Random rnd = new Random();
            int length = maxValue - minValue + 1;
            byte[] keys = new byte[length];
            rnd.NextBytes(keys);
            int[] items = new int[length];
            for (int i = 0; i < length; i++)
            {
                items[i] = i + minValue;
            }
            Array.Sort(keys, items);
            int[] result = new int[count];
            Array.Copy(items, result, count);
            return result;
        }


        #endregion

        #region 判断后缀名是否是图片后缀名 IsImageExt

        /// <summary>
        /// 判断后缀名是否是图片后缀名 IsImageExt
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static bool IsImageExt(string ext)
        {
            ext = ext.ToLower();
            return (ext == ".jpg" || ext == ".gif" || ext == ".png" || ext == ".bmp" || ext == ".jpeg");
        }

        #endregion   


        #region 获取页面HTML GetHtmlFromUrl

        //public string GetHtml(string url)
        //{
        //    string strValue = "";
        //    WebRequest objRequest = System.Net.HttpWebRequest.Create(url + "? " + strValue);
        //    StreamReader sr = new StreamReader(objRequest.GetResponse().GetResponseStream());
        //    string result = sr.ReadToEnd();
        //    sr.Close();
        //    return result;
        //} 

        /// <summary>
        /// 获取页面HTML GetHtmlFromUrl
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlFromUrl(string url)
        {
            string charset = HttpContext.Current.Response.Charset;
            StreamReader reader = null;

            Stream responseStream = WebRequest.Create(url).GetResponse().GetResponseStream();
            if (charset == "utf-8")
            {
                reader = new StreamReader(responseStream, Encoding.GetEncoding(charset), false);
            }
            else
            {
                reader = new StreamReader(responseStream, Encoding.GetEncoding(charset));
            }
            string html = reader.ReadToEnd();
            responseStream.Flush();
            responseStream.Close();
            reader.Close();
            return html;
        }

        #endregion

        #region 获取页面HTML GetHtmlByPage

        /// <summary>
        /// 获取页面HTML GetHtmlByPage
        /// 页面必须没有Form
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetHtmlByPage(Page page)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(stringWriter);
            page.RenderControl(htmlWriter);
            return stringWriter.GetStringBuilder().ToString();

        }

        #endregion

        #region 将页面输出内容保存为HTML文件 CreateHtmlFileByPage

        /// <summary>
        /// 将页面输出内容保存为HTML文件 CreateHtmlFileByPage
        /// </summary>
        /// <param name="page"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool CreateHtmlFileByPage(Page page, string path)
        {
            string html = GetHtmlByPage(page);

            return true;
            //return TextFileHelper.CreateFile(html, path, true);
        }

        #endregion


        #region String

        public static string EmptyString = string.Empty;
        public static string StringParse(string old)
        { return StringParse(old, string.Empty); }
        public static string StringParse(object old)
        { return StringParse(old, string.Empty); }
        public static string StringParse(object old, string ReplaceString)
        {
            if (old == null || old.ToString().Trim().Length == 0)
            {
                if (ReplaceString == null || ReplaceString.Trim().Length == 0) { return string.Empty; }
                else { return ReplaceString.Trim(); }
            }
            else { return old.ToString().Trim(); }
        }
        public static string StringParse(string old, string ReplaceString)
        {
            if (old == null || old.Trim().Length == 0)
            {
                if (ReplaceString == null || ReplaceString.Trim().Length == 0) { return string.Empty; }
                else { return ReplaceString.Trim(); }
            }
            else { return old.Trim(); }
        }
        #endregion

        #region Short
        public static short ShortParse(string old)
        { return ShortParse(old, 0); }
        public static short ShortParse(object old)
        { return ShortParse(old, 0); }
        public static short ShortParse(string old, short NullValue)
        {
            short i = 0;
            try
            {
                if (old != null && old.ToString().IndexOf('.') > 0)
                {
                    string str = old.ToString().Remove(old.ToString().IndexOf('.'));
                    i = short.Parse(str.Trim());
                }
                else { i = short.Parse(old.ToString().Trim()); }

            }
            catch { try { i = NullValue; } catch { i = (short)0; } }
            return i;
        }
        public static short ShortParse(object old, short NullValue)
        {
            short i = 0;
            try
            {
                if (old != null && old.ToString().IndexOf('.') > 0)
                {
                    string str = old.ToString().Remove(old.ToString().IndexOf('.'));
                    i = short.Parse(str.Trim());
                }
                else { i = short.Parse(old.ToString().Trim()); }
            }
            catch { try { i = NullValue; } catch { i = (short)0; } }
            return i;
        }
        public static short ShortTryParse(object srcObj)
        {
            short defaultValue;
            if (srcObj == null) { return 0; }
            Int16.TryParse(srcObj.ToString(), out defaultValue);
            return defaultValue;
        }
        public static short ShortTryParse(object srcObj, short NullValue)
        {
            short defaultValue;
            ////if (srcObj == null) { return 0; }
            Int16.TryParse(srcObj.ToString(), out defaultValue);
            if (!Int16.TryParse(srcObj.ToString(), out defaultValue)) { Int16.TryParse(NullValue.ToString(), out defaultValue); }
            return defaultValue;
        }
        #endregion

        #region 将字符串数组以某符号分隔串起来 JoinString

        public static string JoinString(string[] strArray, string split)
        {
            string result = "";
            foreach (string str in strArray)
            {
                result += split + str;
            }
            if (result.Length > 0)
                result = result.Substring(1);
            return result;
        }

        #endregion

    }
}
