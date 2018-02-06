using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    /// <summary>
    /// 打赏明细
    /// </summary>
    public class TQMDSDsInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 人员ID
        /// </summary>
        public string EmployeeID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 打赏金额
        /// </summary>
        public decimal DsMoney { get; set; }
        /// <summary>
        /// 打赏人openid
        /// </summary>
        public string DsOpenID { get; set; }
        /// <summary>
        /// 打赏时间
        /// </summary>
        public DateTime DsTime { get; set; }
        /// <summary>
        /// 打赏状态
        /// </summary>
        public int DsStatus { get; set; }
        /// <summary>
        /// 微信单号
        /// </summary>
        public string TransanctionID { get; set; }
    }
}
