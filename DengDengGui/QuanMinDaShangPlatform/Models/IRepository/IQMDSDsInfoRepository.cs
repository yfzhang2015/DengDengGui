using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanMinDaShangPlatform.Models.Entity;

namespace QuanMinDaShangPlatform.Models.IRepository
{
    public interface IQMDSDsInfoRepository
    {
        /// <summary>
        /// 添加打赏信息
        /// </summary>
        /// <param name="tQMDSDsInfo">打赏明细信息</param>
        /// <returns></returns>
        bool AddQMDSDsInfo(TQMDSDsInfo tQMDSDsInfo);

        /// <summary>
        /// 修改打赏状态为确认状态
        /// </summary>
        /// <returns></returns>
        bool ModifyOrderStatusToSure();

        /// <summary>
        /// 查询人员打赏金额
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryEmployeeDsMoney(int compamyID);

       
       

    }
}
