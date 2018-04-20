using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace AbsUserCenter.SSOClient
{
    public class AbsAuthorizeLogin
    {
        public static AbsAuthorizeLoginResult AuthorizeCore(string token, string ssourl)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AbsAuthorizeLoginResult(false, 401);
            }
            else
            {
                SessionUser userdata = new SsoApiReference(ssourl).GetUserDataByToken(token);
                if (userdata == null) //token超时
                {
                    return new AbsAuthorizeLoginResult(false, 405);
                }
                return new AbsAuthorizeLoginResult(true, 200, userdata);
            }
        }


    }
}
