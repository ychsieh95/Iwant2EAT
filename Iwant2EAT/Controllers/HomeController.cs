using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Iwant2EAT.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new Service.StoreService().LoadAllStore());
        }

        [HttpPost]
        public ActionResult Search(string storeKeyword)
        {
            return View("Index", new Service.StoreService().LoadAllStore().FindAll(x=>x.Name.Contains(storeKeyword)));
        }
    }
}