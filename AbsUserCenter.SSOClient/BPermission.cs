using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.SSOClient
{
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
