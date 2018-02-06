using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    /// <summary>
    /// 公司打赏模板关系表
    /// </summary>
    public class TQMDSCompanyTemplateRelation
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public int TemplateID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建者ID
        /// </summary>
        public string CreaterID { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 修改者ID
        /// </summary>
        public string ModifierID {get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        public int FValid { get; set; }

    }
}
