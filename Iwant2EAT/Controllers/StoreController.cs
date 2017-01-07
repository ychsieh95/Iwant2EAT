using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Store store)
        {
            ViewBag.AddStoreActive = "active";
            string checkStore = store.CheckStoreFormat();
            if (!string.IsNullOrEmpty(checkStore))
            {
                ViewBag.CreateStoreHtml = checkStore;
                return View();
            }

            //
            store.DayOff = (store.Sunday ? "" : "0;") + (store.Monday ? "" : "1;") + (store.Tuesday ? "" : "2;") + (store.Wednesday ? "" : "3;") + (store.Thursday ? "" : "4;") + (store.Friday ? "" : "5;") + (store.Saturday ? "" : "6;");
            //
            store.Creater = Session["Username"].ToString();
            // Only id of store
            store.Guid = Guid.NewGuid().ToString();

            Services.StoreService ss = new Services.StoreService();
            //Check Duplicate
            if (ss.LoadAllStore().Any(x => x.Name.Equals(store.Name) && x.Branch.Equals(store.Branch)))
            {
                ViewBag.CreateStoreHtml = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 已有重複店家資訊！</div>";
                return View();
            }
            else
            {
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

                if (ss.AddStore(store))
                {
                    // 導向到詳細頁面
                    TempData["CreateStoreHtml"] = "<div class=\"alert alert-success\" role=\"alert\">[Success] 店家資訊建立成功！</div>";
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
            return View(new Services.StoreService().LoadAllStore(Session["Username"].ToString()).Find(x => x.Guid.Equals(Guid)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(Models.Store store)
        {
            string checkStore = store.CheckStoreFormat();
            if (!string.IsNullOrEmpty(checkStore))
            {
                ViewBag.ModifyStoreHTML = checkStore;
                return View(store);
            }

            Services.StoreService ss = new Services.StoreService();
            if (!ss.LoadAllStore().Find(x => x.Guid.Equals(store.Guid)).Creater.Equals(Session["Username"].ToString()))
            {
                TempData["ModifyStoreHtml"] = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 權限錯誤！</div>";
                return RedirectToAction("Detail", new { Guid = store.Guid });
            }
            else
            {
                //
                store.DayOff = (store.Sunday ? "" : "0;") + (store.Monday ? "" : "1;") + (store.Tuesday ? "" : "2;") + (store.Wednesday ? "" : "3;") + (store.Thursday ? "" : "4;") + (store.Friday ? "" : "5;") + (store.Saturday ? "" : "6;");
                //
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
                // Check Duplicate
                if (ss.LoadAllStore().Any(x => x.Name.Equals(store.Name) && x.Branch.Equals(store.Branch) && !x.Guid.Equals(store.Guid)))
                {
                    ViewBag.ModifyStoreHtml = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 已有重複店家資訊！</div>";
                    return View(store);
                }

                //
                if (ss.UpdateStore(store))
                {
                    TempData["ModifyStoreHtml"] = "<div class=\"alert alert-success\" role=\"alert\">[Success] 更新店家資訊成功！</div>";
                    return RedirectToAction("Detail", new { Guid = store.Guid });
                }
                else
                {
                    TempData["ModifyStoreHtml"] = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 更新店家資訊時發生錯誤！</div>";
                    return RedirectToAction("Detail", new { Guid = store.Guid });
                }
            }
        }

        [HttpGet]
        public ActionResult Delete(string Guid)
        {
            Services.StoreService ss = new Services.StoreService();
            Models.Store store = ss.LoadAllStore().Find(x => x.Guid.Equals(Guid));

            if (!store.Creater.Equals(Session["Username"].ToString()))
            {
                TempData["DeleteStoreHtml"] = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 權限錯誤！</div>";
                return RedirectToAction("Detail", new { Guid = Guid });
            }
            else
            {
                string filePath = Request.MapPath(store.ImageUrl);
                if (ss.DeleteStore(Guid))
                {
                    // Delete image
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    // Delete collect
                    Services.CollectService cs = new Services.CollectService();
                    foreach (Models.Collect collect in new Services.CollectService().LoadAllCollect().FindAll(x => x.Guid.Equals(Guid)))
                    {
                        cs.DeleteCollect(collect);
                    }
                    // Delete reply
                    Services.ReplyServices rs = new Services.ReplyServices();
                    foreach (Models.lReply reply in new Services.ReplyServices().LoadAllReply().FindAll(x => x.StoreGuid.Equals(Guid)))
                    {
                        rs.DeleteReply(reply);
                    }
                    TempData["DeleteStoreHtml"] = string.Format("<div class=\"alert alert-success\" role=\"alert\">[success] 刪除 {0}{1} 資訊成功！</div>",
                                                                store.Name, string.IsNullOrEmpty(store.Branch) ? "" : string.Format("({0})", store.Branch));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["DeleteStoreHtml"] = string.Format("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 刪除 {0}{1} 資訊時發生錯誤！</div>",
                                                                store.Name, string.IsNullOrEmpty(store.Branch) ? "" : string.Format("({0})", store.Branch));
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
                Services.StoreService ss = new Services.StoreService();
                Models.Store store = 
                    ss.LoadAllStore(Session["Username"] == null ? "" : Session["Username"].ToString()).Find(x => x.Guid.Equals(Guid));

                if (store == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.IsCreater = false;
                if (Session["Username"] != null)
                {
                    if (Session["Username"].ToString().Equals(store.Creater))
                    {
                        ViewBag.IsCreater = true;
                    }
                    if (new Services.CollectService().LoadAllCollect().Any(x => x.Username.Equals(Session["Username"].ToString()) && x.Guid.Equals(Guid)))
                    {
                        ViewBag.Collect = true;
                    }
                }
                ViewBag.StoreName = store.Name + (string.IsNullOrEmpty(store.Branch) ? "" : string.Format(" ({0})", store.Branch));
                return View(store);
            }
        }

        [HttpGet]
        public ActionResult Collect(string Guid, bool Collect, string Rurl = "")
        {
            if (string.IsNullOrEmpty(Guid))
            {
                return RedirectToAction("Detail", "Store");
            }
            else
            {
                if (Session["Username"] != null)
                {
                    Models.Store store = new Services.StoreService().LoadAllStore().Find(x => x.Guid.Equals(Guid));
                    Services.CollectService ls = new Services.CollectService();
                    if (Collect)
                    {
                        if (ls.AddCollect(new Models.Collect() { Username = Session["Username"].ToString(), Guid = Guid }))
                        {
                            TempData["CollectHtml"] = string.Format("<div class=\"alert alert-success\" role=\"alert\">[Success] {0}{1} 添入收藏成功！</div>",
                                                                    store.Name, string.IsNullOrEmpty(store.Branch) ? "" : string.Format("({0})", store.Branch));
                        }
                        else
                        {
                            TempData["CollectHtml"] = string.Format("<div class=\"alert alert-danger\" role=\"alert\">[Failure] {0}{1} 添入收藏失敗！</div>",
                                                                    store.Name, string.IsNullOrEmpty(store.Branch) ? "" : string.Format("({0})", store.Branch));
                        }
                    }
                    else
                    {
                        if (ls.DeleteCollect(new Models.Collect() { Username = Session["Username"].ToString(), Guid = Guid }))
                        {
                            TempData["CollectHtml"] = string.Format("<div class=\"alert alert-success\" role=\"alert\">[Success] {0}{1} 取消收藏成功！</div>",
                                                                    store.Name, string.IsNullOrEmpty(store.Branch) ? "" : string.Format("({0})", store.Branch));
                        }
                        else
                        {
                            TempData["CollectHtml"] = string.Format("<div class=\"alert alert-danger\" role=\"alert\">[Failure] {0}{1} 取消收藏失敗！</div>",
                                                                    store.Name, string.IsNullOrEmpty(store.Branch) ? "" : string.Format("({0})", store.Branch));
                        }
                    }
                }

                // Redirect
                if (string.IsNullOrEmpty(Rurl))
                {
                    return RedirectToAction("Detail", "Store", new { Guid = Guid });
                }
                else
                {
                    return RedirectToAction(Rurl.Split('/').Last(), Rurl.Split('/').First());
                }
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
                Services.StoreService ss = new Services.StoreService();
                List<Models.Store> stores = ss.LoadAllStore(Session["Username"].ToString()).FindAll(x => x.Creater.Equals(Session["Username"].ToString()));
                if (stores.Count > 0)
                {
                    ViewBag.DayOfWeek = ((int)DateTime.Now.DayOfWeek).ToString();
                    return View(stores);
                }
                else
                {
                    ViewBag.SearchHtml = "<div class=\"alert alert-info\" role=\"alert\">[Info] 無任何已建立之店家資訊！</div>";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult MyCollect()
        {
            if (Session.Count <= 0 || Session["Username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Models.Store> stores = new Services.StoreService().LoadAllStore(Session["Username"].ToString()).FindAll(x => x.IsCollect);
                if (stores.Count > 0)
                {
                    ViewBag.DayOfWeek = ((int)DateTime.Now.DayOfWeek).ToString();
                    return View(stores);
                }
                else
                {
                    ViewBag.SearchHtml = "<div class=\"alert alert-info\" role=\"alert\">[Info] 無任何已收藏之店家資訊！</div>";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Reply(string StoreGuid)
        {
            if (string.IsNullOrEmpty(StoreGuid))
            {
                return RedirectToAction("Index", "Home");
            }

            Models.Store store = new Services.StoreService().LoadAllStore().Find(x => x.Guid.Equals(StoreGuid));
            if (store == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.StoreName = store.Name;
                ViewBag.BranchName = store.Branch;
                return View(new Models.Reply()
                {
                    Replys = new Services.ReplyServices().LoadAllReply().FindAll(x => x.StoreGuid.Equals(StoreGuid)),
                    newReply = new Models.lReply()
                    {
                        StoreGuid = StoreGuid
                    }
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReply(Models.Reply reply)
        {
            if (Session["Username"] == null)
            {
                TempData["CreateReplyHtml"] = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 登入後才可使用留言功能！</div>";
                return RedirectToAction("Reply", new { Guid = reply.newReply.StoreGuid });
            }
            else
            {
                reply.newReply.Creater = Session["Username"].ToString();
                reply.newReply.RecordTime = DateTime.Now;
            }
            string checkFormat = reply.newReply.CheckReplyFormat();
            if (!string.IsNullOrEmpty(checkFormat))
            {
                TempData["CreateReplyHtml"] = checkFormat;
            }
            else
            {
                Services.ReplyServices rs = new Services.ReplyServices();
                if (rs.AddReply(reply.newReply))
                {
                    TempData["CreateReplyHtml"] = "<div class=\"alert alert-success\" role=\"alert\">[Success] 留言成功！</div>";
                }
                else
                {
                    TempData["CreateReplyHtml"] = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 留言失敗！</div>";
                }
            }
            return RedirectToAction("Reply", new { StoreGuid = reply.newReply.StoreGuid });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReply(Models.Reply reply)
        {
            if (Session["Username"] == null || !Session["Username"].ToString().Equals(reply.newReply.Creater))
            {
                TempData["CreateReplyHtml"] = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 權限不足，無法刪除此留言！</div>";
            }
            else
            {
                reply.newReply.Creater = Session["Username"].ToString();
                Services.ReplyServices rs = new Services.ReplyServices();
                if (rs.DeleteReply(reply.newReply))
                {
                    TempData["DeleteReplyHtml"] = "<div class=\"alert alert-success\" role=\"alert\">[Success] 留言刪除成功！</div>";
                }
                else
                {
                    TempData["DeleteReplyHtml"] = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 留言刪除失敗！</div>";
                }
            }
            return RedirectToAction("Reply", new { StoreGuid = reply.newReply.StoreGuid });
        }
    }
}