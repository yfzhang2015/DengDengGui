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

namespace GeneralBusinessSystem.Controllers
{
    /// <summary>
    /// 后台管理controller
    /// </summary>
    public class ManagementController : BaseController
    {
        /// <summary>
        /// 单据模块
        /// </summary>
        IBillModuleRepository _billModule;
        /// <summary>
        /// 查询模块
        /// </summary>
        IQueryModuleRepository _queryModule;
        /// <summary>
        /// 图表模块
        /// </summary>
        IChartModuleRepository _chartModule;
        /// <summary>
        /// 实例化ManagementController
        /// </summary>
        /// <param name="businessRepository">业务仓储类</param>
        public ManagementController(IBusinessRepository business, IBillModuleRepository billModule, IQueryModuleRepository queryModule, IChartModuleRepository chartModule) : base(business)
        {
            _billModule = billModule;
            _queryModule = queryModule;
            _chartModule = chartModule;
        }
        /// <summary>
        /// 后台首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        #region 菜单管理
        /// <summary>-
        /// 菜单管理
        /// </summary>
        /// <returns></returns>
        [HttpGet("menus")]
        public IActionResult Menus()
        {
            return View();
        }

        /// <summary>
        /// 查询全部菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("getmenus")]
        public IActionResult GetMenus()
        {
            try
            {
                var list = _businessRepository.GetMenus();
                return new JsonResult(new { result = 1, message = $"查询全部菜单成功",data= list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"查询全部菜单：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"查询全部菜单：{exc.Message}" });
            }
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="rolename">名称</param>
        /// <returns></returns>
        [HttpPost("addmenu")]
        public IActionResult AddMenu(string name)
        {
            try
            {
               // _businessRepository.AddMenu(name, CompanyID);
                _businessRepository.AddMenu(name, 1);   
                return new JsonResult(new { result = 1, message = $"添加菜单成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"添加菜单：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"添加菜单：{exc.Message}" });
            }
        }
        /// <summary>
        /// 修改菜单 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [HttpPost("modifymenu")]
        public IActionResult ModifyMenu(int id, string name)
        {
            try
            {
                _businessRepository.ModifyMenu(id, name);
                return new JsonResult(new { result = 1, message = $"修改菜单成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"修改菜单：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"修改菜单：{exc.Message}" });
            }
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost("deletemenu")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                _businessRepository.RemoveMenu(id);
                return new JsonResult(new { result = 1, message = $"删除菜单成功" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"删除菜单：{exc.Message}");
                return new JsonResult(new { result = 0, message = $"删除菜单：{exc.Message}" });

            }
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
        /// 获取菜单模块中的菜单列表，单据，查询，图表模块列表
        /// </summary>
        /// <returns></returns>
        public IActionResult GetMenuModules()
        {
            var menus = _businessRepository.GetMenus();
            var billModules = _billModule.GetBillModules();
            var queryModules = _queryModule.GetQueryModules();
            var chartModules = _chartModule.GetChartModules();
            return new JsonResult(new { menus = menus, bills = billModules, queries = queryModules, charts = chartModules }, new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            });
        }

        #endregion


    }
}