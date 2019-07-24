using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vsan.DataMigration.WebApi.Areas.AppPage.Controllers
{
    public class WorkOrderController : Controller
    {
        // GET: AppPage/WorkOrder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            ViewBag.OrderId = Guid.NewGuid().ToString();
            return View();
        }
        public ActionResult Update()
        {
            return View();
        }
    }
}