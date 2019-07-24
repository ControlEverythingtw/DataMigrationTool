using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Vsan.Scheduling.Server.api.Models;
using Vsan.Common;
using Vsan.Common.Cache;

namespace Vsan.Scheduling.Server.api.Filter
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
                Logger.Instance.Info($"Token 是空的 请求的Uri:{JsonConvert.SerializeObject(actionContext.Request.RequestUri)}");
                return false;
            }
            var userinfo = MeCache<UserInfo>.Get(token.Parameter);
            if (userinfo == null)
            {
                Logger.Instance.Info($"Userinfo 是空的 请求的Uri:{JsonConvert.SerializeObject(actionContext.Request.RequestUri)}");
                return false;
            }
            if (userinfo.IsLock == true)
            {
                Logger.Instance.Print($"{token.Parameter} 账号锁定{userinfo.UserId}");

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
