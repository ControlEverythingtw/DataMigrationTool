using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vsan.DataMigration.WebApi.Controllers;

namespace Vsan.DataMigration.WebApi.Areas.AppPage.Controllers
{
    public class DataSourceController : BaseController
    {
        // GET: AppPage/DataSource
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Update()
        {
            return View();
        }
    }
}