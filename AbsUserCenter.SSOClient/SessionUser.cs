using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.SSOClient
{
    /// <summary>
    /// 保存在token中的用户信息
    /// </summary>
    public class SessionUser
    {
        public long UserId { get; set; }
        public string LoginName { get; set; }
        public UserRole UserRole { get; set; }
    }
    public class UserRole
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<BPermission> Permissions { get; set; }
    }

    /// <summary>
    /// 保存在cookie中的用户信息
    /// </summary>
    public class CookieUser
    {
        public long UserId { get; set; }
        public string LoginName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
