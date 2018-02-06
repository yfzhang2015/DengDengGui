using Dapper;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Repository
{
    public class CommercialRepository : ICommercialRepository
    {
        //注入dapper
        private IDapperPlusDB _dapperPlusDB;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="dapperPlusDB"></param>
        public CommercialRepository(IDapperPlusDB dapperPlusDB)
        {
            _dapperPlusDB = dapperPlusDB;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool ModifyPassword(string username, string password)
        {
            var sql = "UPDATE T_QMDS_User SET PassWord=@PassWord WHERE UserName=@UserName";
            return _dapperPlusDB.Execute(sql, new { username, password }) > 0;
        }

        /// <summary>
        /// 商户后台登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public List<dynamic> Login(string username, string password)
        {
            var sql = @"SELECT   UserName ,CompanyID,CompanyName
FROM    dbo.T_QMDS_User
        JOIN dbo.T_QMDS_Company ON dbo.T_QMDS_User.CompanyID = T_QMDS_Company.ID
WHERE   UserName = @username
        AND PassWord =@password
        AND T_QMDS_Company.Fvalid = 1";
            var count = _dapperPlusDB.Query<dynamic>(sql, new { username, password }).ToList();
            return count;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="company"></param>
        /// <param name="groupCompany"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Register(T_QMDS_Company company, T_QMDS_GroupCompany groupCompany, string username, string password)
        {
            #region sql语句

            var sql1 = @" INSERT  INTO dbo.T_QMDS_GroupCompany
            (ID, GCName, CreaterID, CreateTime )
    VALUES  (@ID, @GCName, @CreaterID, GETDATE() )";
            var sql2 = @"INSERT INTO dbo.T_QMDS_Company
        ( ID,
          GCID,
          CompanyName ,
          Phone ,
          WeiXinPay ,
          AliPay ,
          License ,
          CreaterID ,
          CreateTime
        )
VALUES  ( @ID,
          @GCID,
          @CompanyName ,
          @Phone ,
          @WeiXinPay ,
          @AliPay ,
          @License ,
          @CreaterID ,
          GETDATE()
        )";
            var sql3 = @" INSERT  INTO dbo.T_QMDS_User
            ( CompanyID, UserName, [PassWord] )
    VALUES  ( @CompanyID, @UserName, @PassWord )";

            #endregion

            IDbTransaction transaction = null;
            IDbConnection conn = null;
           
            try
            {
                conn = _dapperPlusDB.GetConnection();
                conn.Open();
                transaction = conn.BeginTransaction();
                _dapperPlusDB.Execute(sql2, company, transaction);
                _dapperPlusDB.Execute(sql3, new { CompanyID = company.ID, UserName = username, PassWord = password }, transaction);
                if (groupCompany.GCName != null)
                {
                    _dapperPlusDB.Execute(sql1, groupCompany, transaction);
                }
                transaction.Commit();
                return true;
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw new Exception(exc.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
      
        /// <summary>
        /// 确保用户名的唯一性
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUser(string username)
        {
            var sql = "SELECT COUNT(*) FROM dbo.T_QMDS_User WHERE UserName=@UserName AND Fvalid=1";
            var count = _dapperPlusDB.ExecuteScalar<int>(sql, new { username });
            return count > 0;
        }
        /// <summary>
        /// 确保手机号的唯一性
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool CheckPhone(string phone)
        {
            var sql = "SELECT COUNT(*) as sl FROM dbo.T_QMDS_Employee WHERE Phone=@phone AND Fvalid=1";
            var count = _dapperPlusDB.ExecuteScalar<int>(sql, new { phone });
            return count > 0;
        }
        /// <summary>
        /// 生成员工编号
        /// </summary>
        /// <returns></returns>
        public string YGNumber()
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@tradeNumber", "YG00000001");
            dynamicParameters.Add("@newTradeNumber", dbType: DbType.String, direction: ParameterDirection.Output, size: 11);
            var sql = "GetTradeNumber";
            var ss = _dapperPlusDB.Execute(sql, dynamicParameters, null, null, System.Data.CommandType.StoredProcedure);
            return dynamicParameters.Get<string>("@newTradeNumber");
        }

        /// <summary>
        /// 生成集团公司人员ID
        /// </summary>
        /// <param name="lxnum">ID类型：1集团，2公司，3人员，4单据号</param>
        /// <returns></returns>
        public string  ProduceID(int lxnum)
        {
            var lx = "gc";
            switch (lxnum)
            {
                case 1:
                    lx = "gc";//集团
                    break;
                case 2:
                    lx = "gs";//公司
                    break;
                case 3:
                    lx = "ry";//人员
                    break;
                case 4:
                    lx = "dj";//单据号
                    break;
            }
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@lx", lx);
            dynamicParameters.Add("@lsh", dbType: DbType.String, direction: ParameterDirection.Output, size: 30);
            var sql = "getlsh";
            var ss = _dapperPlusDB.Execute(sql, dynamicParameters, null, null, System.Data.CommandType.StoredProcedure);
            var str= dynamicParameters.Get<string>("@lsh");
            return str;
        }
        #region 人员
        /// <summary>
        /// 人员总金额
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetEmployeeDsMoneyID(string id)
        {
            var sql = "SELECT SUM(DsMoney) FROM [T_QMDS_DSINFO] WHERE EmployeeID=@EmployeeID";
            var str = _dapperPlusDB.ExecuteScalar<dynamic>(sql, new { EmployeeID=id });

            return str;
        }
        
        /// <summary>
        /// 添加人员并设置密码
        /// </summary>
        /// <param name="employee">人员</param>
        /// <returns></returns>
        public bool AddEmployee(T_QMDS_Employee employee, string password)
        {
            #region sql语句

            var sql = @"INSERT INTO dbo.T_QMDS_Employee
        ( ID,
          CompanyID ,
          ENumber ,
          EName ,
          Nickname ,
          Phone ,
          [Address] ,
          NativePlace ,
          [Message] ,
          Picture ,
          CreaterID ,
          CreateTime 
        )
VALUES  ( @ID,
          @CompanyID ,
          @ENumber ,
          @EName ,
          @Nickname ,
          @Phone ,
          @Address ,
          @NativePlace ,
          @Message ,
          @Picture ,
          @CreaterID ,
          GETDATE() 
        )";
            var sql1 = @"INSERT  INTO dbo.T_QMDS_EmployeeUser
        ( UserName ,
          PassWord ,
          EmployeeID 
        )
VALUES  ( @Phone , 
          @PassWord , 
          @EmployeeID 
        )";
            #endregion

            IDbTransaction transaction = null;
            IDbConnection conn = null;

            try
            {
                conn = _dapperPlusDB.GetConnection();
                conn.Open();
                transaction = conn.BeginTransaction();
                _dapperPlusDB.Execute(sql, employee, transaction);
                _dapperPlusDB.Execute(sql1, new { Phone = employee.Phone, PassWord = password, EmployeeID = employee.ID }, transaction);
                
                transaction.Commit();
                return true;
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw new Exception(exc.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// 人员ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEmployee(string id)
        {
            var sql = @"UPDATE dbo.T_QMDS_Employee SET Fvalid=0 WHERE ID=@ID";
            return _dapperPlusDB.Execute(sql, new { ID = id }) > 0;
        }
        /// <summary>
        /// 修改人员
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool ModifyEmployee(T_QMDS_Employee employee)
        {
            var sql = @"UPDATE  T_QMDS_Employee
SET     [EName] = @EName ,
        Nickname = @Nickname ,
        Phone = @Phone ,
        [Address] = @Address ,
        NativePlace = @NativePlace ,
        [Message] = @Message ,
        Picture = @Picture ,
        ModifierID = @ModifierID ,
        ModifyTime = GETDATE()
		WHERE ID=@ID";
            return _dapperPlusDB.Execute(sql, employee) > 0;
        }
        /// <summary>
        /// 查询人员
        /// </summary>
        /// <param name="id">人员ID</param>
        /// <returns></returns>
        public List<T_QMDS_Employee> GetEmployeeByID(string id)
        {
            var sql = @"SELECT [ID]
      ,[GCID]
      ,[CompanyID]
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
  FROM [T_QMDS_Employee] where ID=@ID";
            return _dapperPlusDB.Query<T_QMDS_Employee>(sql, new { ID = id }).ToList();

        }
        /// <summary>
        /// 查询公司下的全部员工
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageRow">行数</param>
        /// <returns></returns>
        public List<T_QMDS_Employee> GetEmployees(string companyID, int pageIndex, int pageRow)
        {
            var sql = $@"SELECT * FROM(
  SELECT 
	  ROW_NUMBER()  OVER(ORDER BY  ID) as RowNum
      ,[ID]
      ,[GCID]
      ,[CompanyID]
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
  FROM T_QMDS_Employee WHERE Fvalid=1
  ) a 
where CompanyID=@CompanyID AND RowNum > ({pageIndex}-1)*{pageRow} AND RowNum <({pageIndex})*{pageRow}+1";
            return _dapperPlusDB.Query<T_QMDS_Employee>(sql, new { CompanyID = companyID }).ToList();
        }
        /// <summary>
        /// 查询人员总数
        /// </summary>
        /// <param name="pageRow">当前页的行数</param>
        /// <returns></returns>
        public string QueryEmployeesCount(int pageRow)
        {
            var sql = $"SELECT CEILING(CONVERT(DECIMAL(14,1),COUNT(1))/{pageRow})AS 个数  from T_QMDS_Employee WHERE Fvalid =1";
            return _dapperPlusDB.Query<string>(sql).First();
        }
        #endregion

        #region 公司
        /// <summary>
        /// 查询公司
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T_QMDS_Company GetCompanByID(string id)
        {
            var sql= @"SELECT  [ID]
      ,[GCID]
      ,[CompanyName]
      ,[Phone]
      ,[WeiXinPay]
      ,[AliPay]
      ,[License]
      ,[CreaterID]
      ,[CreateTime]
      ,[ModifierID]
      ,[ModifyTime]
      ,[Fvalid]
  FROM [T_QMDS_Company] WHERE ID=@ID";
            return _dapperPlusDB.Query<T_QMDS_Company>(sql,new { id}).ToList()[0];
        }
        /// <summary>
        /// 查询公司(注册)
        /// </summary>
        /// <returns></returns>
        public List<T_QMDS_Company> GetCompanies()
        {
            var sql = @"SELECT  [ID]
      ,[GCID]
      ,[CompanyName]
      ,[Phone]
      ,[WeiXinPay]
      ,[AliPay]
      ,[License]
      ,[CreaterID]
      ,[CreateTime]
      ,[ModifierID]
      ,[ModifyTime]
      ,[Fvalid]
  FROM [T_QMDS_Company]";
            return _dapperPlusDB.Query<T_QMDS_Company>(sql).ToList();
        }
        /// <summary>
        /// 删除公司
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCompany(string id)
        {
            var sql = @" UPDATE T_QMDS_Company SET Fvalid=0 AND id=@ID ";
            return _dapperPlusDB.Execute(sql, new { ID = id }) > 0;
        }
        /// <summary>
        /// 修改公司
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public bool ModifyCompany(T_QMDS_Company company)
        {
            var sql = @"UPDATE  T_QMDS_Company
SET     CompanyName = @CompanyName ,
        Phone = @Phone ,
        WeiXinPay = @WeiXinPay ,
        AliPay = @AliPay ,
        License = @License ,
        ModifierID = @ModifierID ,
        ModifyTime = GETDATE()
		WHERE ID=@ID";
            return _dapperPlusDB.Execute(sql, company) > 0;
        }
        #endregion

        #region 集团
        /// <summary>
        /// 添加集团
        /// </summary>
        /// <param name="GroupCompany">集团</param>
        /// <returns></returns>
        public bool AddGroupCompany(T_QMDS_GroupCompany GroupCompany)
        {
            var sql = @"INSERT INTO dbo.T_QMDS_GroupCompany
        ( GCName ,
          CreaterID ,
          CreateTime 
        
        )
VALUES  ( @GCName ,
          @CreaterID ,
          GETDATE() 
        )";
            return _dapperPlusDB.Execute(sql, GroupCompany) > 0;
        }


        /// <summary>
        ///查询集团
        /// </summary>
        /// <returns></returns>
        public List<T_QMDS_GroupCompany> GetGroupCompanies()
        {
            var sql = @"SELECT [ID]
      ,[GCName]
      ,[CreaterID]
      ,[CreateTime]
      ,[ModifierID]
      ,[ModifyTime]
      ,[Fvalid]
  FROM [T_QMDS_GroupCompany]";
            return _dapperPlusDB.Query<T_QMDS_GroupCompany>(sql).ToList();
        }
       
        /// <summary>
        /// 移除集团
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteGroupCompany(string id)
        {
            var sql = @" UPDATE T_QMDS_GroupCompany SET Fvalid=0 WHERE ID=@ID";
            return _dapperPlusDB.Execute(sql, new { ID = id }) > 0;
        }
        /// <summary>
        /// 修改集团
        /// </summary>
        /// <param name="groupCompany"></param>
        /// <returns></returns>
        public bool ModifyGroupCompany(T_QMDS_GroupCompany groupCompany)
        {
            var sql = @"UPDATE  T_QMDS_GroupCompany
SET     GCName = @GCName ,
        ModifierID = @ModifierID ,
        ModifyTime = GETDATE()
		WHERE ID=@ID";
            return _dapperPlusDB.Execute(sql, groupCompany) > 0;

        }
        #endregion
    }
}
