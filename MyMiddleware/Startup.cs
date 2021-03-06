﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyMiddleware.Exceptions;
using MyMiddleware.JsonConvert;
using MyMiddleware.Middleware;

namespace MyMiddleware
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
            services.AddControllers()
                 .ConfigureApiBehaviorOptions(options =>
                 {
                     //options.SuppressInferBindingSourcesForParameters = true;
                     //options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = true;

                     options.InvalidModelStateResponseFactory = actionContext =>
                     {
                         //获取验证失败的模型字段 
                         var errors = actionContext.ModelState
                             .Where(e => e.Value.Errors.Count > 0)
                             .Select(e => e.Value.Errors.First().ErrorMessage)
                             .ToList();

                         throw new UserOperationException(string.Join("|", errors));
                     };
                 })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestLoggingMiddleware();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
