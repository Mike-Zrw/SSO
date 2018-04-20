using AbsUserCenter.Core.Model.Dto;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using AbsUserCenter.Core.Dto;
using AbsUserCenter.Core.Model;
using System.Data.SqlClient;
using AbsUserCenter.Core.IService;
using Microsoft.Extensions.Logging;
using AbsUserCenter.Core.IRepository;

namespace AbsUserCenter.Service
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private IBPermissionRepository _pms;
        private IBUserRepository _usr;
        private IHUserRepository _hur;
        private IBRoleRepository _rol;
        public AccountService(IBPermissionRepository pms, IBUserRepository usr, IBRoleRepository rol, IHUserRepository hur, ILogger<AccountService> logger)
        {
            _pms = pms;
            _usr = usr;
            _rol = rol;
            _hur = hur;
            _logger = logger;
        }
        public ApiResponse<SessionUser> Login(string userName, string passWord, int sysId, string ip)
        {
            Core.Model.BUser user = ValidateLogonUser(userName, passWord);
            if (user != null)
            {
                Core.Model.BRole role = GetRoleByUserId(user.USR_ID);
                if (role == null)
                {
                    return new ApiResponse<SessionUser> { Success = false, Message = "当前登陆账户未分配角色权限！" };
                }
                //判断当前角色的权限数量
                List<Core.Model.BPermission> pmses = GetPermissionsBySysId(role.ROL_ID, sysId);
                if (pmses.Count() == 0)
                {
                    return new ApiResponse<SessionUser> { Success = false, Message = "您无权登录本系统，请联系系统管理员！" };
                }
                LogOn(user.USR_ID, userName, ip);
                return new ApiResponse<SessionUser>(true, "", new SessionUser() { UserId = user.USR_ID, LoginName = userName, UserRole = new UserRole() { ID = role.ROL_ID, Name = role.ROL_DESC, Permissions = pmses } });
            }
            return new ApiResponse<SessionUser> { Success = false, Message = "用户名或密码错误,或当前用户为无效状态！" };
        }

        public SessionUser GetUserData(long userid, int sysId)
        {
            BUser user = _usr.Get(Convert.ToInt32(userid));
            Core.Model.BRole role = GetRoleByUserId(user.USR_ID);
            List<Core.Model.BPermission> pmses = GetPermissionsBySysId(role.ROL_ID, sysId);
            return new SessionUser() { UserId = user.USR_ID, LoginName = user.USR_LOGINNAME, UserRole = new UserRole() { ID = role.ROL_ID, Name = role.ROL_DESC, Permissions = pmses } };
        }
        public List<Core.Model.BPermission> GetPermissionsBySysId(int roleId, int sysId)
        {
            List<Core.Model.BPermission> list = _pms.GetAllRw(string.Format(@" join LRolPms on PMS_ID =LRP_PMS_ID
where PMS_DELETED_FLAG=0 and PMS_SYS_ID={0} and LRP_ROL_ID={1}", sysId, roleId));
            return list;
        }

        public Core.Model.BUser ValidateLogonUser(string loginName, string password)
        {
            try
            {
                Core.Model.BUser user = _usr.GetAllRw(" where USR_LOGINNAME=@USR_LOGINNAME and USR_PASSWORD=@USR_PASSWORD and USR_ACCESS_FLAG=1",
               new SqlParameter("@USR_LOGINNAME", loginName), new SqlParameter("@USR_PASSWORD", Tool.MD5Lib.MD5(password))).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Core.Model.BRole GetRoleByUserId(int userid)
        {
            return _rol.GetAllRw(" join BUser  ON USR_ROL_ID = ROL_ID WHERE ROL_DELETED_FLAG = 0 AND USR_ID = " + userid).FirstOrDefault();
        }


        public void LogOn(int userid, string uname, string ip)
        {
            _usr.LogOn(userid);
            _hur.Add(new Core.Model.HUser()
            {
                HUR_CREATIONUID = uname,
                HUR_CREATION_DT = DateTime.Now,
                HUR_DT = DateTime.Now,
                HUR_TYPE = (byte)EnumHURType.LogIn,
                HUR_USR_ID = userid,
                HUR_IP = ip
            });
        }

        public enum EnumHURType : byte
        {
            AddUser = 1,
            LogIn = 2,
            LogOut = 3,
            ModifyPassword = 4,
            DisableAccount = 5,
            EnableAccount = 6,
        }
    }
}
