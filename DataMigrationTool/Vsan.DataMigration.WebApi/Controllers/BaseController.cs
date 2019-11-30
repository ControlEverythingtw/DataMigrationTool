using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Vsan.Common;
using Vsan.DataMigration.Models;

namespace Vsan.DataMigration.WebApi.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        /// 从Cookie中获取用户编号
        /// </summary>
        public int UserId
        {
            get
            {
                if (User.Identity is System.Security.Principal.GenericIdentity)
                {
                    throw new CheckException(ResultCode.AuthExpire, "授权过期请重新登录");
                }

                var strUser = ((FormsIdentity)User.Identity).Ticket.UserData;
                var _loginInfo = new UserAccount();
                if (strUser.Contains("{") && strUser.Contains("}"))
                {
                    var jss = new JavaScriptSerializer();
                    _loginInfo = jss.Deserialize<UserAccount>(strUser);
                    return _loginInfo.UserId;
                }
                else
                {
                    throw new CheckException(ResultCode.AuthExpire,"授权过期请重新登录");
                }

            }
        }

        /// <summary>
        /// 保存用户登陆信息
        /// </summary>
        public void WriteUserInfoToCookie(UserAccount userinfo)
        {
            var jss = new JavaScriptSerializer();
            var logonInfo = jss.Serialize(userinfo);

            //设置Ticket信息
            var ticket = new FormsAuthenticationTicket(1, userinfo.Account, DateTime.Now,
                                                       DateTime.Now.AddDays(1), false,
                                                       logonInfo);
            //加密验证票据
            var strTicket = FormsAuthentication.Encrypt(ticket);
            //保存cookie
            SetCookie(FormsAuthentication.FormsCookieName, strTicket, ticket.Expiration, true);
        }

        /// <summary>
        /// 写入Cooike
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        /// <param name="isSetExpires"></param>
        public static void SetCookie(string cookiename, string value, DateTime expires, bool isSetExpires)
        {
            var request = System.Web.HttpContext.Current.Request;
            var response = System.Web.HttpContext.Current.Response;
            var cookie = request.Cookies[cookiename] ?? new System.Web.HttpCookie(cookiename);
            cookie.Domain = FormsAuthentication.CookieDomain;
            if (value == null)
            {
                RemoveCookie(cookiename);
            }
            else
            {
                cookie.Value = value;

                //true代表客户端只能读，不能写。只有服务端可写，防止被篡改
                cookie.HttpOnly = true;

                if (isSetExpires)
                {
                    cookie.Expires = expires;
                }
            }
            response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 移除指定名称的cookie对象中的集合对
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        public static void RemoveCookie(string cookieName)
        {
            var cookie = System.Web.HttpContext.Current.Request.Cookies[cookieName];
            var response = System.Web.HttpContext.Current.Response;
            if (cookie == null) return;
            cookie.Values.Clear();
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Expires = DateTime.Now.AddDays(-10000d);
            response.Cookies.Add(cookie);
        }

    }
}