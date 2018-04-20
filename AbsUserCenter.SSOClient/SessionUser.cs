using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.SSOClient
{
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

    public class CookieUser
    {
        public long UserId { get; set; }
        public string LoginName { get; set; }
        public int RoleId { get; set; }
        public string RoleName{ get; set; }
    }
}
