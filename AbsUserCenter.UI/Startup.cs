using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AbsUserCenter.Core.Config;
using AbsUserCenter.Tool;
using AbsUserCenter.UI.Filter;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace AbsUserCenter.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //session
            services.AddDistributedMemoryCache();
            services.AddSession();
            //cookie身份验证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Login/Index";
                options.LogoutPath = "/Login/LogOff";
                options.AccessDeniedPath = new PathString("/Error/Forbidden");//拒绝访问页面
                options.Cookie.Path = "/";
            });
            
            services.AddMvc(options=> {
                options.Filters.Add(typeof(SampleActionFilterAttribute));
            });
            //ioc
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModuleRegister>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            IoC.SetContainer(container);
            return new AutofacServiceProvider(container);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
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
            Configuration.GetSection("App").Bind(new ConfigOptions());
            Configuration.GetReloadToken().RegisterChangeCallback(OnSettingChanged, Configuration);
            
            app.UseStaticFiles();

            app.UseSession();
            ////Cookie(2)使用Cookie身份认证的中间件
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                //routes.MapRoute("defaultApi", "Api/{controller}/{action}/{id?}",
                //    defaults: new { area = "Api" }, constraints: new { area = "Api" });
                routes.MapRoute(
                       name: "default",
                       template: "{controller}/{action}/{id?}",
                       defaults: new { controller = "Home", action = "Index" });
                routes.MapAreaRoute("defaultApi", "Api", "Api/{controller}/{action}/{id?}");
                
            });
        }
        private static IDisposable callbackRegistration;
        public void OnSettingChanged(object state)
        {
            callbackRegistration?.Dispose();
            IConfiguration configuration = (IConfiguration)state;
            configuration.GetSection("App").Bind(new ConfigOptions());
            callbackRegistration = configuration.GetReloadToken()
               .RegisterChangeCallback(OnSettingChanged, state);
        }
    }
}
