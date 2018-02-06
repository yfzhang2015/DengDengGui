using System;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;

namespace QuanMinDaShangPlatform.Models.Repository
{
    public class CompanyTemplateRepository : ICompanyTemplateRepository
    {
        private IDapperPlusDB dapperPlusDB;
        public CompanyTemplateRepository(IDapperPlusDB dapperPlusDB)
        {
            this.dapperPlusDB = dapperPlusDB;
        }

        /// <summary>
        /// 添加公司打赏模板
        /// </summary>
        /// <param name="tQMDSCompanyTemplateRelation">公司打赏模板关系</param>
        /// <returns></returns>
        public bool AddCompanyTemplateRelation(TQMDSCompanyTemplateRelation tQMDSCompanyTemplateRelation)
        {
            if (tQMDSCompanyTemplateRelation==null)
            {
                throw new Exception("公司模板信息不能为空");
            }
            var sql = @"INSERT INTO [dbo].[T_QMDS_CompanyTemplateRelation]
           ([CompanyID]
           ,[TemplateID]
           ,[CreateTime]
           ,[CreaterID]
           ,[FValid])
     VALUES
           (@CompanyID
           ,@TemplateID
           ,GETDATE()
           ,@CreaterID         
           ,1)";
            return dapperPlusDB.Execute(sql, tQMDSCompanyTemplateRelation) > 0;
        }

        /// <summary>
        /// 删除公司打赏模板关系
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool DeleteCompanyTemplateRelation(int companyID)
        {
            if (companyID<1)
            {
                throw new Exception("公司不存在");
            }
            var sql = @"UPDATE [dbo].[T_QMDS_CompanyTemplateRelation]
   SET 
      FValid=0
 WHERE CompanyID=@companyID";
            return dapperPlusDB.Execute(sql,new { CompanyID=companyID})>0;
        }

        /// <summary>
        /// 修改公司模板关系
        /// </summary>
        /// <param name="tQMDSCompanyTemplateRelation"></param>
        /// <returns></returns>
        public bool ModifyCompanyTemplateRelation(TQMDSCompanyTemplateRelation tQMDSCompanyTemplateRelation)
        {
            if (tQMDSCompanyTemplateRelation==null)
            {
                throw new Exception("公司模板关系不能为空");
            }
            var sql = @"UPDATE [dbo].[T_QMDS_CompanyTemplateRelation]
   SET 
      ,[TemplateID] = @TemplateID  
      ,[ModifyTime] =GETDATE()
      ,[ModifierID] = @ModifierID      
 WHERE CompanyID=@companyID";
            return dapperPlusDB.Execute(sql,tQMDSCompanyTemplateRelation)>0;
        }

        /// <summary>
        /// 通过公司ID获取公司模板信息
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <returns></returns>
        public dynamic QueryQMDSTemplateRelationByCompanyID(int companyID)
        {
            if (companyID<1)
            {
                throw new Exception("公司未注册");
            }
            var sql = @"SELECT  
        c.CompanyName AS 公司名称 ,
        b.DS1 AS 打赏金额1 ,
        b.DS2 AS 打赏金额2 ,
        b.DS3 AS 打赏金额3 ,
        b.DS4 AS 打赏金额4 ,
        a.CreateTime AS 创建时间
FROM    dbo.T_QMDS_CompanyTemplateRelation a
        LEFT JOIN dbo.T_QMDS_Template b ON a.TemplateID = b.ID
                                           AND a.FValid = 1
                                           AND b.FValid = 1
        LEFT JOIN dbo.T_QMDS_Company c ON a.CompanyID = c.ID
                                          AND c.Fvalid = 1
WHERE   a.CompanyID = @companyID";
            return dapperPlusDB.Query<dynamic>(sql);
        }


    }
}
