using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.SSOClient
{
    /// <summary>
    /// SSOtoken验证结果
    /// </summary>
    public class AbsAuthorizeLoginResult
    {
        public bool Success { get; set; }
        public string ErrorMsg { get; set; }
        public SessionUser User { get; set; }
        public AbsAuthorizeLoginResult(bool success)
        {
            this.Success = success;
        }
        public AbsAuthorizeLoginResult(bool success, SessionUser user)
        {
            this.Success = success;
            this.User = user;
        }
        public AbsAuthorizeLoginResult(bool success, string errmsg, SessionUser user)
        {
            this.Success = success;
            this.User = user;
            this.ErrorMsg = errmsg;
        }
    }
}
