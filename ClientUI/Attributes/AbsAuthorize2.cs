using AbsUserCenter.SSOClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ClientUI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AbsAuthorize2 : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //本系统验证登陆
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //sso服务端验证是否登陆
                AbsAuthorizeLoginResult result = AbsAuthorizeLogin.AuthorizeCore(httpContext.Request["token"], "http://localhost:54805/");
                if (!result.Success)
                {
                    httpContext.Response.StatusCode = 401;
                    return false;
                }
                else
                {
                    string CookieUser = JsonConvert.SerializeObject(new CookieUser() { UserId = result.User.UserId, RoleId = result.User.UserRole.ID, LoginName = result.User.LoginName, RoleName = result.User.UserRole.Name });//序列化用户实体               
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(1, result.User.LoginName, DateTime.Now, DateTime.Now.AddDays(1), false, CookieUser);
                    HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(Ticket));//加密身份信息，保存至Cookie
                    httpContext.Response.Cookies.Add(Cookie);
                    Cookie.HttpOnly = true;
                    return true;
                }

            }
            return true;
        }

        /// <summary>
        /// 授权验证未成功
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Response.StatusCode == 401)  //未登陆或者token无效
            {
                string backurl = filterContext.HttpContext.Request.Url.ToString();
                filterContext.Result = new RedirectResult("http://localhost:54805/Account/ValidLogin?backUrl=" + HttpUtility.UrlEncode(backurl) + "&sysId=1");
            }
        }

    }
}