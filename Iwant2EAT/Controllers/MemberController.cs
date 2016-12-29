using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public ActionResult Register(Models.Member member)
        {
            Service.MemberService ms = new Service.MemberService();
            List<Models.Member> memberList = ms.LoadAllMember();

            Session.Clear();
            if (memberList == null || memberList.Count <= 0)
            {
                if (ms.AddMember(member))
                {
                    Session.Add("Username", member.Username);
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

        [HttpPost]
        public ActionResult Login(Models.Member member)
        {
            Service.MemberService ms = new Service.MemberService();
            List<Models.Member> memberList = ms.LoadAllMember();

            Session.Clear();
            if (memberList.Any(x => x.Username.Equals(member.Username) && x.Password.Equals(member.Password)))
            {
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
    }
}