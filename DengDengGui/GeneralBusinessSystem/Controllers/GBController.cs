using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using Microsoft.Extensions.Logging;
using NLog;
using Microsoft.AspNetCore.Http;

namespace GeneralBusinessSystem.Controllers
{
    /// <summary>
    /// 通用项目平台控制层类
    /// </summary>
    public class GBController : Controller
    {
        /// <summary>
        /// 业务仓储类
        /// </summary>
        protected IBusinessRepository _businessRepository;
        /// <summary>
        /// 日志类
        /// </summary>
        protected Logger _log;
        /// <summary>
        /// 通用项目平台控制层实例
        /// </summary>
        /// <param name="businessRepository">业务仓储类</param>
        public GBController(IBusinessRepository businessRepository)
        {
            _log = LogManager.GetCurrentClassLogger();
            _businessRepository = businessRepository;
        }

        /// <summary>
        /// 通用项目平台控制层实例
        /// </summary>
        public GBController()
        {
            _log = LogManager.GetCurrentClassLogger();
        }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID
        {
            get
            {
                var cookie = Request.Cookies["browseweb"];
                var userJson = HttpContext.Session.GetString(cookie + HttpContext.Connection.RemoteIpAddress.ToString());
                var userObj = Newtonsoft.Json.JsonConvert.DeserializeObject(userJson);
                var companyID = (userObj as Newtonsoft.Json.Linq.JObject).GetValue("companyid").First.ToString();
                return Convert.ToInt32(companyID);
            }
        }
    }
}