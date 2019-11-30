using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vsan.DataMigration.WebApi.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var userid=UserId;
            return View();
        }
        public ActionResult Main()
        {
            var userid = UserId;
            ViewBag.SaveData = saveData;
            return View();
        }
        public static string saveData;
        
        [HttpPost]
        public string SaveData(string data)
        {
            saveData = data;
            return saveData;
        }
    }
}
