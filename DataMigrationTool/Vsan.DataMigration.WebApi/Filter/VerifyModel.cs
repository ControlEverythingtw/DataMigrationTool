using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Vsan.DataMigration.WebApi
{
    /// <summary>
    /// 模型验证过滤器
    /// </summary>
    public class VerifyModelAttribute : ActionFilterAttribute
    {


        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (!actionContext.ModelState.IsValid)
            {
                StringBuilder builder = new StringBuilder();
                var ErrorsModels = actionContext.ModelState.Values.Where(item => { return item.Errors.Count > 0; });

                foreach (var ErrorsModel in ErrorsModels)
                {
                    if (ErrorsModel != null)
                    {
                        builder.AppendLine(ErrorsModel.Errors[0].ErrorMessage);
                    }
                }
               
                string ErrorMsg = builder.ToString();
               
               actionContext.Response = actionContext.Request.CreateResponse(
                 HttpStatusCode.OK,
                 new 
                 {
                     code = 1,
                     message = ErrorMsg,
                     data=false,
                 });

            }
        }
    }
}