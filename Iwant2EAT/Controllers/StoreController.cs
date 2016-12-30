using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Iwant2EAT.Controllers
{
    public class StoreController : Controller
    {

        [HttpGet]
        public ActionResult Create()
        {
            if (Session.Count <= 0)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                ViewBag.AddStoreActive = "active";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(Models.Store store)
        {
            ViewBag.AddStoreActive = "active";
            if (string.IsNullOrEmpty(store.Name))
            {
                ViewBag.AddStoreHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 店家名稱禁止空白！</div>";
                return View();
            }

            //
            store.DayOff = (store.Sunday ? "" : "0;") + (store.Monday ? "" : "1;") + (store.Tuesday ? "" : "2;") + (store.Wednesday ? "" : "3;") + (store.Thursday ? "" : "4;") + (store.Friday ? "" : "5;") + (store.Saturday ? "" : "6;");
            //
            store.Creater = Session["Username"].ToString();
            // Only id of store
            store.Guid = Guid.NewGuid().ToString();

            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var file = Request.Files[0];
                string dir = string.Format("~/Content/Image/");

                // Check dir is exits or not. if not, create it
                var trueDir = System.Web.Hosting.HostingEnvironment.MapPath(dir);
                if (!System.IO.Directory.Exists(trueDir))
                {
                    System.IO.Directory.CreateDirectory(trueDir);
                }

                // Save file
                string fileName = store.Guid + file.FileName.Remove(0, file.FileName.LastIndexOf('.'));
                file.SaveAs(System.IO.Path.Combine(trueDir, fileName));

                store.ImageUrl = this.Url.Content(System.IO.Path.Combine(dir, fileName));
            }

            Service.StoreService ss = new Service.StoreService();
            if (ss.LoadAllStore().Any(x => x.Name.Equals(store.Name) && x.Branch.Equals(store.Branch)))
            {
                ViewBag.CreateStoreHtml = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 已有重複店家名稱！</div>";
                return View();
            }
            else
            {
                if (ss.AddStore(store))
                {
                    // 導向到詳細頁面
                    TempData.Add("CreateStoreHtml", "<div class=\"alert alert-success\" role=\"alert\">[Success] 店家資訊建立成功！</div>");
                    return RedirectToAction("Detail", new { Guid = store.Guid });
                }
                else
                {
                    ViewBag.CreateStoreHtml = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 店家資訊建立失敗！</div>";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Modify(string Guid)
        {
            return View(new Service.StoreService().LoadAllStore().Find(x => x.Guid.Equals(Guid)));
        }

        [HttpPost]
        public ActionResult Modify(Models.Store store)
        {
            //
            store.DayOff = (store.Sunday ? "" : "0;") + (store.Monday ? "" : "1;") + (store.Tuesday ? "" : "2;") + (store.Wednesday ? "" : "3;") + (store.Thursday ? "" : "4;") + (store.Friday ? "" : "5;") + (store.Saturday ? "" : "6;");

            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var file = Request.Files[0];
                string dir = string.Format("~/Content/Image/");

                // Check dir is exits or not. if not, create it
                var trueDir = System.Web.Hosting.HostingEnvironment.MapPath(dir);
                if (!System.IO.Directory.Exists(trueDir))
                {
                    System.IO.Directory.CreateDirectory(trueDir);
                }

                // Save file
                string fileName = store.Guid + file.FileName.Remove(0, file.FileName.LastIndexOf('.'));
                file.SaveAs(System.IO.Path.Combine(trueDir, fileName));

                store.ImageUrl = this.Url.Content(System.IO.Path.Combine(dir, fileName));
            }

            Service.StoreService ss = new Service.StoreService();
            if (!ss.LoadAllStore().Find(x => x.Guid.Equals(store.Guid)).Creater.Equals(Session["Username"].ToString()))
            {
                TempData.Add("UpdateStoreHtml", "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 權限錯誤！</div>");
                return RedirectToAction("Detail", new { Guid = store.Guid });
            }
            else
            {
                if (ss.UpdateStore(store))
                {
                    TempData.Add("UpdateStoreHtml", "<div class=\"alert alert-success\" role=\"alert\">[Success] 更新店家資訊成功！</div>");
                    return RedirectToAction("Detail", new { Guid = store.Guid });
                }
                else
                {
                    TempData.Add("UpdateStoreHtml", "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 更新店家資訊時發生錯誤！</div>");
                    return RedirectToAction("Detail", new { Guid = store.Guid });
                }
            }
        }

        [HttpGet]
        public ActionResult Delete(string Guid)
        {
            Service.StoreService ss = new Service.StoreService();
            if (!ss.LoadAllStore().Find(x => x.Guid.Equals(Guid)).Creater.Equals(Session["Username"].ToString()))
            {
                TempData.Add("DeleteStoreHtml", "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 權限錯誤！</div>");
                return RedirectToAction("Detail", new { Guid = Guid });
            }
            else
            {
                string filePath = Request.MapPath(ss.LoadAllStore().Find(x => x.Guid.Equals(Guid)).ImageUrl);
                if (ss.DeleteStore(Guid))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    TempData.Add("DeleteStoreHtml", "<div class=\"alert alert-success\" role=\"alert\">[success] 刪除店家資訊成功！</div>");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData.Add("DeleteStoreHtml", "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 刪除店家資訊時發生錯誤！</div>");
                    return RedirectToAction("Detail", new { Guid = Guid });
                }
            }
        }

        [HttpGet]
        public ActionResult Detail(string Guid)
        {
            if (string.IsNullOrEmpty(Guid))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Service.StoreService ss = new Service.StoreService();
                Models.Store store = ss.LoadAllStore().Find(x => x.Guid.Equals(Guid));

                ViewBag.IsCreater = false;
                if (Session.Count > 0 && Session["Username"] != null)
                {
                    if (Session["Username"].ToString().Equals(store.Creater))
                    {
                        ViewBag.IsCreater = true;
                    }
                }
                ViewBag.StoreName = store.Name + (string.IsNullOrEmpty(store.Branch) ? "" : string.Format(" ({0})", store.Branch));
                return View(store);
            }
        }

        [HttpGet]
        public ActionResult MyStore()
        {
            if (Session.Count <= 0 || Session["Username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Service.StoreService ss = new Service.StoreService();
                List<Models.Store> stores = ss.LoadAllStore().FindAll(x => x.Creater.Equals(Session["Username"].ToString()));
                if (stores.Count > 0)
                {
                    return View(stores);
                }
                else
                {
                    ViewBag.SearchHtml = "<div class=\"alert alert-info\" role=\"alert\">[Info] 無任何已建立之店家資訊！</div>";
                    return View();
                }
            }
        }
    }
}