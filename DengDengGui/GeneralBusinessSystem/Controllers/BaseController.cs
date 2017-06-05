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
    /// 平台控制层基类
    /// </summary>
    public class BaseController : Controller
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
        public BaseController(IBusinessRepository businessRepository)
        {
            _log = LogManager.GetCurrentClassLogger();
            _businessRepository = businessRepository;
        }

        /// <summary>
        /// 通用项目平台控制层实例
        /// </summary>
        public BaseController()
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
                var companyID = GetSessionValue("companyid");
                return Convert.ToInt32(companyID);
            }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string  Name
        {
            get
            {
                return GetSessionValue("name");
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string  UserName
        {
            get
            {              
                return GetSessionValue("username");
            }
        }
        /// <summary>
        /// 获取session中的数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string GetSessionValue(string key)
        {
            var cookie = Request.Cookies["browseweb"];
            var userJson = HttpContext.Session.GetString(cookie + HttpContext.Connection.RemoteIpAddress.ToString());
            var userObj = Newtonsoft.Json.JsonConvert.DeserializeObject(userJson);
            var value = (userObj as Newtonsoft.Json.Linq.JObject).GetValue(key).First.ToString();
            return value;
        }
    }
}