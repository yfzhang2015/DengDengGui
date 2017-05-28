
using GeneralBusinessRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GeneralBusinessSystem.Middleware
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
        /// 权限仓储对象
        /// </summary>
        IPermissionRepository _permissionRepository;
        /// <summary>
        /// 用户权限集合
        /// </summary>
        static List<dynamic> _userPermissions;

        /// <summary>
        /// 权限中间件构造
        /// </summary>
        /// <param name="next">管道代理对象</param>
        /// <param name="permissionResitory">权限仓储对象</param>
        /// <param name="option">权限中间件配置选项</param>
        public PermissionMiddleware(RequestDelegate next, IPermissionRepository permissionRepository, PermissionMiddlewareOption option)
        {
            _option = option;
            _permissionRepository = permissionRepository;
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
                foreach (var dic in _permissionRepository.GetUserPermissions())
                {
                    _userPermissions.Add(new { UserName = dic["UserName"], Action = dic["Action"] });
                }
            }
        }
        /// <summary>
        /// 调用管道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {

            //过滤客户端文件和无权限页面
            if (!Path.HasExtension(context.Request.Path.Value) && context.Request.Path.Value != _option.NoPermissionAction && context.Request.Path.Value != @"/ws")
            {
                //前台用cookies存一个标识，来对应后台的session的中的真正用户数据
                var cookie = context.Request.Cookies["browseweb"];
                if (cookie == null)
                {
                    if (context.Request.Path.Value == _option.LoginAction)
                    {
                        if (context.Request.Method == "POST")
                        {
                            var userName = context.Request.Form["username"];
                            var password = context.Request.Form["password"];
                            //验证用户名密码
                            var userDic = _permissionRepository.Login(userName, password);

                            if (userDic != null && userDic.Count > 0)
                            {
                                //添加cookie
                                var guid = Guid.NewGuid().ToString("N");
                                context.Response.Cookies.Append("browseweb", guid, new CookieOptions() { Path = "/", HttpOnly = true });
                                context.Session.SetString(guid + context.Connection.RemoteIpAddress.ToString(), userName);
                            }
                            else
                            {
                                context.Response.Redirect(_option.LoginAction);
                            }
                        }
                    }
                    else
                    {
                        context.Response.Redirect(_option.LoginAction);
                    }
                }
                else
                {
                    //验证权限
                    var username = context.Session.GetString(cookie + context.Connection.RemoteIpAddress.ToString());
                    if (string.IsNullOrEmpty(username))
                    {
                        context.Response.Cookies.Delete("browseweb");
                        context.Response.Redirect(_option.LoginAction);
                    }
                    else
                    { 
                        var actionCount = _userPermissions.Where(w => w.UserName == username&&w.Action== context.Request.Path.Value).Count();

                        if (actionCount<1)
                        {
                            context.Response.Redirect(_option.NoPermissionAction);
                        }
                    }
                }
            }
            return this._next(context);
        }
    }
}
