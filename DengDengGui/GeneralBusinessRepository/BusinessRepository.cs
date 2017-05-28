using GeneralBusinessData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GeneralBusinessRepository.SqlServer
{
    /// <summary>
    /// 通用业务平台仓储处理类（For MS SqlServer)
    /// </summary>
    public class BusinessRepository : IBusinessRepository
    {
        /// <summary>
        /// 数据库操作类
        /// </summary>
        ISqlHelper _sqlHelper;
        /// <summary>
        /// 实例化sqlserver的仓储对象
        /// </summary>
        /// <param name="sqlHelper"></param>
        public BusinessRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        } 

        #region 菜单管理
        /// <summary>
        /// 按用户名查询菜单
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetMenusByUserName(string userName)
        {
            var sql = $@"SELECT rolemodules.menuid ,menus.name AS menuname,rolemodules.moduleid,modules.name AS modulename
FROM    users
        JOIN userroles ON users.id = userroles.userid
        JOIN roles ON userroles.roleid = roles.id
        JOIN rolemodules ON roles.id = rolemodules.roleid
		JOIN dbo.Menus ON Menus.id=rolemodules.menuid
        JOIN ( SELECT   id ,
                        name ,
                        menuid
               FROM     billmodules
               UNION ALL
               SELECT   id ,
                        name ,
                        menuid
               FROM     QueryModules
               UNION ALL
               SELECT   id ,
                        name ,
                        menuid
               FROM     ChartModules
             ) AS modules ON modules.menuid = rolemodules.menuid
                             AND modules.id = rolemodules.moduleid
							 WHERE users.username=@username";
            var userNameParameter = new SqlParameter() { Value = userName, ParameterName = "@username" };
            return _sqlHelper.QueryList(sql, userNameParameter);

        }



        #endregion


    }
}
