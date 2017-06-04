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
        /// 查询全部菜单
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetMenus()
        {
            var sql = "select id,name from menus";
            return _sqlHelper.QueryList(sql);
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public int AddMenu(string name, int companyID)
        {
            var sql = "insert into menus(name) values(@name)";
            var nameParameter = new SqlParameter() { Value = name, ParameterName = "@name" };
            var companyIDParameter = new SqlParameter() { Value = companyID, ParameterName = "@companyID" };
            return _sqlHelper.ChangeData(sql, nameParameter, companyIDParameter);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public int ModifyMenu(int id, string name)
        {
            var sql = "update menus set name=@name where id=@id";
            var nameParameter = new SqlParameter() { Value = name, ParameterName = "@name" };
            var idParameter = new SqlParameter() { Value = id, ParameterName = "@id" };
            return _sqlHelper.ChangeData(sql, nameParameter, idParameter);
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public int RemoveMenu(int id)
        {
            var sql ="delete menus where id=@id";
            var idParameter = new SqlParameter() { Value = id, ParameterName = "@id" };
            return _sqlHelper.ChangeData(sql, idParameter);
        }
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

  

        #region 查询模块管理
        public List<Dictionary<string, dynamic>> GetQueryModules()
        {
            var sql = "select * from querymodules";
            return _sqlHelper.QueryList(sql);
        }
        #endregion
        #region 图表模块管理
        public List<Dictionary<string, dynamic>> GetChartModules()
        {
            var sql = "select * from chartmodules";
            return _sqlHelper.QueryList(sql);
        }
        #endregion
    }
}
