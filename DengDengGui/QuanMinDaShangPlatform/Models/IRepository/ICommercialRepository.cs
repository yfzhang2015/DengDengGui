using QuanMinDaShangPlatform.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.IRepository
{
    /// <summary>
    /// 商户注册业务接口
    /// </summary>
    public interface ICommercialRepository
    {

        /// <summary>
        /// 商户后台登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        List<dynamic> Login(string username, string password);

        bool ModifyPassword(string username, string password);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="company"></param>
        /// <param name="groupCompany"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Register(T_QMDS_Company company, T_QMDS_GroupCompany groupCompany, string username, string password);
        /// <summary>
        /// 确保用户名的唯一性
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        bool CheckUser(string username);

        /// <summary>
        /// 生成员工编号
        /// </summary>
        /// <returns></returns>
        string YGNumber();

        /// <summary>
        /// 生成集团公司人员ID
        /// </summary>
        /// <param name="lxnum">ID类型：1集团，2公司，3人员，4单据号</param>
        /// <returns></returns>
        string ProduceID(int lxnum);

        #region 人员增删改查

        /// <summary>
        /// 添加人员并设置密码
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        bool AddEmployee(T_QMDS_Employee employee, string password);

        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteEmployee(string id);

        /// <summary>
        /// 修改人员
        /// </summary>
        /// <param name="employee">添加人员</param>
        /// <returns></returns>
        bool ModifyEmployee(T_QMDS_Employee employee);
        /// <summary>
        /// 查询人员
        /// </summary>
        /// <param name="id">人员ID</param>
        /// <returns></returns>
        List<T_QMDS_Employee> GetEmployeeByID(string id);

        //查询人员打赏总金额
        dynamic GetEmployeeDsMoneyID(string id);
        /// <summary>
        /// 查询公司下全部人员
        /// </summary>
        /// <param name="companyID">公司</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageRow">行数</param>
        /// <returns></returns>
        List<T_QMDS_Employee> GetEmployees(string companyID, int pageIndex, int pageRow);

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="pageRow"></param>
        /// <returns></returns>
        string QueryEmployeesCount(int pageRow);

        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        bool CheckPhone(string phone);
        
        #endregion

        #region 公司


        /// <summary>
        /// 查询公司
        /// </summary>
        /// <returns></returns>
        List<T_QMDS_Company> GetCompanies();

        T_QMDS_Company GetCompanByID(string id);
        /// <summary>
        /// 删除公司
        /// </summary>
        /// <param name="id">公司ID</param>
        /// <returns></returns>
        bool DeleteCompany(string id);

        /// <summary>
        /// 修改公司
        /// </summary>
        /// <param name="employee">添加公司</param>
        /// <returns></returns>
        bool ModifyCompany(T_QMDS_Company company);
        #endregion

        #region 集团

        /// <summary>
        /// 添加集团
        /// </summary>
        /// <param name="groupCompany">集团实体</param>
        /// <returns></returns>
        bool AddGroupCompany(T_QMDS_GroupCompany groupCompany);
        /// <summary>
        /// 查询集团
        /// </summary>
        /// <returns></returns>
        List<T_QMDS_GroupCompany> GetGroupCompanies();


        /// <summary>
        /// 删除集团
        /// </summary>
        /// <param name="id">集团ID</param>
        /// <returns></returns>
        bool DeleteGroupCompany(string id);

        /// <summary>
        /// 修改集团
        /// </summary>
        /// <param name="employee">添加集团</param>
        /// <returns></returns>
        bool ModifyGroupCompany(T_QMDS_GroupCompany groupCompany);
        #endregion

    }
}
