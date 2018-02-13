using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TipWeb.Models;

namespace TipWeb.Controllers
{
    public class HomeController : Controller
    {

           /// <summary>
        /// 配置文件
        /// </summary>
        readonly AppSettings _setting;

      
        public HomeController(AppSettings appSettings)
        {
            _setting = appSettings;           
        }

        public IActionResult Index()
        {
            return View();
        }

        #region 获取GetOpenid
        /// <summary>
        /// 获取GetOpenid
        /// </summary>
        /// <param name="redirectUrl">路由</param>
        /// <param name="code">微信code</param>
        /// <returns></returns>
        [HttpGet("getopenid/{employeeid}")]
        public IActionResult GetOpenID(string redirectUrl, string code = null, string employeeid = null)
        {
            var jsApiPay = new JsApiPay();
            try
            {
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                var url = jsApiPay.GetOpenidAndAccessToken($@"{_setting.DomainName}/getopenid/{employeeid}?redirectUrl=" + redirectUrl, code);

                if (string.IsNullOrEmpty(url))
                {
                    if (employeeid == "employee")
                    {
                        return Redirect($"/login?openID={jsApiPay.openid}");
                    }
                    return Redirect($"/wxpay?openid={jsApiPay.openid}&employeeid={employeeid}");
                }
                else
                {
                    return Redirect(url);
                }

            }
            catch (Exception exc)
            {
                return NotFound();
            }
        }
        #endregion
    }
}
