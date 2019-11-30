using System.Web;
using System.Web.Mvc;
using Vsan.DataMigration.WebApi.Filter;

namespace Vsan.DataMigration.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MvcExceptionHandler());
        }
    }
}
