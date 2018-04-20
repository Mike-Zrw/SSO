using AbsUserCenter.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.Core.Dto
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
        public IEnumerable<BPermission> Permissions { get; set; }
    }
}
