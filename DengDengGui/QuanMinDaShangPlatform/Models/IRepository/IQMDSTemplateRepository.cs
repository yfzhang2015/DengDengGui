using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanMinDaShangPlatform.Models.Entity;

namespace QuanMinDaShangPlatform.Models.IRepository
{
    /// <summary>
    /// 打赏模板仓储类
    /// </summary>
    public interface IQMDSTemplateRepository
    {
        /// <summary>
        /// 添加全民打赏模板
        /// </summary>
        /// <param name="tQMDSTemplate">全民打赏模板信息</param>
        /// <returns></returns>
        bool AddQMDSTemplate(TQMDSTemplate tQMDSTemplate);

        /// <summary>
        /// 删除全民打赏模板
        /// </summary>
        /// <param name="id">全民打赏模板ID</param>
        /// <returns></returns>
        bool DeleteQMDSTemplate(int id);

        /// <summary>
        /// 修改全民打赏模板信息
        /// </summary>
        /// <param name="tQMDSTemplate">全民打赏模板信息</param>
        /// <returns></returns>
        bool ModifyQMDSTemplate(TQMDSTemplate tQMDSTemplate);

        /// <summary>
        /// 查询所有打赏模板信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<TQMDSTemplate> QureyTemplates(string companyID);

        /// <summary>
        /// 按照ID  查询一个模板的信息
        /// </summary>
        /// <param name="id">模板ID</param>
        /// <returns></returns>
        IEnumerable<TQMDSTemplate> QueryTemplateByTemplateID(int id);

      
    }
}
