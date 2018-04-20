using AbsUserCenter.SSOClient;
using ClientUI.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ClientUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie!=null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                CookieUser user = JsonConvert.DeserializeObject<CookieUser>(ticket.UserData);
                ViewData["uname"] = user.LoginName;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}