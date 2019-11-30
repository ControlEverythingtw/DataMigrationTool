using Vsan.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Vsan.Common;
using Vsan.Common.Safety;
using Vsan.DataMigration.Models;
using JsonResult = System.Web.Mvc.JsonResult;
using System.Web.Security;

namespace Vsan.DataMigration.WebApi.Controllers
{
    [VerifyModel]
    public class AccountController : BaseController
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        #region 注册


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Register(UserAccount param)
        {
            if (!ModelState.IsValid) return Json(new ReturnResult(2, Global.VerifyModel(ModelState)));
            var code = MeCache<string>.Get(string.Format(MeCacheKey.EmailVerifyCode, param.Account));
            if (code != param.VerifyCode_Email)
            {
                return Json(new ReturnResult(2, "邮箱验证码错误"));
            }
            using (var db = new DataMigrationEntities())
            {
                var user_account = db.user_account.FirstOrDefault(a => a.Email == param.Account);
                if (user_account != null)
                {
                    return Json(ReturnResult.IsExist());
                }
                user_account = new user_account
                {
                    CreateTime = DateTime.Now,
                    Creator = "login_api",
                    Modifier = string.Empty,
                    ModifyTime = DateTime.Now,
                    Email = param.Account,
                    Account = param.Account,
                    HeadPortrait = string.Empty,
                    Mobile = string.Empty,
                    Nickname = string.Empty,
                    Password = param.Password,
                    TrueName = string.Empty,
                };
                db.user_account.Add(user_account);
                db.SaveChanges();
                return Json(ReturnResult.Ok);
            }
        }
        #endregion





        #region 验证码

        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyCode(int width = 100, int height = 30)
        {
            var code = new Random().Next(1000, 9999).ToString();
            Session["VerifyCode"] = code;
            var stem = VerifyerCode.Create(code, width, height);
            return File(stem.ToArray(), "image/jpeg");
        }
        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendVerifyCode(UserAccount param,string functionName="")
        {
            if (!ModelState.IsValid) return Json(new ReturnResult(2, Global.VerifyModel(ModelState)));
            using (var db = new DataMigrationEntities())
            {
                var user_account = db.user_account.FirstOrDefault(a => a.Email == param.Account);
                if (user_account != null)
                {
                    return Json(ReturnResult.IsExist());
                }
            }
            return Json(SendVerifyCodeToEmail(param, functionName));
        }

        #endregion

        #region 登录 

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(UserAccount param)
        {
            if (!ModelState.IsValid) return Json(new ReturnResult(2, Global.VerifyModel(ModelState)));

            var code = Session["VerifyCode"]?.ToString();

            if (param.VerifyCode_Image != code)
            {
                return Json(new ReturnResult(2, TipString.验证码错误));
            }


            using (var db = new DataMigrationEntities())
            {
                var user_account = db.user_account.FirstOrDefault(a => a.Email == param.Account);
                if (user_account == null)
                {
                    return Json(ReturnResult.NotExist);
                }
                if (user_account.Password != param.Password)
                {
                    return Json(new ReturnResult(2, TipString.密码错误));
                }
                param.UserId = (int)user_account.Id;
                WriteUserInfoToCookie(param);
                return Json(ReturnResult.Ok);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public void SignOut()
        {
            RemoveCookie(FormsAuthentication.FormsCookieName);
        }
        #endregion


        #region 找回密码

      
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        public ActionResult FindPwd()
        {
            return View();
        }
        /// <summary>
        /// 发送邮箱验证码_找回密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FindPwd_SendVerifyCode(UserAccount param)
        {
            if (!ModelState.IsValid) return Json(new ReturnResult(2, Global.VerifyModel(ModelState)));
            using (var db = new DataMigrationEntities())
            {
                var user_account = db.user_account.FirstOrDefault(a => a.Email == param.Account);
                if (user_account == null)
                {
                    return Json(ReturnResult.NotExist);
                }
            }
            var res = SendVerifyCodeToEmail(param,"找回密码");

            return Json(res);
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FindPwd(UserAccount param)
        {
            if (!ModelState.IsValid) return Json(new ReturnResult(2, Global.VerifyModel(ModelState)));
            var code = MeCache<string>.Get(string.Format(MeCacheKey.EmailVerifyCode, param.Account)+ "找回密码");
            if (code != param.VerifyCode_Email)
            {
                return Json(new ReturnResult(2, "邮箱验证码错误"));
            }
            using (var db = new DataMigrationEntities())
            {
                var user_account = db.user_account.FirstOrDefault(a => a.Email == param.Account);
                if (user_account == null)
                {
                    return Json(ReturnResult.NotExist);
                }
                user_account.Password = param.Password;
                user_account.Modifier = "self";
                user_account.ModifyTime = DateTime.Now;
                db.SaveChanges();
                return Json(ReturnResult.Ok);
            }
        }
        #endregion


        #region 私有方法

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="user_account"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static ReturnResult SendVerifyCodeToEmail(UserAccount account,string functionName="")
        {
            try
            {

            var key = string.Format(MeCacheKey.EmailVerifyCode, account.Account)+ functionName;
            var data = MeCache<string>.Get(key);
            if (string.IsNullOrWhiteSpace(data))
            {
                var vCode = new Random().Next(100000, 999999).ToString();

                var param = new Dictionary<string, string> {
                            {"Recipient",account.Account },
                            {"Content","<p>您"+functionName+"的邮箱验证码为:<h1 style=\"color:red;\">"+vCode+"</h1></p>" },
                            {"CName","【工务园系统】" }
                        };

                using (var http=new HttpClient())
                {
                   var response= http.PostAsJsonAsync("http://111.231.116.56:8080/api/email/send", param).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        var content = "无内容";
                        try
                        {
                            content = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception)
                        {

                        }
                        return new ReturnResult(0, $"邮件发送服务异常. (错误代码:{response.StatusCode},响应内容:{content})");
                    }
                }
                //Vsan.Common.EmailHelper.SendMailb("496988878@qq.com", "邮箱验证码", "gzxixoaaerawbgfj", account.Account, "你的邮箱验证码为:【" + vCode + "】 5分钟内有效.", vCode, "smtp.qq.com", false);
                MeCache<string>.AddOrUpdate(key, vCode, DateTime.Now.AddMinutes(5));
            }
            return new ReturnResult(0, "验证码已发送至你的邮箱，请查收");

            }
            catch (Exception ex)
            {
                return new ReturnResult(2, ex.Message+ex.InnerException?.Message+ex.InnerException?.InnerException?.Message);
            }
        }



        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="user_account"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static UserInfo SetCache(user_account user_account, string token)
        {
            var userInfo = new UserInfo
            {
                Id = user_account.Id,
                Logo = user_account.HeadPortrait,
                Name = user_account.Nickname,
            };
            MeCache<UserInfo>.AddOrUpdate(token, userInfo, DateTime.Now.AddDays(7));
            return userInfo;
        }
        #endregion
    }
}
