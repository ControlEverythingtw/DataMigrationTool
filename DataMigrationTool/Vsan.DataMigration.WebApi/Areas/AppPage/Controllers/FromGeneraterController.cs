using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vsan.DataMigration.WebApi.Areas.AppPage.Controllers
{

    /// <summary>
    /// 表单生成器
    /// </summary>
    public class FromGeneraterController : Controller
    {
        // GET: AppPage/FromGenerater
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
    }
}