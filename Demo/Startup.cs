using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DUWENINK.Captcha.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo
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
            services.AddControllersWithViews(); // ����MVC��Ŀ
            services.AddMemoryCache();//ʹ�û��� 
            services.AddDUWENINKCaptcha();//ʹ����֤��
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

           // app.UsePathBase("Home/Index");

            app.UseRouting();
            // ���һ���м���������URL�ķ���
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    // �ض���/Home/Index
                    context.Response.Redirect("/Home/Index");
                    return;
                }
                await next();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                // ��������˵�����
            });
        }
    }
}
