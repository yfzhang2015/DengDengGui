using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanMinDaShangPlatform.Models.Entity;

namespace QuanMinDaShangPlatform.Models.IRepository
{
    public interface IDsInfoRepository
    {
        /// <summary>
        /// 添加打赏信息
        /// </summary>
        /// <param name="tQMDSDsInfo">打赏信息</param>
        /// <returns></returns>
        bool AddDsInfoRepository(TQMDSDsInfo tQMDSDsInfo);

        /// <summary>
        /// 修改登记信息为有效
        /// </summary>
        /// <returns></returns>
        bool ModifyDsInfoRepository(string orderNo);

        /// <summary>
        /// 通过订单号查询订单信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        IEnumerable<TQMDSDsInfo> QueryDsInfoRepository(string orderNo);

        /// <summary>
        /// 验证员工的有效性
        /// </summary>
        /// <param name="openID">openID</param>
        /// <returns></returns>
        IEnumerable<T_QMDS_Employee> QueryEmployeeByOpenID(string openID);

        /// <summary>
        /// 通过电话和密码查询员工是否存在
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <param name="passwor">密码</param>
        /// <returns></returns>
        IEnumerable<T_QMDS_EmployeeUser> QueryEmployeeByPhoneAndPssword(string phone, string passwor);

        /// <summary>
        /// 绑定OpenID
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="employeeID">人员ID</param>
        /// <returns></returns>
        bool BindEmployeeOpenID(string openID,string employeeID);

        /// <summary>
        /// 员工查询自己的打赏明细
        /// </summary>
        /// <param name="index">行号</param>
        /// <param name="openID">openid</param>
        /// <param name="stat">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryEmployeeInfo(int index,string openID,string stat,string end,out decimal 
            totalFee);

        /// <summary>
        /// 后台查询员工被打赏的信息
        /// </summary>
        /// <param name="index">分页索引值</param>
        /// <param name="employeeid">员工ID</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="totalFee">打赏总金额</param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryEmployeeInfoByEmployeeID(int index,string employeeid,string start,string end,out decimal totalFee);


        IEnumerable<dynamic> QueryCompanyEmployeeDsInfoByCompanyID(int index, string employeeid, string start, string end, out decimal totalFee);
       
    }
}
