using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TipWeb.Models;

namespace TipWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
           
            services.AddMvc();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 设置微信和支付宝参数
        /// </summary>
        /// <param name="appSetting"></param>
        void BindConfig(AppSettings appSetting)
        {
            WxPayConfig.APPID = appSetting.WXAppID;
            WxPayConfig.APPSECRET = appSetting.WXAppSecert;
            WxPayConfig.KEY = appSetting.WXKey;
            WxPayConfig.MCHID = appSetting.WXMchid;
            WxPayConfig.NOTIFY_URL = $"http://{appSetting.DomainName}/notify";
            WxPayConfig.SSLCERT_PASSWORD = appSetting.WXSSLCertPassword;
            WxPayConfig.SSLCERT_PATH = appSetting.WXSSLCertPath;

        }
    }
}
