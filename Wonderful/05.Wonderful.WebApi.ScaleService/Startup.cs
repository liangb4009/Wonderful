using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Wonderful.WebApi.ScaleService.Models;

namespace Wonderful.WebApi.ScaleService
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {

                        builder.WithOrigins("https://localhost:44344",
                                            "http://localhost:44344",
                                            "https://localhost:55612",
                                            "http://localhost:55612",
                                            "https://10.210.146.140:8089",
                                            "http://10.143.172.99",
                                            "http://localhost:2194",
                                            "http://10.210.146.140:8089",
                                            "http://scsvr331.na.intranet.msd").AllowAnyMethod().AllowAnyHeader();
                    });

                options.AddPolicy("AnotherPolicy",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44344",
                                            "http://localhost:44344",
                                            "https://localhost:55612",
                                            "http://localhost:55612",
                                            "https://10.210.146.140:8089",
                                            "http://10.143.172.99",
                                            "http://localhost:2194",
                                            "http://10.210.146.140:8089",
                                            "http://scsvr331.na.intranet.msd")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });

            });
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ScaleService", Version = "v1" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "Wonderful.WebApi.ScaleService.xml");
                c.IncludeXmlComments(xmlPath);
            });
            services.AddDbContext<TodoContext>(opt =>
                //opt.UseInMemoryDatabase("TodoList")
                {
                    opt.UseSqlServer(Configuration["SqlServerConnStr"]);
                    opt.UseLazyLoadingProxies(false);
                }
            );
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=ScalePageService}/{action=DisplayScalePage}");
            });
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScaleService");
            });
        }
    }
}
