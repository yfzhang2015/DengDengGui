using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanMinDaShangPlatform.Models.Entity;

namespace QuanMinDaShangPlatform.Models.IRepository
{
    public interface IEmployeeTemplateRelationRepository
    {
        /// <summary>
        /// 添加人员打赏模板
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <param name="templateID">模板ID</param>
        /// <returns></returns>
        bool AddEmployeeTemplateRelation(string companyID,int templateID);

        /// <summary>
        /// 删除人员打赏模板信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool DeleteEmployeeTemplateRelation(int id);

        /// <summary>
        /// 修改人员打赏模板信息
        /// </summary>
        /// <param name="tQMDSEmployeeTemplateRelation">人员打赏模板信息</param>
        /// <returns></returns>
        bool ModifyEmployeeTemplateRelation(TQMDSEmployeeTemplateRelation tQMDSEmployeeTemplateRelation);

        /// <summary>
        /// 查询所有人员打赏模板信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> QueryEmployeeTemplateRelation(string companyID);

        bool DeleteEmployeeTemplateRelationsByComponyID(string compamyID,int templateID);

        /// <summary>
        /// 查询人员的打赏设置信息
        /// </summary>
        /// <param name="employeeID">人员ID</param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryEmployeeDsSetMessage(string employeeID);

        /// <summary>
        /// 查询打赏单人的打赏信息
        /// </summary>
        /// <param name="id">人员模板关系ID</param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryEmployeeDsSetByID(int id);
       
    }
}
