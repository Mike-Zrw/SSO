using System;
using System.Collections.Generic;
using System.Text;
using AbsUserCenter.Core.Dto;
using AbsUserCenter.Core.Model;
using AbsUserCenter.Core.Model.Dto;

namespace AbsUserCenter.Core.IService
{
    public  interface IAccountService
    {
        List<BPermission> GetPermissionsBySysId(int roleId, int sysId);
        ApiResponse<SessionUser> Login(string uSR_LOGINNAME, string uSR_PASSWORD, int sysid, string v);
        SessionUser GetUserData(long usrId, int sysId);
    }
}
