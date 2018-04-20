using AbsUserCenter.Core.IRepository;
using AbsUserCenter.Repository.DBUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.Repository
{
    public partial class BUserRepository : IBUserRepository
    {
        public void LogOn(int userid)
        {
            string sqlupd = "update BUser set USR_ONLINE_FLAG=1 where USR_ID=" + userid;
            SqlHelper.ExecuteNonQuery(SqlHelper.RwViewConnString, System.Data.CommandType.Text, sqlupd);
        }
    }
}
