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

namespace ClientUI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AbsAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            //1. 如果session中不存在user对象，说明session超时或者用户没有登录
            var session = httpContext.Session;
            if (session["user"] == null)
            {
                //var cookie = httpContext.Request.Cookies["usrck"];
                //if (cookie == null)
                //{
                    AbsAuthorizeLoginResult result = AbsAuthorizeLogin.AuthorizeCore(httpContext.Request["token"], "http://localhost:54805/");
                    if (!result.Success)
                        httpContext.Response.StatusCode = 401;
                    else
                    {
                        session["user"] = result.User;
                        httpContext.Request.Cookies.Add(new HttpCookie("usrck", JsonConvert.SerializeObject(result.User)));
                    }
                //}
                //else
                //{
                //    session["user"] = JsonConvert.DeserializeObject<SessionUser>(cookie.Value);
                //}
            }
            //3. 通过角色鉴权
            return true;
        }
      
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.Response.StatusCode == 405)
            {
                string backurl = filterContext.HttpContext.Request.Url.ToString();
                filterContext.Result = new RedirectResult("http://localhost:54805/Account/ValidLogin?backUrl=" + backurl + "&sysId=1");
            }
            else
            {
                base.OnAuthorization(filterContext);
            }

        }
    }
}