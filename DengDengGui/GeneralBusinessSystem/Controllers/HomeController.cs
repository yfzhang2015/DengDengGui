using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using System.Security.Claims;
using NLog;

namespace GeneralBusinessSystem.Controllers
{
    public class HomeController : BaseController
    {
        IPermissionRepository _permissionRepository;

        /// <summary>
        /// 实例化homecontroller
        /// </summary>
        /// <param name="businessRepository">业务仓储类</param>
        public HomeController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        /// <summary>
        /// 错误
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// 无权限action
        /// </summary>
        /// <returns></returns>
        [HttpGet("nopermission")]
        public IActionResult NoPermission()
        {
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登录提交
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(string username,string password)
        {
            _log.Info($"{username}登录");
            var user = _permissionRepository.Login(username, password);
            if (user != null && user.Count > 0)
            {
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,username)
                };
                HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
                return Redirect("/");
            }
            else
            {
                _log.Info($"{username}用户名或密码不正确");
                return View();
            }
        }

        [HttpGet("/autocomplete/countries")]
        public IActionResult GetCountry(string queryBH)
        {
            var list = new List<dynamic>();
            list.Add(new { value = "United Arab Emirates", data = "AE" });
            list.Add(new { value = "United Kingdom", data = "UK" });
            list.Add(new { value = "United States", data = "US" });
            list.Add(new { value = "Chinese ", data = "CN" });
            return new JsonResult(new {query= queryBH, suggestions = list });
        }

        public IActionResult WebSocketPage()
        {
            return View();
        }

    }
}
