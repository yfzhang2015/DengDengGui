using System;
using System.Collections.Generic;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;

/// <summary>
/// 增删改查全民打赏模板信息
/// </summary>
namespace QuanMinDaShangPlatform.Models.Repository
{
    public class QMDSTemplateRepository : IQMDSTemplateRepository
    {
        private IDapperPlusDB dapperPlusDB;
        public QMDSTemplateRepository(IDapperPlusDB dapperPlusDB)
        {
            this.dapperPlusDB = dapperPlusDB;
        }

        /// <summary>
        /// 添加全民打赏信息
        /// </summary>
        /// <param name="tQMDSTemplate">全民打赏信息</param>
        /// <returns></returns>
        public bool AddQMDSTemplate(TQMDSTemplate tQMDSTemplate)
        {
            if (tQMDSTemplate==null)
            {
                throw new Exception("打赏模板信息不能为空");
            }
            var sql = @"INSERT INTO [dbo].[T_QMDS_Template]
           ([DSTemplateType]
           ,[DS1]
           ,[DS2]
           ,[DS3]
           ,[DS4]
           ,[DSTemplateName]
           ,[CreateTime]
           ,[CreaterID]
           ,[FValid]
           ,[CompanyID])
     VALUES
           (@DSTemplateType
           ,@DS1
           ,@DS2
           ,@DS3
           ,@DS4
           ,@DSTemplateName
           ,GETDATE()
           ,@CreaterID
           ,1
           ,@CompanyID)";
            return dapperPlusDB.Execute(sql,tQMDSTemplate)>0;
        }


        /// <summary>
        /// 删除打赏模板信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteQMDSTemplate(int id)
        {
            if (id<1)
            {
                throw new Exception("打赏模板不存在");
            }
            var sql = @"UPDATE [dbo].[T_QMDS_Template]
   SET FValid=0
 WHERE ID=@id";
            return dapperPlusDB.Execute(sql,new { ID=id})>0;
        }

        /// <summary>
        /// 修改打赏模板信息
        /// </summary>
        /// <param name="tQMDSTemplate"></param>
        /// <returns></returns>
        public bool ModifyQMDSTemplate(TQMDSTemplate tQMDSTemplate)
        {
            if (tQMDSTemplate==null)
            {
                throw new Exception("打赏模板信息不能为空");
            }
            var sql = @"UPDATE [dbo].[T_QMDS_Template]
   SET [DSTemplateType] = @DSTemplateType
      ,[DS1] = @DS1
      ,[DS2] = @DS2
      ,[DS3] = @DS3
      ,[DS4] = @DS4
      ,[DSTemplateName] = @DSTemplateName
      ,[ModifyTime] = GETDATE()
      ,[ModifierID] = @ModifierID
 WHERE ID=@id";
            return dapperPlusDB.Execute(sql,tQMDSTemplate)>0;
        }

        /// <summary>
        /// 查询打赏模板信息通过ID
        /// </summary>
        /// <param name="id">模板信息ID</param>
        /// <returns></returns>
        public IEnumerable<TQMDSTemplate> QueryTemplateByTemplateID(int id)
        {
            if (id<1)
            {
                throw new Exception("打赏模板不存在");
            }
            var sql = @"SELECT [ID]
      , [DSTemplateType]
      ,[DS1]
      ,[DS2]
      ,[DS3]
      ,[DS4]
      ,[DSTemplateName]
      ,[CreateTime]
      ,[CreaterID]
      ,[ModifyTime]
      ,[ModifierID]
      ,[FValid]
      ,[CompanyID]
  FROM [dbo].[T_QMDS_Template]
  WHERE FValid=1 AND ID=@id";
            return dapperPlusDB.Query<TQMDSTemplate>(sql,new { ID=id});
        }

        /// <summary>
        /// 查询所有模板信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TQMDSTemplate> QureyTemplates(string companyID)
        {
            var sql = @"SELECT [ID]
      , [DSTemplateType]
      ,[DS1]
      ,[DS2]
      ,[DS3]
      ,[DS4]
      ,[DSTemplateName]
      ,[CreateTime]
      ,[CreaterID]
      ,[ModifyTime]
      ,[ModifierID]
      ,[FValid]
      ,[CompanyID]
  FROM [dbo].[T_QMDS_Template]
  WHERE FValid=1 AND CompanyID=@companyID";
            return dapperPlusDB.Query<TQMDSTemplate>(sql,new { CompanyID=companyID});
        }
    }
}
