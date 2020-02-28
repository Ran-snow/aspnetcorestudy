using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace MyMiddleware.Middleware
{
    /// <summary>
    /// 日志中间件
    /// </summary>
    /// <remarks>
    /// 参考文章：
    /// https://blog.csdn.net/qq_22949043/article/details/90717459
    /// https://zhuanlan.zhihu.com/p/39453491
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/request-response?view=aspnetcore-3.1
    /// </remarks>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //可重复读取Request.Body  Request.Body的CanSeek属性是false
            context.Request.EnableBuffering();

            //准备日志部分
            var stopWatch = Stopwatch.StartNew();
            var requestTime = DateTime.UtcNow;

            #region 获取请求报文

            PipeReader pipeReader = context.Request.BodyReader;
            string requestBodyContent = GetString((await pipeReader.ReadAsync()).Buffer, context.Request.ContentType);

            _logger.LogInformation(GetLogHeader(context, "RequestTime") + requestTime);
            _logger.LogInformation(GetLogHeader(context, "RequestMethod") + context.Request.Method);
            _logger.LogInformation(GetLogHeader(context, "RequestPath") + context.Request.Path);
            _logger.LogInformation(GetLogHeader(context, "RequestQueryString") + context.Request.QueryString);
            _logger.LogInformation(GetLogHeader(context, "RequestHeaders") + (context.Request.Headers.Count > 0 ?
                context.Request.Headers.Aggregate((a, b) =>
                    new KeyValuePair<string, StringValues>(string.Empty, new StringValues(a.Key + ":" + a.Value + Environment.NewLine + b.Key + ":" + b.Value)))
                    : new KeyValuePair<string, StringValues>(string.Empty, string.Empty)).Value, _logger);
            _logger.LogInformation(GetLogHeader(context, "RequestBodyContent") + requestBodyContent);

            #endregion

            #region 获取响应报文部分-1

            //由于相应报文流是不可读的，所以先备份上一个中间件的流，然后创建
            //一个可读写的流给下一个中间件，下一个中间件执行完毕后，读值，再把
            //上一个中间件的流弄回去
            var originalResponseBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            #endregion

            //执行其他中间件
            await _next(context);

            #region 获取响应报文部分-2

            var responseBodyContent = await ReadResponseBodyAsync(context.Response);
            await responseBody.CopyToAsync(originalResponseBodyStream);
            stopWatch.Stop();
            _logger.LogInformation(GetLogHeader(context, "ResponseHeaders") + (context.Response.Headers.Count > 0 ?
                context.Response.Headers.Aggregate((a, b) =>
                    new KeyValuePair<string, StringValues>(string.Empty, new StringValues(a.Key + ":" + a.Value + Environment.NewLine + b.Key + ":" + b.Value)))
                    : new KeyValuePair<string, StringValues>(string.Empty, string.Empty)).Value, _logger);
            _logger.LogInformation(GetLogHeader(context, "ResponseBodyContent") + responseBodyContent);
            _logger.LogInformation(GetLogHeader(context, "ElapsedMilliseconds") + stopWatch.ElapsedMilliseconds);

            #endregion

            //TODO 警告：使用之后，请求Body将会被释放!
            pipeReader.Complete();
        }

        /// <summary>
        /// 获取日志标题
        /// </summary>
        /// <param name="context"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private static string GetLogHeader(HttpContext context, string header)
        {
            return context.TraceIdentifier + "-" + header + "->";
        }

        /// <summary>
        /// 获取响应报文文本
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async ValueTask<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Position = 0;

            StreamReader responseReader = new StreamReader(response.Body, GetEncoding(response.ContentType));

            var responseContent = await responseReader.ReadToEndAsync();

            response.Body.Position = 0;

            return responseContent;
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static string GetString(ReadOnlySequence<byte> buffer, string contentType)
        {
            Encoding encoding = GetEncoding(contentType);

            if (buffer.IsSingleSegment)
            {
                return encoding.GetString(buffer.First.Span);
            }

            return string.Create((int)buffer.Length, buffer, (span, sequence) =>
            {
                foreach (var segment in sequence)
                {
                    encoding.GetChars(segment.Span, span);

                    span = span.Slice(segment.Length);
                }
            });
        }

        /// <summary>
        /// 获取Encoding
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static Encoding GetEncoding(string contentType)
        {
            var requestMediaType = contentType == null ? default : new MediaType(contentType);
            var requestEncoding = requestMediaType.Encoding;
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.UTF8;
            }
            return requestEncoding;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
