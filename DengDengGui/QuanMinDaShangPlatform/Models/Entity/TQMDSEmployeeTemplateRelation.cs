using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    /// <summary>
    /// 人员打赏关系表
    /// </summary>
    public class TQMDSEmployeeTemplateRelation
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string EmployeeID { get; set; }

        /// <summary>
        /// 打赏模板名字
        /// </summary>
       public string DSTemplateName { get; set; }

        /// <summary>
        /// 打赏模板类型
        /// </summary>
        public int DSTemplateType { get; set; }

        /// <summary>
        /// 打赏金额1
        /// </summary>
        public decimal DS1 { get; set; }

        /// <summary>
        /// 打赏金额2
        /// </summary>
        public decimal DS2 { get; set; }

        /// <summary>
        ///打赏金额3 
        /// </summary>
        public decimal DS3 { get; set; }

        /// <summary>
        /// 打赏金额4
        /// </summary>
        public decimal DS4 { get; set; }

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
        /// 修改人ID
        /// </summary>
        public string ModifierID { get; set; }

        /// <summary>
        /// 有效性
        /// </summary>
        public int FValid { get; set; }
    }
}
