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
        public ActionResult Search(string Keyword, string Condition, string Source)
        {
            if (string.IsNullOrEmpty(Keyword))
            {
                ViewBag.SearchHtml = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 搜尋必須要有關鍵字！</div>";
                return View();
            }
            if (Session["Username"] == null && (!string.IsNullOrEmpty(Source) && !Source.Equals("AllStore")))
            {
                ViewBag.SearchHtml = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 搜尋權限不足，請先登入！</div>";
                return View();
            }
            ViewBag.DayOfWeek = ((int)DateTime.Now.DayOfWeek).ToString();

            //
            if (string.IsNullOrEmpty(Condition))
            {
                Condition = "Name";
            }
            if (string.IsNullOrEmpty(Source))
            {
                Source = "AllStore";
            }

            // Load all store
            Services.StoreService ss = new Services.StoreService();
            Dictionary<string, List<Models.Store>> storesDic = new Dictionary<string, List<Models.Store>>()
            {
                { "Name", ss.LoadAllStore((Session["Username"] != null) ? Session["Username"].ToString() : "").FindAll(x => x.Name.Contains(Keyword)) },
                { "Branch", ss.LoadAllStore((Session["Username"] != null) ? Session["Username"].ToString() : "").FindAll(x => x.Branch.Contains(Keyword)) },
                { "Type", ss.LoadAllStore((Session["Username"] != null) ? Session["Username"].ToString() : "").FindAll(x => x.Type.Contains(Keyword)) },
                { "Address", ss.LoadAllStore((Session["Username"] != null) ? Session["Username"].ToString() : "").FindAll(x => x.Address.Contains(Keyword)) }
            };
            // Judge
            if (Session["Username"] != null)
            {
                foreach (string key in storesDic.Keys)
                {
                    switch (Source)
                    {
                        case "MyStore":
                            storesDic[key].RemoveAll(x => !x.Creater.Equals(Session["Username"].ToString()));
                            break;
                        case "MyCollect":
                            storesDic[key].RemoveAll(x => !x.IsCollect);
                            break;
                    }
                }
            }

            switch (Condition)
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
            switch (Source)
            {
                case null:
                case "":
                case "AllStore":
                    ViewBag.AllStore = "active";
                    break;
                case "MyStore":
                    ViewBag.MyStore = "active";
                    break;
                case "MyCollect":
                    ViewBag.MyCollect = "active";
                    break;
            }
            ViewBag.Keyword = Keyword;
            ViewBag.Condition = Condition;
            ViewBag.Source = Source;

            ViewBag.NameCount = storesDic["Name"].Count;
            ViewBag.BranchCount = storesDic["Branch"].Count;
            ViewBag.TypeCount = storesDic["Type"].Count;
            ViewBag.AddressCount = storesDic["Address"].Count;

            if (Session["Username"] != null)
            {
                ViewBag.AllStoreCount = storesDic.Sum(x => x.Value.Count);
                ViewBag.MyStoreCount = storesDic.Sum(x => x.Value.Count(y => y.Creater.Equals(Session["Username"].ToString())));
                ViewBag.MyCollectCount = storesDic.Sum(x => x.Value.Count(y => y.IsCollect));
            }
            return View(storesDic[string.IsNullOrEmpty(Condition) ? "Name" : Condition]);
        }
    }
}