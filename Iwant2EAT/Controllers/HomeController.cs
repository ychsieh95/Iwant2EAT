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
            ViewBag.DayOfWeek = ((int)DateTime.Now.DayOfWeek).ToString();
            return View(new Services.StoreService().LoadAllStore((Session["Username"] != null) ? Session["Username"].ToString() : ""));
        }

        [HttpPost]
        public ActionResult Search(string storeKeyword)
        {
            ViewBag.DayOfWeek = ((int)DateTime.Now.DayOfWeek).ToString();
            return View("Index", new Services.StoreService().LoadAllStore((Session["Username"] != null) ? Session["Username"].ToString() : "")
                                                            .FindAll(x=>x.Name.Contains(storeKeyword)));
        }
    }
}