using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vsan.Common;

namespace Vsan.DataMigration.WebApi.Filter
{
    public class MvcExceptionHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Logger.Instance.Error(filterContext.Exception);
            Logger.Instance.Error(filterContext.Exception.InnerException);

            if (filterContext.Exception is CheckException checkException)
            {
                if (checkException.ResultCode==ResultCode.AuthExpire)
                {
                    filterContext.HttpContext.Response.Redirect("/Account/Login");
                }
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}