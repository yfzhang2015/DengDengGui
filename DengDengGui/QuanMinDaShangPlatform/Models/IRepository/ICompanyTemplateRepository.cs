using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanMinDaShangPlatform.Models.Entity;
namespace QuanMinDaShangPlatform.Models.IRepository
{
    /// <summary>
    /// 公司模板对应关系
    /// </summary>
    public interface ICompanyTemplateRepository
    {
        /// <summary>
        /// 添加公司打赏模板关系表
        /// </summary>
        /// <param name="tQMDSCompanyTemplateRelation">公司模板信息</param>
        /// <returns></returns>
        bool AddCompanyTemplateRelation(TQMDSCompanyTemplateRelation tQMDSCompanyTemplateRelation);

        /// <summary>
        /// 删除公司模板关系
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool DeleteCompanyTemplateRelation(int id);

        /// <summary>
        /// 修改公司模板关系
        /// </summary>
        /// <param name="tQMDSCompanyTemplateRelation">公司模板关系</param>
        /// <returns></returns>
        bool ModifyCompanyTemplateRelation(TQMDSCompanyTemplateRelation tQMDSCompanyTemplateRelation);

        /// <summary>
        /// 查询所有公司模板关系通过公司id
        /// </summary>
        /// <param name="id">公司ID</param>
        /// <returns></returns>
        dynamic QueryQMDSTemplateRelationByCompanyID(int id);
    }
}
