using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Common;
using Microsoft.AspNetCore.Http;
using GeneralBusinessSystem.Middleware;

namespace GeneralBusinessSystem.Controllers
{
    public class PermissionController : BaseController
    {
        /// <summary>
        /// 权限仓储对象
        /// </summary>
        IPermissionRepository _permissionRepository;

        /// <summary>
        /// 实例化ManagementController
        /// </summary>
        /// <param name="businessRepository">业务仓储类</param>

        public PermissionController(IBusinessRepository businessRepository, IPermissionRepository permissionRepository) : base(businessRepository)
        {
            _permissionRepository = permissionRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有action,为树形控件作数据源
        /// </summary>
        /// <returns></returns>
        [HttpGet("allaction")]
        public IActionResult GetActions()
        {
            try
            {
                var permissions = _permissionRepository.GetPermissions();
                var actions = Common.ActionHandle.GetActions("basecontroller");
                var list = new List<dynamic>();
                foreach (var groupItem in actions.GroupBy(s => s.ControllerName))
                {
                    var node = new { name = groupItem.Key, open = true, children = new List<dynamic>() };
                    foreach (var action in actions)
                    {
                        if (groupItem.Key == action.ControllerName)
                        {
                            var permission = permissions.SingleOrDefault(s => s["Action"] == action.ActionName && s["ControllerName"] == action.ControllerName && s["Predicate"] == action.Predicate.ToString());
                            if (permission == null)
                            {
                                node.children.Add(new { name = $"{action.ActionName}【{action.Predicate}】" });
                            }
                            else
                            {
                                node.children.Add(new { name = $"{action.ActionName}【{action.Predicate}】", chkDisabled = true });
                            }
                        }
                    }
                    list.Add(node);
                }
                return new JsonResult(new { result = 1, message = "获取所有action成功", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"获取所有action：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"获取所有action：{exc.Message}" });
            }
        }

        #region User 用户操作
        /// <summary>
        /// 用户页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public ActionResult Users()
        {
            return View();
        }

        /// <summary>
        /// 查询全部用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("getusers")]
        public ActionResult GetUsers(string queryName)
        {
            try
            {
                if (string.IsNullOrEmpty(queryName))
                {
                    var list = _permissionRepository.GetUsers();
                    return new JsonResult(new { result = 1, message = "查询全部用户成功", data = list }, new JsonSerializerSettings()
                    {
                        ContractResolver = new LowercaseContractResolver()
                    });
                }
                else
                {
                    var list = _permissionRepository.GetUsers(queryName);
                    return new JsonResult(new { result = 1, message = $"按{queryName}查询用户成功", data = list }, new JsonSerializerSettings()
                    {
                        ContractResolver = new LowercaseContractResolver()
                    });
                }
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"查询全部用户：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"查询全部用户：{exc.Message}" });
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [HttpPost("adduser")]
        public IActionResult UserAdd(string userName, string password, string name)
        {
            try
            {
                // _permissionRepository.AddUser(userName, password, name, CompanyID);
                _permissionRepository.AddUser(userName, password, name, 1);
                return new JsonResult(new { result = 1, message = "添加用户成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"添加用户：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"添加用户：{exc.Message}" });
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [HttpPut("modifyuser")]
        public IActionResult UserModify(int ID, string userName, string password, string name)
        {
            try
            {
                _permissionRepository.ModifyUser(ID, userName, password, name);
                return new JsonResult(new { result = 1, message = "修改用户成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"修改用户：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"修改用户：{exc.Message}" });
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("deleteuser")]
        public IActionResult UserDelete(int ID)
        {

            try
            {
                _permissionRepository.RemoveUser(ID);
                return new JsonResult(new { result = 1, message = "删除用户成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"删除用户：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"删除用户：{exc.Message}" });
            }
        }
        #endregion

        #region Role 角色管理    
        /// <summary>
        /// 角色管理
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public IActionResult Roles()
        {
            return View();
        }
        /// <summary>
        /// 查询全部角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("getroles")]
        public IActionResult GetRoles()
        {
            try
            {
                var list = _permissionRepository.GetRoles();
                return new JsonResult(new { result = 1, message = $"查询全部角色", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"查询全部角色：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"查询全部角色：{exc.Message}" });
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="rolename">角色名称</param>
        /// <returns></returns>
        [HttpPost("addrole")]
        public IActionResult AddRole(string name)
        {
            try
            {
                //_permissionRepository.AddRole(name, CompanyID);
                _permissionRepository.AddRole(name, 1);
                return new JsonResult(new { result = 1, message = "添加角色成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"添加角色：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"添加角色：{exc.Message}" });
            }
        }
        /// <summary>
        /// 修改角色 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="rolename">角色名称</param>
        /// <returns></returns>
        [HttpPut("modifyrole")]
        public IActionResult ModifyRole(int id, string name)
        {
            try
            {
                _permissionRepository.ModifyRole(id, name);
                return new JsonResult(new { result = 1, message = "修改角色成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"修改角色：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"修改角色：{exc.Message}" });
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpDelete("deleterole")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                _permissionRepository.RemoveRole(id);
                return new JsonResult(new { result = 1, message = "删除角色成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"删除角色：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"删除角色：{exc.Message}" });
            }
        }
        #endregion

        #region Permission权限管理
        [HttpGet("permissions")]
        public IActionResult Permissions()
        {
            return View();
        }
        /// <summary>
        /// 获取全部权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("getpermissions")]
        public IActionResult GetPermissions()
        {
            try
            {
                var list = _permissionRepository.GetPermissions();
                return new JsonResult(new { result = 1, message = $"获取全部权限成功", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"获取全部权限：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"获取全部权限：{exc.Message}" });
            }
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns></returns>
        [HttpDelete("removepermission")]
        public IActionResult RemovePermission(int id)
        {
            try
            {
                var result = _permissionRepository.RemovePermission(id);
                return new JsonResult(new { result = 1, message = "删除权限成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"删除权限：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"删除权限：{exc.Message}" });
            }
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="actiondescription">action描述</param>
        /// <param name="controllername">controller</param>
        /// <param name="predicate">谓词</param>
        /// <returns></returns>
        [HttpPost("addpermission")]
        public dynamic AddPermission(string action, string actiondescription, string controllername, string predicate)
        {
            try
            {
                _permissionRepository.AddPermission(action, actiondescription, controllername, predicate, 1);
                // _permissionRepository.AddPermission(action, actiondescription, controllername, predicate, CompanyID);
                return new JsonResult(new { result = 1, message = "添加权限成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"添加权限：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"添加权限：{exc.Message}" });
            }
        }




        #endregion

        #region 角色权限管理
        /// <summary>
        /// 角色权限设置
        /// </summary>
        /// <returns></returns>
        [HttpGet("rolepermission")]
        public IActionResult RolePermission()
        {
            return View();
        }
        /// <summary>
        /// 按角色ID查询权限
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        [HttpGet("getpermission")]
        public IActionResult GetPermissionByRoleID(int roleID)
        {
            try
            {
                var list = _permissionRepository.GetPermissionsByRoleID(roleID);
                return new JsonResult(new { result = 1, message = $"按角色ID:{roleID}查询权限成功", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"按角色ID:{roleID}查询权限：{ exc.Message}");
                return new JsonResult(new { result = 0, message = $"按角色ID:{roleID}查询权限：{ exc.Message}" });
            }
        }
        /// <summary>
        /// 批量保存角色权限
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="rolePermissions">角色权限</param>
        /// <returns></returns>
        [HttpPost("savarolepermissons")]
        public IActionResult SavaRolePermissions(int roleid, List<Model.ViewModel.RolePermission> rolepermissions)
        {
            try
            {
                var list = new List<dynamic>();
                list.AddRange(rolepermissions);
                _permissionRepository.SavaRolePermissions(roleid, list);
                ReLoadPermissions();
                return new JsonResult(new { result = 1, message = "批量保存角色权限成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"批量保存角色权限：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"批量保存角色权限：{exc.Message}" });
            }
        }
        /// <summary>
        /// 重新加载权限
        /// </summary>
        void ReLoadPermissions()
        {
            PermissionMiddleware._userPermissions.Clear();
            PermissionMiddleware._userPermissions = new List<dynamic>();
            foreach (var dic in _permissionRepository.GetUserPermissions())
            {
                PermissionMiddleware._userPermissions.Add(new
                {
                    UserName = dic["UserName"],
                    Action = dic["Action"]
                });
            }
        }
        #endregion

        #region 用户角色管理
        /// <summary>
        /// 用户角色设置
        /// </summary>
        /// <returns></returns>
        [HttpGet("userrole")]
        public IActionResult UserRole()
        {
            return View();
        }
        /// <summary>
        /// 按用户ID查询角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        [HttpGet("getrole")]
        public IActionResult GetRoleByUserID(int userID)
        {
            try
            {
                var list = _permissionRepository.GetRoleByUserID(userID);
                return new JsonResult(new { result = 1, message = $"按用户ID:{userID}查询角色成功", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"按用户ID:{userID}查询角色：{ exc.Message}");
                return new JsonResult(new { result = 0, message = $"按用户ID:{userID}查询角色：{ exc.Message}" });
            }
        }
        /// <summary>
        /// 批量保存用户角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="userroles">用户角色</param>
        /// <returns></returns>
        [HttpPost("savauserroles")]
        public IActionResult SavaUserRoles(int userid, List<Model.ViewModel.UserRole> userroles)
        {
            try
            {
                var list = new List<dynamic>();
                list.AddRange(userroles);
                _permissionRepository.SavaUserRoles(userid, list);
                return new JsonResult(new { result = 1, message = "批量保存用户角色成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"批量保存用户角色：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"批量保存用户角色：{exc.Message}" });
            }
        }
        #endregion

    }
}