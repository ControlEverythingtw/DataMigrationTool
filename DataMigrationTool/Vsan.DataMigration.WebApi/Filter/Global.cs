using Vsan.Common.Cache;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Vsan.DataMigration.Models;

namespace Vsan.DataMigration.WebApi
{
    /// <summary>
    /// 模型验证过滤器
    /// </summary>
    public class Global
    {


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetUserInfo()
        {
            var value = HttpContext.Current.Request.Headers["Authorization"];
            if (value == null)
            {
                throw new Exception( "授权失效，请您重新登录。");
            }

            var tokens = value.Split(' ');

            if (tokens == null || tokens.Length < 2)
            {
                throw new Exception( "授权失效，请您重新登录。");
            }

            return MeCache<UserInfo>.Get(tokens[1]);

        }
        /// <summary>
        /// 获取用户编号
        /// </summary>
        /// <returns></returns>
        public static long GetUserId()
        {
            var user = GetUserInfo();
            if (user == null)
            {
                throw new Exception("授权失效，请您重新登录。");
            }
            return user.Id;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public static bool LoginOut()
        {
            var value = HttpContext.Current.Request.Headers["Authorization"];
            if (value != null)
            {
                var token = value.Split(' ')[1];
                //var userinfo = MeCache<UserInfoBase>.Get(token);
                //移除Token
                MeCache<UserInfo>.Remove(token);
                return true;
            }
            return false;
        }

        public static string VerifyModel(ModelStateDictionary ModelState)
        {

            StringBuilder builder = new StringBuilder();
            var ErrorsModels = ModelState.Values.Where(item => { return item.Errors.Count > 0; });

            foreach (var ErrorsModel in ErrorsModels)
            {
                if (ErrorsModel != null)
                {
                    builder.AppendLine(ErrorsModel.Errors[0].ErrorMessage);
                }
            }
            string errorMsg = builder.ToString();
            return errorMsg;

        }
    }
}