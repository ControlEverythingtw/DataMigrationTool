
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vsan.DataMigration.WebApi.Areas.AppPage.Controllers
{
    public class QaController : Controller
    {
        // GET: AppPage/Qa
        public ActionResult Index()
        {
            return View();
        }
        // GET: AppPage/Qa/AddQuerstionGroup
        public ActionResult AddQuerstionGroup()
        {
            return View();
        }
        // GET: AppPage/Qa/EditQuerstions
        public ActionResult EditQuerstions()
        {
            return View();
        }
    }
}