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

        [HttpGet]
        public ActionResult Search(string Keyword, string Findway)
        {
            if (string.IsNullOrEmpty(Keyword))
            {
                ViewBag.SearchHtml = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 搜尋必須要有關鍵字！</div>";
                return View();
            }

            ViewBag.DayOfWeek = ((int)DateTime.Now.DayOfWeek).ToString();
            Services.StoreService ss = new Services.StoreService();
            Dictionary<string, List<Models.Store>> storesDic = new Dictionary<string, List<Models.Store>>()
            {
                { "Name", ss.LoadAllStore().FindAll(x => x.Name.Contains(Keyword)) },
                { "Branch", ss.LoadAllStore().FindAll(x => x.Branch.Contains(Keyword)) },
                { "Type", ss.LoadAllStore().FindAll(x => x.Type.Contains(Keyword)) },
                { "Address", ss.LoadAllStore().FindAll(x => x.Address.Contains(Keyword)) }
            };
            switch (Findway)
            {
                case "Name":
                    ViewBag.Name = "active"; break;
                case "Branch":
                    ViewBag.Branch = "active"; break;
                case "Type":
                    ViewBag.Type = "active"; break;
                case "Address":
                    ViewBag.Address = "active"; break;
                default:
                    ViewBag.Name = "active"; break;
            }
            ViewBag.Keyword = Keyword;
            ViewBag.NameCount = storesDic["Name"].Count;
            ViewBag.BranchCount = storesDic["Branch"].Count;
            ViewBag.TypeCount = storesDic["Type"].Count;
            ViewBag.AddressCount = storesDic["Address"].Count;
            return View(storesDic[string.IsNullOrEmpty(Findway) ? "Name" : Findway]);
        }
    }
}