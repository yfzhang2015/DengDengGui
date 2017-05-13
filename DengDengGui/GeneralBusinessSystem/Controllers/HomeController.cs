using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using System.Security.Claims;

namespace GeneralBusinessSystem.Controllers
{
    public class HomeController : Controller
    {
        IBusinessRepository _businessRepository;
        public HomeController(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
                return View();
            }
        }

    }
}
