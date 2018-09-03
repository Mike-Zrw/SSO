using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AbsUserCenter.Core.Dto;
using AbsUserCenter.Core.IService;
using AbsUserCenter.Core.Model;
using AbsUserCenter.Core.Model.Dto;
using AbsUserCenter.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AbsUserCenter.UI.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Area("Api")]
    //[Route("rest/Account")]
    public class AccountController : Controller
    {
        private IHttpContextAccessor _accessor;
        private IAccountService _accSer;
        public AccountController(IHttpContextAccessor accessor, IAccountService accSer)
        {
            _accessor = accessor;
            _accSer = accSer;
        }
        /// <summary>
        /// 检查用户是否登陆
        /// </summary>
        /// <param name="returnurl"></param>
        /// <returns></returns>
        //[Route("GetUserDataByToken")]
        [HttpPost]
        public SessionUser GetUserDataByToken()
        {
            string token = Request.Headers["token"];
            try
            {
                TokenClaims claim = TokenBuilder.DecodeToken(token);
                if (claim == null || TokenBuilder.IsOverTime(claim))
                {
                    return null;
                }
                else
                {
                    SessionUser data = _accSer.GetUserData(claim.TokenPayload.UsrId, claim.TokenPayload.SysId);
                    return data;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
         
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse<SessionUser> Login([FromQuery]int sysid, [FromBody]BUser user)
        {
            string ip = GetClientIpAddress();
            ApiResponse<SessionUser> loginResult = _accSer.Login(user.USR_LOGINNAME, user.USR_PASSWORD, sysid, HttpContext.Request.Host.ToString());
            return loginResult;
        }

        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<BPermission> GetMenuPermission([FromQuery]int roleId, int sysId)
        {
            List<BPermission> pmses = _accSer.GetPermissionsBySysId(roleId, sysId);
            return pmses;
        }


        private string GetClientIpAddress()
        {
            string result = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            return result;
        }
    }
}