using System;
using System.Collections.Generic;
using Dapper;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;

namespace QuanMinDaShangPlatform.Models.Repository
{
    public class EmployeeTemplateRelationRepository : IEmployeeTemplateRelationRepository
    {
        private IDapperPlusDB dapperPlusDB;
        public EmployeeTemplateRelationRepository(IDapperPlusDB dapperPlusDB)
        {
            this.dapperPlusDB = dapperPlusDB;
        }

        #region 添加人员模板
        /// <summary>
        /// 添加人员模板信息
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <param name="templateID">模板ID</param>
        /// <returns></returns>
        public bool AddEmployeeTemplateRelation(string companyID,int templateID)
        {
            if (string.IsNullOrEmpty(companyID)|| templateID < 1)
            {
                throw new Exception("人员模板信息不能为空");
            }
            var dynamicParameter= new DynamicParameters();
            dynamicParameter.Add("@componyID", companyID);
            dynamicParameter.Add("@templateID", templateID);
            var sql = "Proc_AddEmployeeRelation";
            return dapperPlusDB.Execute(sql,dynamicParameter,null,null,System.Data.CommandType.StoredProcedure)>0;
        }
        #endregion

        #region 删除人员模板信息
        /// <summary>
        /// 删除人员打赏模板信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool DeleteEmployeeTemplateRelation(int id)
        {
            if (id<1)
            {
                throw new Exception("人员打赏模板不存在");
            }
            var sql = @"UPDATE [dbo].[T_QMDS_EmployeeTemplateRelation]
   SET 
       FValid=0
 WHERE ID=@id";
            return dapperPlusDB.Execute(sql,new { ID=id})>0;
        }
        #endregion

        /// <summary>
        /// 删除原有人员模板添加新的模板
        /// </summary>
        /// <param name="compamyID">公司ID</param>
        /// <param name="templateID">模板ID</param>
        /// <returns></returns>
        public bool DeleteEmployeeTemplateRelationsByComponyID(string compamyID,int templateID)
        {
            if (string.IsNullOrEmpty(compamyID))
            {
                throw new Exception("公司ID不存在");
            }
            var sql = $@"BEGIN TRAN
BEGIN TRY
DELETE  dbo.T_QMDS_EmployeeTemplateRelation
WHERE   EmployeeID IN ( SELECT  id
                        FROM    dbo.T_QMDS_User
                      WHERE   CompanyID = @companyID )
INSERT INTO [dbo].[T_QMDS_EmployeeTemplateRelation]
           ([EmployeeID]
           ,[TemplateID]
           ,[DS1]
           ,[DS2]
           ,[DS3]
           ,[DS4]
           ,[CreateTime]
           ,[CreaterID]
           ,[FValid])
    SELECT a.ID,c.ID,c.DS1,c.ds2,c.ds3,c.DS4,GETDATE(),{templateID},1 FROM dbo.T_QMDS_User a LEFT JOIN dbo.T_QMDS_CompanyTemplateRelation b ON a.CompanyID=b.CompanyID
	AND a.Fvalid=1
	AND b.FValid=1
	LEFT JOIN dbo.T_QMDS_Template c ON b.TemplateID=c.ID
	WHERE a.ID=@companyID
END TRY
BEGIN CATCH
IF(@@TRANCOUNT>0)
ROLLBACK TRAN
END CATCH
IF(@@TRANCOUNT>0)
COMMIT TRAN ";
            return dapperPlusDB.Execute(sql, new { CompanyID = compamyID }) > 0;
        }

        /// <summary>
        /// 修改人员打赏模板信息
        /// </summary>
        /// <param name="tQMDSEmployeeTemplateRelation">人员打赏模板信息</param>
        /// <returns></returns>
        public bool ModifyEmployeeTemplateRelation(TQMDSEmployeeTemplateRelation tQMDSEmployeeTemplateRelation)
        {
            if (tQMDSEmployeeTemplateRelation==null)
            {
                throw new Exception("人员模板添加信息不能为空");
            }
            var sql = @"UPDATE [dbo].[T_QMDS_EmployeeTemplateRelation]
   SET [DSTemplateName] = @DSTemplateName
      ,[DSTemplateType] = @DSTemplateType
      ,[DS1] = @DS1
      ,[DS2] = @DS2
      ,[DS3] = @DS3
      ,[DS4] = @DS4
      ,[ModifyTime] = GETDATE()
      ,[ModifierID] = @ModifierID
 WHERE ID=@id";
            return dapperPlusDB.Execute(sql,tQMDSEmployeeTemplateRelation)>0;
        }

        /// <summary>
        /// 查询所有人员打赏模板信息通过公司ID
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryEmployeeTemplateRelation(string companyID)
        {
            var sql = @"SELECT  a.CompanyName AS 公司名称 ,
        b.EName AS 姓名 ,
        c.ds1 AS 打赏金额1 ,
        c.ds2 AS 打赏金额2 ,
        c.ds3 AS 打赏金额3 ,
        c.ds4 AS 打赏金额4, 
		DSTemplateName AS 打赏模板名称,
        DSTemplateType AS 打赏方式,
        c.ID
FROM    dbo.T_QMDS_Company a
        LEFT JOIN dbo.T_QMDS_Employee b ON a.id = b.CompanyID
                                       AND a.Fvalid = 1
                                       AND b.Fvalid = 1
        JOIN dbo.T_QMDS_EmployeeTemplateRelation c ON b.ID = c.EmployeeID
                                                           AND c.FValid = 1
WHERE   a.ID = @id";
            return dapperPlusDB.Query<dynamic>(sql,new { ID = companyID });
        }

        /// <summary>
        /// 查询人员设置信息
        /// </summary>
        /// <param name="employeeID">人员设置ID</param>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryEmployeeDsSetMessage(string employeeID)
        {
            if (string.IsNullOrEmpty(employeeID))
            {
                throw new Exception("人员不存在");
            }
            var sql = @"SELECT  d.CompanyName,
a.[Count], 
a.Picture,
a.EName, 
DSTemplateType,
b.DS1,
b.DS2,
b.DS3,
b.DS4,
a.[Message]
FROM    dbo.T_QMDS_Employee a
        LEFT JOIN dbo.T_QMDS_EmployeeTemplateRelation b ON a.ID = b.EmployeeID
                                                           AND a.Fvalid = 1
                                                           AND b.FValid = 1
		
		LEFT JOIN dbo.T_QMDS_Company d ON a.CompanyID=d.ID
		AND d.Fvalid=1
WHERE a.ID=@id
";
            return dapperPlusDB.Query<dynamic>(sql,new { ID=employeeID});
        }

        /// <summary>
        /// 查询人员模板信息
        /// </summary>
        /// <param name="id">人员模板信息</param>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryEmployeeDsSetByID(int id)
        {
            if (id<1)
            {
                throw new Exception("人员模板信息不存在");
            }
            var sql = @"SELECT  a.CompanyName AS 公司名称 ,
        b.EName AS 姓名 ,
        c.ds1 AS 打赏金额1 ,
        c.ds2 AS 打赏金额2 ,
        c.ds3 AS 打赏金额3 ,
        c.ds4 AS 打赏金额4, 
		DSTemplateName AS 打赏模板名称,
        DSTemplateType AS 打赏方式
FROM    dbo.T_QMDS_Company a
        LEFT JOIN dbo.T_QMDS_Employee b ON a.id = b.CompanyID
                                       AND a.Fvalid = 1
                                       AND b.Fvalid = 1
        LEFT JOIN dbo.T_QMDS_EmployeeTemplateRelation c ON b.ID = c.EmployeeID
                                                           AND c.FValid = 1		
WHERE c.ID=@id  ";
            return dapperPlusDB.Query<dynamic>(sql,new { ID=id});
        }

    }
}
