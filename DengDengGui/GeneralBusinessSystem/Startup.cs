﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GeneralBusinessRepository;
using GeneralBusinessSystem.Model;
using GeneralBusinessData;
using Microsoft.Extensions.Options;
using GeneralBusinessData.SqlServer;
using GeneralBusinessSystem.Middleware;
using NLog.Web;
using NLog.Extensions.Logging;

namespace GeneralBusinessSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            //配置文件
            services.AddOptions();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //连接字符串
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            #region 添加session
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(120000);
                options.CookieHttpOnly = true;
            });
            #endregion

            //注入仓储对象
            DIRepository(services);

            services.AddMvc();
        }
        /// <summary>
        /// 注入仓储对象
        /// </summary>
        /// <param name="services">服务</param>
        void DIRepository(IServiceCollection services)
        {
            //注入权限模块，for sql server
            services.AddSingleton<IPermissionRepository>(new GeneralBusinessRepository.SqlServer.PermissionRepository(CreateSqlHelper()));
            //注入业务处理模块，for sql server
            services.AddSingleton<IBusinessRepository>(new GeneralBusinessRepository.SqlServer.BusinessRepository(CreateSqlHelper()));
            //注入单据模块，for sql server
            services.AddSingleton<IBillModuleRepository>(new GeneralBusinessRepository.SqlServer.BillModuleRepository(CreateSqlHelper()));
            //注入查询模块，for sql server
            services.AddSingleton<IQueryModuleRepository>(new GeneralBusinessRepository.SqlServer.QueryModuleRepository(CreateSqlHelper()));
            //注入图表模块，for sql server
            services.AddSingleton<IChartModuleRepository>(new GeneralBusinessRepository.SqlServer.ChartModuleRepository(CreateSqlHelper()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {


            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            // 添加NLog到.net core框架中
            loggerFactory.AddNLog();
            //添加NLog的中间件
            app.AddNLogWeb();
            // 指定NLog的配置文件
            env.ConfigureNLog("nlog.config");

            //添加session
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            //添加websocket中间件
            //app.UseWebSockets();
            //app.UseWebSocketNotify();
            ////添加权限中间件
            //app.UsePermission(new PermissionMiddlewareOption()
            //{
            //    LoginAction = @"/login",
            //    NoPermissionAction = @"/nopermission"
            //});


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 创建SqlHelper
        /// </summary>
        /// <param name="settings">配置文件</param>
        /// <param name="connectionStrings">数据库连接字符串</param>
        /// <returns></returns>
        ISqlHelper CreateSqlHelper()
        {
            ISqlHelper sqlHelper = null;
            var dataBase = Configuration.GetSection("AppSettings")["DataBase"];
            var defaultConnection = Configuration.GetSection("ConnectionStrings")["DefaultConnection"];
            switch (dataBase)
            {
                case "sqlserver":
                    sqlHelper = new SqlServerHelper(defaultConnection);
                    break;
            }
            return sqlHelper;
        }
    }
}
