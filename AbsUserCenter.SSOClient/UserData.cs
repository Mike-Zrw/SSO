using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.SSOClient
{
    public class UserData
    {
        public BUser User { get; set; }
        public BRole Role { get; set; }
        public List<BPermission> Permissions { get; set; }
    }
    public partial class BUser
    {
        /// <summary>
        /// 
        /// </summary>
        public int USR_ID { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string USR_LOGINNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string USR_PASSWORD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string USR_NAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string USR_EMAIL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string USR_BOSS_EMAIL { get; set; }
        /// <summary>
        /// 是否可用（1.可用；0.禁用）
        /// </summary>
        public bool USR_ACCESS_FLAG { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> USR_LASTACTIVE_DT { get; set; }
        /// <summary>
        /// 是否在线（1.在线；0.离线；默认0）
        /// </summary>
        public bool USR_ONLINE_FLAG { get; set; }
        /// <summary>
        /// 角色ID(对应BRole表)
        /// </summary>
        public Nullable<int> USR_ROL_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string USR_CREATIONUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime USR_CREATION_DT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string USR_WECHAT_OPENID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string USR_PHONE { get; set; }

    }

    public partial class BRole
    {
        /// <summary>
        /// 
        /// </summary>
        public int ROL_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ROL_DESC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ROL_MEMO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ROL_ORDER_NUM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ROL_DELETED_FLAG { get; set; }
        /// <summary>
        /// 部门ID(对应BDepartment表)
        /// </summary>
        public Nullable<int> ROL_DPT_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ROL_CREATIONUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ROL_CREATION_DT { get; set; }

    }

    public partial class BPermission
    {
        /// <summary>
        /// 
        /// </summary>
        public int PMS_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PMS_DESC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> PMS_PMT_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PMS_PARENT_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> PMS_SYS_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool PMS_DELETED_FLAG { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PMS_ORDER_NUM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PMS_CREATIONUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PMS_CREATION_DT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PMS_URL { get; set; }

    }
}
