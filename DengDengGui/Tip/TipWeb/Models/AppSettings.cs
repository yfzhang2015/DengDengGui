using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TipWeb.Models
{
    public class AppSettings
    {
        IConfiguration _cfg;
        public AppSettings(IConfiguration cfg)
        {
            _cfg = cfg;
        }
        public string DBConnection
        {
            get
            {
                return _cfg.GetSection("AppSettings").GetSection("DBConnection").Value;
            }
        }
        public string DomainName
        {
            get
            {
                return _cfg.GetSection("AppSettings").GetSection("DomainName").Value;
            }
        }

        #region 微信参数
        /// <summary>
        /// 微信appid
        /// </summary>
        public string WXAppID
        {
            get
            {
                return _cfg.GetSection("AppSettings").GetSection("WXAppID").Value;
            }
        }
        /// <summary>
        /// 微信mchid
        /// </summary>
        public string WXMchid
        {
            get
            {
                return _cfg.GetSection("AppSettings").GetSection("WXMchid").Value;
            }
        }
        /// <summary>
        /// 微信key
        /// </summary>
        public string WXKey
        {
            get
            {
                return _cfg.GetSection("AppSettings").GetSection("WXKey").Value;
            }
        }
        /// <summary>
        /// 微信app secert
        /// </summary>
        public string WXAppSecert
        {
            get
            {
                return _cfg.GetSection("AppSettings").GetSection("WXAppSecert").Value;
            }
        }
        /// <summary>
        /// 微信ssl cert path
        /// </summary>
        public string WXSSLCertPath
        {
            get
            {
                return _cfg.GetSection("AppSettings").GetSection("WXSSLCertPath").Value;
            }
        }
        /// <summary>
        /// 微信ssl cert password
        /// </summary>
        public string WXSSLCertPassword
        {
            get
            {
                return _cfg.GetSection("AppSettings").GetSection("WXSSLCertPassword").Value;
            }
        }
        #endregion
    }
}
