using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using System.Reflection;

namespace GeneralBusinessSystem.Controllers
{
    public class ManagementController : GBController
    {
        /// <summary>
        /// 实例化ManagementController
        /// </summary>
        /// <param name="businessRepository">业务仓储类</param>
        public ManagementController(IBusinessRepository businessRepository) : base(businessRepository)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        #region User 用户操作
        /// <summary>
        /// 用户页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public ActionResult Users()
        {
            return View(_businessRepository.GetUsers());
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        [HttpGet("queryusers")]
        public ActionResult QueryUser(string queryName)
        {
            return new JsonResult(_businessRepository.GetUsers(queryName));
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [HttpPost("adduser")]
        public bool UserAdd(string userName, string password, string name)
        {
            try
            {
                _businessRepository.AddUser(userName, password, name);

                return true;
            }
            catch
            {
                return false;
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
        [HttpPost("modifyuser")]
        public bool UserModify(int ID, string userName, string password, string name)
        {
            try
            {

                _businessRepository.ModifyUser(ID, userName, password, name);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("deleteuser")]
        public bool UserDelete(int ID)
        {

            try
            {
                _businessRepository.RemoveUser(ID);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Role 角色管理
        /// <summary>
        /// 添加角色 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddRole()
        {
            return View();
        }

        [HttpGet("roles")]
        public IActionResult Roles()
        {
            var list = _businessRepository.GetRoles();
            return View(list);
        }
        [HttpPost("roles")]
        public IActionResult GetRoles()
        {
            var list = _businessRepository.GetRoles();
            return new JsonResult(list, new Newtonsoft.Json.JsonSerializerSettings());
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="rolename">角色名称</param>
        /// <returns></returns>
        [HttpPost("addrole")]
        public bool AddRole(string rolename)
        {
            return _businessRepository.AddRole(rolename) > 0 ? true : false;
        }
        /// <summary>
        /// 修改角色 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="rolename">角色名称</param>
        /// <returns></returns>
        [HttpPost("modifyrole")]
        public bool ModifyRole(int id, string rolename)
        {
            return _businessRepository.ModifyRole(id, rolename) > 0 ? true : false;
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost("deleterole")]
        public bool DeleteRole(int id)
        {
            return _businessRepository.RemoveRole(id) > 0 ? true : false;
        }
        #endregion

        #region Permission权限管理
        [HttpGet("permissions")]
        public IActionResult Permissions()
        {
            return View();
        }
        #endregion

        #region 菜单模块管理
        /// <summary>
        /// 菜单模块管理
        /// </summary>
        /// <returns></returns>
        [HttpGet("menumodules")]
        public IActionResult MenuModules()
        {
            return View();
        }
        /// <summary>
        /// 获取所有action
        /// </summary>
        /// <returns></returns>
        public IActionResult GetActions()
        {
            var actions = Common.ActionHandle.GetActions();
            return new JsonResult(actions);
        }
        #endregion
    }
}