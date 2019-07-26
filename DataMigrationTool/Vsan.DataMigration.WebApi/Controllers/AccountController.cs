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

namespace Vsan.DataMigration.WebApi.Controllers
{
    [VerifyModel]
    public class AccountController : Controller
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login()
        {
            return View();
        }


        #region 验证码

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendVerifyCode(UserAccount param)
        {
            return Json(SendVerifyCodeToEmail(param));
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendVerifyCode()
        {
            var vCode = new Random().Next(1000, 9999).ToString();
            Session["VCode"] = vCode;
            var ms= VerifyerCode.CreateValidateGraphic2(vCode,200,100);
            return File(ms,"image/png");
        }

        #endregion

        #region 登录 or 注册

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(UserAccount param)
        {
            var token = Guid.NewGuid().ToString();
            UserInfo userInfo = null;
            if (!ModelState.IsValid) return Json(Global.VerifyModel(ModelState));

            using (var db = new DataMigrationEntities())
            {
                var user_account = db.user_account.FirstOrDefault(a => a.Email == param.Account);

                if (user_account == null)
                {
                    //注册流程
                    //验证账号
                    var vCode = MeCache<string>.Get(string.Format(MeCacheKey.EmailVerifyCode, param));
                    if (string.IsNullOrWhiteSpace(vCode))
                    {
                        var json = SendVerifyCodeToEmail(param);
                        return SendVerifyCode(param);
                    }
                    if (param.Code != vCode)
                    {
                        return Json(ReturnResult.Fail(null));
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
                        Password = "123456",
                        TrueName = string.Empty,
                    };
                    db.user_account.Add(user_account);
                    db.SaveChanges();
                    userInfo = SetCache(user_account, token);
                    return Json(new ReturnResult(0, token, userInfo));

                }

                if (user_account.Password != param.Code)
                {
                    return Json(new ReturnResult(2, TipString.密码错误));
                }

                userInfo = SetCache(user_account, token);
                return Json(new ReturnResult(0, token, userInfo));
            }
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="user_account"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static ReturnResult SendVerifyCodeToEmail(UserAccount account)
        {
            var vCode = new Random().Next(100000, 999999).ToString();

            MeCache<string>.AddOrUpdate(string.Format(MeCacheKey.EmailVerifyCode, account), vCode, DateTime.Now.AddMinutes(5));

            Vsan.Common.EmailHelper.SendMailb("496988878@qq.com", "邮箱验证码", "gzxixoaaerawbgfj", account.Account, "你的邮箱验证码为:【" + vCode + "】 5分钟内有效.", vCode, "smtp.qq.com", false);

            return new ReturnResult(0, "验证码已发送至你的邮箱，请查收");
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
