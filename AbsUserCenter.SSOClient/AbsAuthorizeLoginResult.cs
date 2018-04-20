using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.SSOClient
{
    public class AbsAuthorizeLoginResult
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public SessionUser User { get; set; }
        public AbsAuthorizeLoginResult(bool success, int status)
        {
            this.Success = success;
            this.StatusCode = status;
        }
        public AbsAuthorizeLoginResult(bool success, int status, SessionUser user)
        {
            this.Success = success;
            this.StatusCode = status;
            this.User = user;
        }
    }
}
