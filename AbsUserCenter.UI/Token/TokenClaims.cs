using Newtonsoft.Json;
using System;

namespace AbsUserCenter.Token
{
    /// <summary>
    /// 一个token所包含的数据
    /// </summary>
    public class TokenClaims
    {
        public TokenPayload TokenPayload { get; set; }
    }


    public class TokenPayload
    {
        /// <summary>
        /// token的发行者
        /// </summary>
        public string Iss { get; set; }
        /// <summary>
        /// 用户登录名
        /// </summary>
        public string Name { get; set; }
        public int SysId { get; set; }
        /// <summary>
        /// 签发时间 秒
        /// </summary>
        public long Iat { get; set; }
        /// <summary>
        /// 到期时间 秒
        /// </summary>
        public long Exp { get; set; }
        /// <summary>
        /// 用户唯一标识id
        /// </summary>
        public long UsrId { get; set; }
    }
}
