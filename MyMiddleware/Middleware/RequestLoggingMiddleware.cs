using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MyMiddleware.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //context.Request.EnableRewind();

            //var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            //await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            //var requestBody = Encoding.UTF8.GetString(buffer);
            //context.Request.Body.Seek(0, SeekOrigin.Begin);

            //var builder = new StringBuilder(Environment.NewLine);
            //builder.AppendLine($"Request Path:{context.Request.Path.Value}");
            //foreach (var header in context.Request.Headers)
            //{
            //    builder.AppendLine($"{header.Key}:{header.Value}");
            //}

            //builder.AppendLine($"Request body:{requestBody}");

            ////LogHelper.WriteInfo(this.GetType(), builder.ToString());
            //Console.WriteLine(builder.ToString());
            ////throw new Exception("此处加日志");

            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
