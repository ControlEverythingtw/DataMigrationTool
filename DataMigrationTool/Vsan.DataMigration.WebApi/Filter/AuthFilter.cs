using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Vsan.Common;
using Vsan.Common.Cache;
using Newtonsoft.Json;
using Vsan.DataMigration.Models;

namespace Vsan.DataMigration.WebApi
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class AuthFilterAttribute : AuthorizeAttribute
    {

        /// <summary>
        /// 过滤未登录的或过期请求
        /// </summary>
        /// <param name="actionContext"></param>

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {

            // base.OnAuthorization(actionContext);
            var token = actionContext.Request.Headers.Authorization;
            if (token == null || string.IsNullOrWhiteSpace(token.Parameter))
            {
                Vsan.Common.Logger.Instance.Info($"授权过滤器36  Token 是空的 请求的Uri:{JsonConvert.SerializeObject(actionContext.Request.RequestUri)}");
                //actionContext.Response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.OK,
                new 
                {
                    code = 401,
                    message = "请求头的 Authorization 参数为空"
                });
                return false;
            }
            var userinfo = MeCache<UserInfo>.Get(token.Parameter);
            if (userinfo == null)
            {
                Logger.Instance.Info($"授权过滤器50行 Userinfo 是空的 请求的Uri:{JsonConvert.SerializeObject(actionContext.Request.RequestUri)}");
                actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.OK,
                new 
                {
                    code =401,
                    message = "授权过期"
                });
                return false;
            }
            if (userinfo.IsLock==true)
            {
                Logger.Instance.Print($"{token.Parameter} 账号锁定{userinfo.Id}");
                actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.Forbidden,
                new 
                {
                    code = 403,
                    message = "您的账号已被封号,具体咨询平台客服"
                });
                return true;
            }
            return true;
        }

       
      

    }
}