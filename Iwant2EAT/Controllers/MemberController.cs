using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Iwant2EAT.Controllers
{
    public class MemberController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.Member member)
        {
            string checkMsg = member.CheckMember();
            if (!string.IsNullOrEmpty(checkMsg))
            {
                ViewBag.AddMemberHTML = checkMsg;
                return View();
            }

            member.LastLogin = DateTime.Now;
            member.LastIpAdr = Request.UserHostAddress;

            Services.MemberService ms = new Services.MemberService();
            List<Models.Member> memberList = ms.LoadAllMember().FindAll(x => x.Username.Equals(member.Username));

            Session.Clear();
            if (memberList == null || memberList.Count <= 0)
            {
                if (ms.AddMember(member))
                {
                    Session.Add("Username", member.Username);
                    ViewBag.AddMemberHTML = "<div class=\"alert alert-success\" role=\"alert\">[Success] 帳號註冊成功！</div>";
                    return Redirect("/Home/Index");
                }
                else
                {
                    ViewBag.AddMemberHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 帳號註冊失敗！</div>";
                    return View();
                }
            }
            else
            {
                if (memberList.Any(x => x.Username.Equals(member.Username)))
                {
                    ViewBag.AddMemberHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 帳號已被使用！</div>";
                    return View();
                }
                else if (memberList.Any(x => x.Email.Equals(member.Email)))
                {
                    ViewBag.AddMemberHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 信箱已被使用！</div>";
                    return View();
                }
                else
                {
                    ViewBag.AddMemberHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 帳號註冊失敗！</div>";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Member member)
        {
            string checkMsg = member.CheckMember();
            if (!string.IsNullOrEmpty(checkMsg))
            {
                ViewBag.AddMemberHTML = checkMsg;
                return View();
            }

            member.LastLogin = DateTime.Now;
            member.LastIpAdr = Request.UserHostAddress;

            Services.MemberService ms = new Services.MemberService();
            List<Models.Member> memberList = ms.LoadAllMember();

            Session.Clear();
            if (memberList.Any(x => x.Username.Equals(member.Username) && x.Password.Equals(member.Password)))
            {
                // Update login time and ip
                ms.UpdateMember(string.Format("LastLogin='{0}', LastIpAdr='{1}'", member.LastLogin.ToString("yyyy/MM/dd HH:mm:ss"), member.LastIpAdr), string.Format("Username='{0}'", member.Username));
                Session.Add("Username", member.Username);
                return Redirect("/Home/Index");
            }
            else
            {
                if (memberList.Any(x => x.Username.Equals(member.Username)))
                {
                    ViewBag.AddMemberHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 密碼驗證錯誤！</div>";
                    return View();
                }
                else
                {
                    ViewBag.AddMemberHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 帳號不存在！</div>";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public ActionResult Profile()
        {
            if (Session.Count <= 0)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                Services.MemberService ms = new Services.MemberService();
                Models.Member member = ms.LoadAllMember().Find(x => x.Username.Equals(Session["Username"]));
                ViewBag.Email = member.Email;
                ViewBag.LastLogin = member.LastLogin;
                ViewBag.LastIpAdr = member.LastIpAdr;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ChangeEmail(string password, string newEmail)
        {
            Services.MemberService ms = new Services.MemberService();
            List<Models.Member> memberList = ms.LoadAllMember();
            
            if (memberList.Find(x => x.Username.Equals(Session["Username"].ToString())).Password.Equals(password))
            {
                if (ms.UpdateMember(setCommand: string.Format("Email='{0}'", newEmail), whereCommand: string.Format("Username='{0}'", Session["Username"].ToString())))
                {
                    ViewBag.ChangeEmailHTML = "<div class=\"alert alert-success\" role=\"alert\">[Failure] 信箱修改成功！</div>";
                }
                else
                {
                    ViewBag.ChangeEmailHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 信箱修改失敗！</div>";
                }
            }
            else
            {
                ViewBag.ChangeEmailHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 密碼驗證錯誤！</div>";
            }
            return View("Profile");
        }

        [HttpPost]
        public ActionResult ChangePwd(string password, string newPassword)
        {
            Services.MemberService ms = new Services.MemberService();
            List<Models.Member> memberList = ms.LoadAllMember();

            if (memberList.Find(x => x.Username.Equals(Session["Username"].ToString())).Password.Equals(password))
            {
                if (ms.UpdateMember(setCommand: string.Format("Password='{0}'", newPassword), whereCommand: string.Format("Username='{0}'", Session["Username"].ToString())))
                {
                    ViewBag.ChangePwdHTML = "<div class=\"alert alert-success\" role=\"alert\">[Failure] 密碼修改成功！</div>";
                }
                else
                {
                    ViewBag.ChangePwdHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 密碼修改失敗！</div>";
                }
            }
            else
            {
                ViewBag.ChangePwdHTML = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 密碼驗證錯誤！</div>";
            }
            return View("Profile");
        }

        [HttpPost]
        public ActionResult DelAccount(string password)
        {
            Services.MemberService ms = new Services.MemberService();
            List<Models.Member> memberList = ms.LoadAllMember();

            if (memberList.Find(x => x.Username.Equals(Session["Username"].ToString())).Password.Equals(password))
            {
                // Delete account
                if (ms.DeleteMember(username: Session["Username"].ToString()))
                {
                    // Delete store
                    Services.StoreService ss = new Services.StoreService();
                    foreach (Models.Store store in ss.LoadAllStore().FindAll(x => x.Creater.Equals(Session["Username"].ToString())))
                    {
                        if (System.IO.File.Exists(Request.MapPath(store.ImageUrl)))
                        {
                            System.IO.File.Delete(Request.MapPath(store.ImageUrl));
                        }
                        ss.DeleteStore(store.Guid);
                    }
                    // Delete collect
                    Services.CollectService cs = new Services.CollectService();
                    foreach (Models.Collect collect in cs.LoadAllCollect().FindAll(x => x.Username.Equals(Session["Username"].ToString())))
                    {
                        cs.DeleteCollect(collect);
                    }
                    ViewBag.DelAccount = "<div class=\"alert alert-success\" role=\"alert\">[Failure] 帳戶刪除成功！</div>";
                    Session.Clear();
                    return Redirect("/Home/Index");
                }
                else
                {
                    ViewBag.DelAccount = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 帳戶刪除失敗！</div>";
                }
            }
            else
            {
                ViewBag.DelAccount = "<div class=\"alert alert-danger\" role=\"alert\">[Failure] 密碼驗證錯誤！</div>";
            }
            return View("Profile");
        }
    }
}