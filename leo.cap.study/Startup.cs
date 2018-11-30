using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leo.cap.study.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static leo.cap.study.Service.TestService;

namespace leo.cap.study
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
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                   (b) =>
                   {
                       b.UseRowNumberForPaging();
                       b.MigrationsAssembly("leo.cap.study");
                   })
           );
            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddCap(x =>
            {
                //如果你使用的 EF 进行数据操作，你需要添加如下配置：
                x.UseEntityFramework<AppDbContext>();  //可选项，你不需要再次配置 x.UseSqlServer 了

                //如果你使用的Ado.Net，根据数据库选择进行配置：
                //x.UseSqlServer("数据库连接字符串");
                //x.UseMySql("Your ConnectionStrings");
                //x.UsePostgreSql("Your ConnectionStrings");

                //如果你使用的 MongoDB，你可以添加如下配置：
                //x.UseMongoDB("Your ConnectionStrings");  //注意，仅支持MongoDB 4.0+集群

                //如果你使用的 RabbitMQ 或者 Kafka 作为MQ，根据使用选择配置：
                x.UseRabbitMQ(options=> {
                    options.HostName = "123.207.70.217";
                    options.Port = 5672;
                    options.UserName = "guest";
                    options.Password = "guest";
                });
                //x.UseKafka("localhost");

                // 注册 Dashboard
                x.UseDashboard();
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseCap();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
