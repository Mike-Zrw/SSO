using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace AbsUserCenter.SSOClient
{
    /// <summary>
    /// sso登陆验证
    /// </summary>
    public class AbsAuthorizeLogin
    {
        /// <summary>
        /// 验证用户是否在sso系统中已经登陆
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ssourl"></param>
        /// <returns></returns>
        public static AbsAuthorizeLoginResult AuthorizeCore(string token, string ssourl)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AbsAuthorizeLoginResult(false, "用户未登录", null);
            }
            else
            {
                SessionUser result = new SsoApiReference(ssourl).GetUserDataByToken(token);
                if (result == null) //token超时
                {
                    return new AbsAuthorizeLoginResult(false, "token无效", null);
                }
                return new AbsAuthorizeLoginResult(true, result);
            }
        }


    }
}
