using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using Microsoft.Extensions.Logging;
using NLog;

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
        public GBController( )
        {
            _log = LogManager.GetCurrentClassLogger();
        }
    }
}