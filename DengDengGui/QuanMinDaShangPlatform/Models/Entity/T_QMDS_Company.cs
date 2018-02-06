using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    /// <summary>
    /// 公司实体
    /// </summary>
    public class T_QMDS_Company
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 集团ID
        /// </summary>
        public string GCID { get; set; }
        /// <summary>
        /// 公司名(商家名)
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 微信账号
        /// </summary>
        public string WeiXinPay { get; set; }
        /// <summary>
        /// 支付宝账号
        /// </summary>
        public string AliPay { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        public string License { get; set; }


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
