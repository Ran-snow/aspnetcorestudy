using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebAPIHttp
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //有cookie时 Access-Control-Allow-Origin 不能为 *
            //https://segmentfault.com/a/1190000015552557
            //https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Access_control_CORS
            services.AddCors(options =>
            {
                options.AddPolicy("any", policyBuilder =>
                {
                    policyBuilder.AllowAnyMethod()
                        .AllowAnyHeader()
                        //.WithMethods("GET", "HEAD", "POST", "PUT", "PATCH", "DELETE", "OPTIONS", "DEBUG");
                        .AllowCredentials();//指定处理cookie

                    var cfg = Configuration.GetSection("AllowedHosts").Get<List<string>>();
                    if (cfg == null || cfg.Contains("*")) policyBuilder.AllowAnyOrigin(); //允许任何来源的主机访问
                    else policyBuilder.WithOrigins(cfg.ToArray()); //允许类似http://localhost:8080等主机访问
                });
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(configureOptions =>
                {
                    configureOptions.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("any");
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
