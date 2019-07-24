using Vsan.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Vsan.DataMigration.WebApi
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ExceptionHandleAttribute : ExceptionFilterAttribute
    {
       
        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            
            var code = ResultCode.Exception;
            var message = actionExecutedContext.Exception.Message;
            //#if DEBUG
            //            message = $" {actionExecutedContext.Exception.Message}【调试模式下显示】{actionExecutedContext.Exception.StackTrace}";
            //#endif

            switch (actionExecutedContext.Exception)
            {
                //如果是参数异常
                case ArgumentException _:
                    code = ResultCode.ArgumentException;
                    break;
                //"对一个或多个实体的验证失败。有关详细信息，请参见“EntityValidationErrors”属性"。
                case DbEntityValidationException ex1:
                    {

                        StringBuilder stringBuilder=new StringBuilder();

                        foreach (var errors in ex1.EntityValidationErrors)
                        {
                            if (errors!=null)
                            {
                                foreach (var error in errors.ValidationErrors)
                                {
                                    stringBuilder.AppendLine(
                                        $"\tPropertyName={error.PropertyName};ErrorMessage={error.ErrorMessage}");
                                }
                            }
                        }
                        code = ResultCode.ArgumentException;
                        message = $"请输入正确的参数。\n\t实体模型验证失败:\n{stringBuilder}";

                       Vsan.Common.Logger.Instance.Info(message);

                        break;
                    }
                //如果是自定义的检查异常
                case CheckException ex:
                    {
                        code = ex.ResultCode;
                        message = ex.Message;
                        actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(
                            HttpStatusCode.OK,
                            new
                            {
                                code = code,
                                message = message,
                                data = ex.data
                            });
                        var source = string.Join("\n", actionExecutedContext.Exception.StackTrace.Split('\n').Take(3));

                        Logger.Instance.Debug($"Source={source}\nMessage={message}");
                        return;
                    }
            }
           
        }
    }
}