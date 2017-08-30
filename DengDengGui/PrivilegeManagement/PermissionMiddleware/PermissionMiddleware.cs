

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrivilegeManagement.Middleware
{
    /// <summary>
    /// 权限中间件
    /// </summary>
    public class PermissionMiddleware
    {
        /// <summary>
        /// 管道代理对象
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 权限中间件的配置选项
        /// </summary>
        private readonly PermissionMiddlewareOption _option;

        /// <summary>
        /// 用户权限集合
        /// </summary>
        internal static List<dynamic> _userPermissions;

        /// <summary>
        /// 权限中间件构造
        /// </summary>
        /// <param name="next">管道代理对象</param>
        /// <param name="permissionResitory">权限仓储对象</param>
        /// <param name="option">权限中间件配置选项</param>
        public PermissionMiddleware(RequestDelegate next, PermissionMiddlewareOption option)
        {
            _option = option;
            _next = next;
            LoadUserPermissions();
        }
        /// <summary>
        /// 获取用户权限
        /// </summary>
        void LoadUserPermissions()
        {
            if (_userPermissions == null)
            {
                _userPermissions = new List<dynamic>();

            }
        }
        /// <summary>
        /// 调用管道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            var questUrl = context.Request.Path.Value;
            var userName = context.User.Identity.Name;
            var isAuthenticated = context.User.Identity.IsAuthenticated;

            ////无权限跳转到拒绝页面
            //context.Response.Redirect(_option.NoPermissionAction);

            return this._next(context);
        }

    }
}
