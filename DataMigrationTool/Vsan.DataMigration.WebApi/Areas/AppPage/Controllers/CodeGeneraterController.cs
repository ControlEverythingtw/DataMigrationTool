using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vsan.DataMigration.WebApi.Controllers;

namespace Vsan.DataMigration.WebApi.Areas.AppPage.Controllers
{
    public class CodeGeneraterController : BaseController
    {
        // GET: AppPage/CodeBuilder
        public ActionResult Index()
        {

            return View();
        }
    }
}