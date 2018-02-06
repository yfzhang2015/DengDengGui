using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    /// <summary>
    /// 全名打赏模板表
    /// </summary>
    public class TQMDSTemplate
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 打赏模板类型ID
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
        /// 打赏金额3
        /// </summary>
        public decimal DS3 { get; set; }
        /// <summary>
        /// 打赏金额4
        /// </summary>
        public decimal DS4 { get; set; }
        /// <summary>
        /// 打赏金额模板名称
        /// </summary>
        public string DSTemplateName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreaterID { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public int ModifierID { get; set; }
        /// <summary>
        /// 公司IDsdfaa
        /// </summary>
        public string CompanyID { get; set; }
    }
}
