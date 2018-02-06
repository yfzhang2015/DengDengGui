using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;


namespace QuanMinDaShangPlatform.Models.Repository
{
    public class DsInfoRepository : IDsInfoRepository
    {
        public IDapperPlusDB dapperPlusDB;
        public DsInfoRepository(IDapperPlusDB dapperPlusDB)
        {
            this.dapperPlusDB = dapperPlusDB;
        }
        /// <summary>
        /// 添加打赏信息
        /// </summary>
        /// <param name="tQMDSDsInfo">打赏信息</param>
        /// <returns></returns>
        public bool AddDsInfoRepository(TQMDSDsInfo tQMDSDsInfo)
        {
            if (tQMDSDsInfo==null)
            {
                throw new Exception("添加的打赏信息不能为空");
            }
            var dynamicParameters = new DynamicParameters();
            var sql = "Proc_SaveDsInfo";
            dynamicParameters.Add("@EmployeeID", tQMDSDsInfo.EmployeeID);
            dynamicParameters.Add("@DsMoney", tQMDSDsInfo.DsMoney);
            dynamicParameters.Add("@DsOpenID", tQMDSDsInfo.DsOpenID);
            dynamicParameters.Add("@TransanctionID", tQMDSDsInfo.TransanctionID);
            dynamicParameters.Add("@OrderNo", tQMDSDsInfo.TransanctionID);
            return dapperPlusDB.Execute(sql,dynamicParameters,null,null, System.Data.CommandType.StoredProcedure) >0;
        }

      
        /// <summary>
        /// 修改打赏信息
        /// </summary>
        /// <returns></returns>
        public bool ModifyDsInfoRepository(string orderNo)
        {
            var sql = @"UPDATE [dbo].[T_QMDS_DSINFO]
   SET 
      [DsStatus] = 1
    WHERE OrderNo=@orderNo";
            return dapperPlusDB.Execute(sql, new { OrderNo = orderNo }) > 0;
        }

        /// <summary>
        /// 通过定单号查询订单信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        public  IEnumerable<TQMDSDsInfo> QueryDsInfoRepository(string orderNo)
        {
            var sql = @"SELECT [ID]
      ,[EmployeeID]
      ,[OrderNo]
      ,[DsMoney]
      ,[DsOpenID]
      ,[DsStatus]
      ,[DsTime]
      ,[TransanctionID]
  FROM [dbo].[T_QMDS_DSINFO]
  WHERE OrderNo=@orderNo";
            return dapperPlusDB.Query<TQMDSDsInfo>(sql,new { OrderNo=orderNo});
        }

        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="openID">openID</param>
        /// <returns></returns>
        public IEnumerable<T_QMDS_Employee> QueryEmployeeByOpenID(string openID)
        {
            var sql = @"SELECT [ID]
      ,[GCID]
      ,[CompanyID]
      ,[OpenID]
      ,[ENumber]
      ,[EName]
      ,[Nickname]
      ,[Phone]
      ,[Address]
      ,[NativePlace]
      ,[Message]
      ,[Picture]
      ,[Count]
      ,[CreaterID]
      ,[CreateTime]
      ,[ModifierID]
      ,[ModifyTime]
      ,[Fvalid]
  FROM [dbo].[T_QMDS_Employee]
  WHERE OpenID=@openid AND Fvalid=1";
            return dapperPlusDB.Query<T_QMDS_Employee>(sql,new { OpenID=openID});
        }

        /// <summary>
        /// 通过电话和密码查询员工是否存在
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <param name="passwor">密码</param>
        /// <returns></returns>
        public IEnumerable<T_QMDS_EmployeeUser> QueryEmployeeByPhoneAndPssword(string phone, string password)
        {
            if (string.IsNullOrEmpty(phone))
            {
                throw new Exception("手机号不能为空");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("密码不能为空");
            }
            var sql = @"SELECT [ID]
      ,[UserName]
      ,[PassWord]
      ,[EmployeeID]
      ,[Fvalid]
  FROM [dbo].[T_QMDS_EmployeeUser]
  WHERE UserName=@phone AND [PassWord]=@password AND Fvalid=1";
            var dynamicPara = new DynamicParameters();
            dynamicPara.Add("@phone",phone);
            dynamicPara.Add("@password",password);
            return dapperPlusDB.Query<T_QMDS_EmployeeUser>(sql,dynamicPara);

        }

        /// <summary>
        /// 绑定openID通过用户名
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="openID">openID</param>
        /// <returns></returns>
        public bool BindEmployeeOpenID(string openID,string employeeID)
        {
            if (string.IsNullOrEmpty(openID))
            {
                throw new Exception("openID不能为空");
            }
            if (string.IsNullOrEmpty(employeeID))
            {
                throw new Exception("用户ID不能为空");
            }
            var sql = @"UPDATE dbo.T_QMDS_Employee SET OpenID=@openID 
WHERE ID=@employeeID";
            return dapperPlusDB.Execute(sql,new { OpenID=openID,EmployeeID=employeeID})>0;
        }

        /// <summary>
        /// 员工通过自己的openid获取打赏信息
        /// </summary>
        /// <param name="index">分页索引值</param>
        /// <param name="openID">openid</param>
        /// <param name="stat">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="totalFee">打赏金额</param>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryEmployeeInfo(int index,string openID, string stat, string end,out decimal totalFee)
        {
    
            var sql = @"Proc_EmployeeInfo";
            var dyanmicPara = new DynamicParameters();
            dyanmicPara.Add("@index", index);
            dyanmicPara.Add("@openid", openID);
            dyanmicPara.Add("@start",stat);
            dyanmicPara.Add("@end", end);
            dyanmicPara.Add("@totalFee",dbType: System.Data.DbType.Double, direction: System.Data.ParameterDirection.Output);
            var result=dapperPlusDB.Query<dynamic>(sql,dyanmicPara,null,true,null,System.Data.CommandType.StoredProcedure);
            totalFee =Convert.ToDecimal(dyanmicPara.Get<double>("@totalFee"));
            return result;
        }

        /// <summary>
        /// 后台通过员工编号查询打赏明细
        /// </summary>
        /// <param name="index">分页索引值</param>
        /// <param name="employeeid">人员ID</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="totalFee">总金额</param>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryEmployeeInfoByEmployeeID(int index, string employeeid, string start, string end, out decimal totalFee)
        {
            var sql = "Proc_EmployeeInfoByEmployeeID";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@index",index);
            dynamicParameter.Add("@employeeid",employeeid);
            dynamicParameter.Add("@start", start);
            dynamicParameter.Add("@end",end);
            dynamicParameter.Add("@totalFee", 0, dbType: System.Data.DbType.Double, direction: System.Data.ParameterDirection.Output);
            var result= dapperPlusDB.Query<dynamic>(sql,dynamicParameter,null,true,null,System.Data.CommandType.StoredProcedure);
            totalFee =Convert.ToDecimal(dynamicParameter.Get<double>("@totalFee"));
            return result;
        }

        /// <summary>
        /// 后台通过公司查询没个人的打赏明细
        /// </summary>
        /// <param name="index">分页索引值</param>
        /// <param name="employeeid">员工ID</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="totalFee">金额</param>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryCompanyEmployeeDsInfoByCompanyID(int index, string companyID, string start, string end, out decimal totalFee)
        {
            var sql = "Proc_CompanyEmployeeID";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@index", index);
            dynamicParameter.Add("@companyID", companyID);
            dynamicParameter.Add("@start", start);
            dynamicParameter.Add("@end", end);
            dynamicParameter.Add("@totalFee", 0, dbType: System.Data.DbType.Double, direction: System.Data.ParameterDirection.Output);
            var result = dapperPlusDB.Query<dynamic>(sql, dynamicParameter, null, true, null, System.Data.CommandType.StoredProcedure);
            totalFee = Convert.ToDecimal(dynamicParameter.Get<double>("@totalFee"));
            return result;
        }
    }
}
