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
    public class HomeController : Controller
    {
        /// <summary>
        /// 业务仓储类
        /// </summary>
        IBusinessRepository _businessRepository;
        /// <summary>
        /// 日志类
        /// </summary>
        Logger _log;
        /// <summary>
        /// 实例化homecontroller
        /// </summary>
        /// <param name="businessRepository">业务仓储类</param>
        public HomeController(IBusinessRepository businessRepository)
        {
            _log = LogManager.GetCurrentClassLogger();
            _businessRepository = businessRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        [HttpGet("nopermission")]
        public IActionResult NoPermission()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(string username,string password)
        {
            _log.Info($"{username}登录");
            var user = _businessRepository.Login(username, password);
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

    }
}
