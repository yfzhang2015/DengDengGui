using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog.Web;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;
using QuanMinDaShangPlatform.Models.Repository;

namespace QuanMinDaShangPlatform
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddOptions();
          //  services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
           // var connectionString = Configuration.GetSection("AppSettings:DBConnection").Value;
            //services.AddSingleton(connectionString);
            services.AddSingleton<AppSettings>();
            //sqlieconnection注放
            services.AddScoped<IDbConnection, SqlConnection>();
            //注入DapperPlus类
            services.AddScoped<IDapperPlusDB, DapperPlusDB>();
            //商户登录注册
            services.AddScoped<ICommercialRepository, CommercialRepository>();
            //打赏信息
            services.AddScoped<IDsInfoRepository, DsInfoRepository>();

            //全民打赏模板
            services.AddScoped<IQMDSTemplateRepository, QMDSTemplateRepository>();

            //公司打赏模板
            services.AddScoped<ICompanyTemplateRepository, CompanyTemplateRepository>();

            //人员打赏模板关系
            services.AddScoped<IEmployeeTemplateRelationRepository, EmployeeTemplateRelationRepository>();

            //权限认证
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "loginvalidate";
            }).AddCookie("loginvalidate", m =>
            {
                m.LoginPath = new PathString("/comlogin");
                m.AccessDeniedPath = new PathString("/comlogin");
               // m.LogoutPath = new PathString("/comlogin");
                m.Cookie.Path = "/";
            });
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,AppSettings appSettings)
        {
            
            //添加NLog的中间件
            app.AddNLogWeb();
            // 指定NLog的配置文件
            env.ConfigureNLog("nlog.config");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            BindConfig(appSettings);
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
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
