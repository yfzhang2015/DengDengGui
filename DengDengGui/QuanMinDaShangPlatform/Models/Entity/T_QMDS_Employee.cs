using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    public class T_QMDS_Employee
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 集团ID
        /// </summary>
        public string GCID { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// 员工号
        /// </summary>
        public string ENumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string EName { get; set; }
        /// <summary>
        /// 工作昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string NativePlace { get; set; }
        /// <summary>
        /// 主页留言
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 本人照片
        /// </summary>
        public string Picture { get; set; }
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
