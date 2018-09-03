using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AbsUserCenter.Core.Dto;
using AbsUserCenter.Core.IService;
using AbsUserCenter.Core.Model.Dto;
using AbsUserCenter.Token;
using AbsUserCenter.Tool;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AbsUserCenter.UI.Controllers
{
    public class AccountController : Controller
    {

        private IHttpContextAccessor _accessor;
        private IAccountService _accSer;
        public AccountController(IHttpContextAccessor accessor, IAccountService accSer)
        {
            _accessor = accessor;
            _accSer = accSer;
        }
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户ID</param>
        /// <param name="passWord">用户密码</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(string userName, string passWord, string txtCode, string backUrl, int sysId)
        {
            string code = HttpContext.Session.GetString("CheckCode").ToLower();
            if (txtCode.ToLower() != code)
                return new JsonResult(new { success = false, msg = "验证码错误" });
            ApiResponse<SessionUser> loginResult = _accSer.Login(userName, passWord, sysId, GetClientIpAddress());
            if (!loginResult.Success)
            {
                return new JsonResult(new { success = false, msg = loginResult.Message });
            }
            var user = new ClaimsPrincipal(
             new ClaimsIdentity(new[]
             {
                        new Claim("LoginName",userName),
                        new Claim("UserId",loginResult.Data.UserId+""),
                        new Claim("sysId",sysId+""),
             }, CookieAuthenticationDefaults.AuthenticationScheme)
            );
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
            string tokenStr = TokenBuilder.MakeToken(userName, sysId, loginResult.Data.UserId);
            backUrl = HttpUtility.UrlDecode(backUrl);
            if (backUrl.Contains("?"))
            {
                backUrl = backUrl + "&token=" + tokenStr;
            }
            else
            {
                backUrl = backUrl + "?token=" + tokenStr;
            }
            return new JsonResult(new { success = true, msg = "登陆成功", backUrl = backUrl });//登录成功
        }
        public ActionResult ChkCode()
        {
            string checkCode = RandLib.CreateRandomCode(4);
            HttpContext.Session.SetString("CheckCode", checkCode);
            var buffer = RandLib.CreateImage(checkCode);
            return File(buffer, "image/Jpeg");
        }


        /// <summary>
        /// 检查用户是否登陆
        /// 若未登录跳转至登陆页面，否则跳回原页面
        /// </summary>
        /// <param name="returnurl"></param>
        /// <returns></returns>
        public IActionResult ValidLogin(string backUrl, int sysId)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string loginname = HttpContext.User.Claims.First().Value;
                int userid = Convert.ToInt32(HttpContext.User.Claims.ToList()[1].Value);
                string tokenStr = TokenBuilder.MakeToken(loginname, sysId, userid);
                backUrl = HttpUtility.UrlDecode(backUrl);
                backUrl = UrlRemoveParam(backUrl, "token");
                if (backUrl.Contains("?"))
                {
                    backUrl = backUrl + "&token=" + tokenStr;
                }
                else
                {
                    backUrl = backUrl + "?token=" + tokenStr;
                }
                return new RedirectResult(backUrl);
            }
            return new RedirectResult(string.Format("/Account/Login?backUrl={0}&sysId={1}", HttpUtility.UrlEncode(backUrl), sysId));
        }
        #region URL中去除指定参数 
        /// <summary> 
        /// 中去除指定参数 
        /// </summary> 
        /// <param name="url">地址</param> 
        /// <param name="param">参数</param> 
        /// <returns></returns> 
        public static string UrlRemoveParam(string url, string param)
        {
            string url1 = url;
            if (url.IndexOf(param) > 0)
            {
                if (url.IndexOf("&", url.IndexOf(param) + param.Length) > 0)
                {
                    url1 = url.Substring(0, url.IndexOf(param) - 1) + url.Substring(url.IndexOf("&", url.IndexOf(param) + param.Length) + 1);
                }
                else
                {
                    url1 = url.Substring(0, url.IndexOf(param) - 1);
                }
                return url1;
            }
            else
            {
                return url1;
            }
        }
        #endregion
        public async Task<IActionResult> Logout(string backUrl, int sysId)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string loginname = HttpContext.User.Claims.First().Value;
            return new RedirectResult(string.Format("/Account/Login?backUrl={0}&sysId={1}", HttpUtility.UrlEncode(backUrl), sysId));

        }


        private string GetClientIpAddress()
        {
            string result = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            return result;
        }
    }
}