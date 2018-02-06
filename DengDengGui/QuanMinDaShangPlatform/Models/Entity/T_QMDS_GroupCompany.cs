using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    /// <summary>
    /// 集团实体
    /// </summary>
    public class T_QMDS_GroupCompany
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 集团名称
        /// </summary>
        public string GCName { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreaterID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public string ModifierID { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}
