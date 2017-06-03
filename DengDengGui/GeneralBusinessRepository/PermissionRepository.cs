using GeneralBusinessData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GeneralBusinessRepository.SqlServer
{
    /// <summary>
    /// 权限仓储处理类（For MS SqlServer)
    /// </summary>
    public class PermissionRepository : IPermissionRepository
    {
        /// <summary>
        /// 数据库操作类
        /// </summary>
        ISqlHelper _sqlHelper;
        /// <summary>
        /// 实例化sqlserver的仓储对象
        /// </summary>
        /// <param name="sqlHelper"></param>
        public PermissionRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #region 用户管理
        /// <summary>
        /// 查询全部用户
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetUsers()
        {
            var sql = "select * from users";
            return _sqlHelper.QueryList(sql);
        }

        /// <summary>
        /// 按用户名或名称查询用户
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetUsers(string queryName)
        {
            var sql = "select * from users where username like @queryname or name like @queryname";
            var queryNameParameter = new SqlParameter() { Value = "%" + queryName + "%", ParameterName = "@queryname" };
            return _sqlHelper.QueryList(sql,queryNameParameter);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public int AddUser(string userName, string password, string name,int companyID)
        {
            var sql = $@"insert into users(username,password,name,companyid) values(@username,@password,@name,@companyid)";
            var userNameParameter = new SqlParameter() { Value = userName, ParameterName = "@username" };
            var passwordParameter = new SqlParameter() { Value = password, ParameterName = "@password" };
            var nameParameter = new SqlParameter() { Value = name, ParameterName = "@name" };
            var companyIDParameter = new SqlParameter() { Value = companyID, ParameterName = "@companyID" };
            return _sqlHelper.ChangeData(sql, userNameParameter, passwordParameter, nameParameter, companyIDParameter);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public int ModifyUser(int id, string userName, string password, string name)
        {
            var sql = "update  users set [username]=@username,[password] =@password,[name]=@name where id=@id";
            var userNameParameter = new SqlParameter() { Value = userName, ParameterName = "@username" };
            var passwordParameter = new SqlParameter() { Value = password, ParameterName = "@password" };
            var idParameter = new SqlParameter() { Value = id, ParameterName = "@id" };
            var nameParameter = new SqlParameter() { Value = name, ParameterName = "@name" };
            return _sqlHelper.ChangeData(sql, userNameParameter, passwordParameter, idParameter, nameParameter);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public int RemoveUser(int id)
        {
            var sql = "delete  users  where id=@id";
            var idParameter = new SqlParameter() { Value = id, ParameterName = "@id" };
            return _sqlHelper.ChangeData(sql, idParameter);
        }
        /// <summary>
        /// 登录，并返回该用户合部信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Dictionary<string, dynamic> Login(string userName, string password)
        {
            var sql = "select * from users where username=@username and password=@password";
            var userNameParameter = new SqlParameter() { Value = userName, ParameterName = "@username" };
            var passwordParameter = new SqlParameter() { Value = password, ParameterName = "@password" };
            var result = _sqlHelper.QueryList(sql, userNameParameter, passwordParameter);
            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public bool ModifyPassword(string userName, string newPassword)
        {
            var sql = "update  users set password=@newpassword where username=@username ";
            var userNameParameter = new SqlParameter() { Value = userName, ParameterName = "@username" };
            var newPasswordParameter = new SqlParameter() { Value = newPassword, ParameterName = "@newpassword" };
            var result = _sqlHelper.ChangeData(sql, userNameParameter, newPasswordParameter);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 查用户名查询权限中的action
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetPermissionByUserID(string username)
        {
            var sql = $@"SELECT  [Permissions].[Action]
FROM    dbo.Users
        JOIN dbo.UserRoles ON Users.ID = UserRoles.UserID
        JOIN dbo.Roles ON Roles.ID = UserRoles.RoleID
        JOIN RolePermissions ON Roles.ID = RolePermissions.RoleID
        JOIN [Permissions] ON [Permissions].ID = RolePermissions.PermissionID
WHERE   Users.UserName = @username";
            var userNameParameter = new SqlParameter() { Value = username, ParameterName = "@username" };
            return _sqlHelper.QueryList(sql, userNameParameter);
        }
        /// <summary>
        /// 获取全部用户和全部权限
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetUserPermissions()
        {
            var sql = @"SELECT  [Permissions].[Action],[Users].[UserName],[Users].[ID] AS UserID
FROM    dbo.Users
        JOIN dbo.UserRoles ON Users.ID = UserRoles.UserID
        JOIN dbo.Roles ON Roles.ID = UserRoles.RoleID
        JOIN RolePermissions ON Roles.ID = RolePermissions.RoleID
        JOIN [Permissions] ON [Permissions].ID = RolePermissions.PermissionID";
            return _sqlHelper.QueryList(sql);
        }

        #endregion

        #region 角色管理
        /// <summary>
        /// 查询全部角色
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetRoles()
        {
            var sql = "select id,name from roles";
            return _sqlHelper.QueryList(sql);
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public int AddRole(string roleName,int companyID)
        {
            var sql = $@"insert into roles(name,cpmpanyid) values(@name,@cpmpanyid)";
            var roleNameParameter = new SqlParameter() { Value = roleName, ParameterName = "@name" };
            var companyIDParameter = new SqlParameter() { Value = companyID, ParameterName = "@companyID" };
            return _sqlHelper.ChangeData(sql, roleNameParameter, companyIDParameter);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public int ModifyRole(int id, string roleName)
        {
            var sql = $@"update roles set name=@name where id=@id";
            var nameParameter = new SqlParameter() { Value = roleName, ParameterName = "@name" };
            var idParameter = new SqlParameter() { Value = id, ParameterName = "@id" };
            return _sqlHelper.ChangeData(sql, nameParameter, idParameter);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public int RemoveRole(int id)
        {
            var sql = $@"delete roles where id=@id";
            var idParameter = new SqlParameter() { Value = id, ParameterName = "@id" };
            return _sqlHelper.ChangeData(sql, idParameter);
        }
        #endregion

        #region 权限管理
        /// <summary>
        /// 查询全部权限
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetPermissions()
        {
            var sql = "select * from permissions";
            return _sqlHelper.QueryList(sql);
        }
        /// <summary>
        /// 移除权限
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public int RemovePermission(int id)
        {
            var sql = $@"delete permissions where id=@id";
            var idParameter = new SqlParameter() { Value = id, ParameterName = "@id" };
            return _sqlHelper.ChangeData(sql, idParameter);
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="actiondescription">action描述</param>
        /// <param name="controllername">controller</param>
        /// <param name="predicate">谓词</param>
        /// <returns></returns>
        public int AddPermission(string action, string actiondescription, string controllername, string predicate, int companyID)
        {
            var sql = $@"insert into permissions(action,actiondescription,controllername,predicate,companyid) values(@action,@actiondescription,@controllername,@predicate,@companyid)";
            var actionParameter = new SqlParameter() { Value = action, ParameterName = "@action" };
            var actionDescriptionParameter = new SqlParameter() { ParameterName = "@actiondescription" };
            if (string.IsNullOrEmpty(actiondescription))
            {
                actionDescriptionParameter.Value = "";
            }
            else
            {
                actionDescriptionParameter.Value = actiondescription;
            }
            var controllerNameParameter = new SqlParameter() { Value = controllername, ParameterName = "@controllername" };
            var predicateParameter = new SqlParameter() { Value = predicate, ParameterName = "@predicate" };
            var companyIDParameter = new SqlParameter() { Value = companyID, ParameterName = "@companyID" };
            return _sqlHelper.ChangeData(sql, actionParameter, actionDescriptionParameter, controllerNameParameter, predicateParameter, companyIDParameter);
        }
        #endregion

        #region 角色权限管理
        /// <summary>
        /// 按角色ID查询权限
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetPermissionsByRoleID(int roleID)
        {
            var sql = "select * from rolepermissions where roleid=@roleid";
            var roleIDParameter = new SqlParameter() { Value = roleID, ParameterName = "@roleid" };
            return _sqlHelper.QueryList(sql, roleIDParameter);
        }
        /// <summary>
        /// 批量保存角色权限
        /// </summary>
        /// <param name="rolePermissons">角色权限</param>
        /// <returns></returns>
        public bool SavaRolePermissions(int roleID, List<dynamic> rolePermissons)
        {
            var sqlOperations = new List<SqlOperation>();
            //删除原来的
            sqlOperations.Add(new SqlOperation()
            {
                Sql = "delete rolepermissions where roleid=@roleid",
                parmeters = new System.Data.Common.DbParameter[]{
                    new SqlParameter(){ ParameterName="@roleid", Value=roleID}}
            });

            //添加新的
            foreach (var rolePermisson in rolePermissons)
            {
                sqlOperations.Add(new SqlOperation()
                {
                    Sql = "insert into rolepermissions(roleid,permissionid) values(@roleid,@permissionid)",
                    parmeters = new System.Data.Common.DbParameter[]{
                    new SqlParameter(){ ParameterName="@roleid", Value=rolePermisson.roleid},
                    new SqlParameter(){ ParameterName="@permissionid", Value=rolePermisson.permissionid}}
                });
            }
            return _sqlHelper.ExecuteTransactionSql(sqlOperations);
        }
        #endregion
        #region 用户角色管理
        /// <summary>
        /// 按用户ID查询角色
        /// </summary>
        /// <param name="roleID">用户ID</param>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> GetRoleByUserID(int userID)
        {
            var sql = "select * from userroles where userid=@userid";
            var userIDParameter = new SqlParameter() { Value = userID, ParameterName = "@userid" };
            return _sqlHelper.QueryList(sql, userIDParameter);
        }
        /// <summary>
        /// 批量保存用户角色
        /// </summary>
        /// <param name="rolePermissons">用户角色</param>
        /// <returns></returns>
        public bool SavaUserRoles(int userID, List<dynamic> userRoles)
        {
            var sqlOperations = new List<SqlOperation>();
            //删除原来的
            sqlOperations.Add(new SqlOperation()
            {
                Sql = "delete userroles where userid=@userID",
                parmeters = new System.Data.Common.DbParameter[]{
                    new SqlParameter(){ ParameterName="@userID", Value=userID}}
            });

            //添加新的
            foreach (var userRole in userRoles)
            {
                sqlOperations.Add(new SqlOperation()
                {
                    Sql = "insert into userroles(userid,roleid) values(@userid,@roleid)",
                    parmeters = new System.Data.Common.DbParameter[]{
                    new SqlParameter(){ ParameterName="@roleid", Value=userRole.roleid},
                    new SqlParameter(){ ParameterName="@userid", Value=userRole.userid}}
                });
            }
            return _sqlHelper.ExecuteTransactionSql(sqlOperations);
        }
     
        #endregion
    }
}
