using System;
using System.Collections.Generic;

namespace GeneralBusinessRepository
{
    /// <summary>
    /// 通用业务平台仓储处理接口
    /// </summary>
    public interface IBusinessRepository
    {


        #region 用户管理
        /// <summary>
        /// 查询全部用户
        /// </summary>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> GetUsers();
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        int AddUser(string userName, string password);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        int ModifyUser(int id, string userName, string password);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        int RemoveUser(int id);
        /// <summary>
        /// 登录，并返回该用户合部信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Dictionary<string, dynamic> Login(string userName, string password);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        bool ModifyPassword(string userName, string newPassword);

        /// <summary>
        /// 获取全部用户和全部权限
        /// </summary>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> GetUserPermissions();
        /// <summary>
        /// 查用户名查询权限中的action
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> GetPermissionByUserID(string username);
        #endregion

        #region 角色管理
        /// <summary>
        /// 查询全部角色
        /// </summary>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> GetRoles();
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        int AddRole(string roleName);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        int ModifyRole(int id, string roleName);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        int RemoveRole(int id);

  


        #endregion

        #region 菜单管理
        /// <summary>
        /// 按用户名查询菜单
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> GetMenusByUserName(string userName);



        #endregion
    }
}
