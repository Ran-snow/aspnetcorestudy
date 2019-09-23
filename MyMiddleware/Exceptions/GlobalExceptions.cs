using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyMiddleware.Exceptions
{
    /// <summary>
    /// 全局Exception
    /// </summary>
    public class GlobalExceptions : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;

        public GlobalExceptions(IHostingEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            //这里面是自定义的操作记录日志
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                if (_env.IsDevelopment())
                {
                    json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息                
                }
                context.Result = new BadRequestObjectResult(json);//返回异常数据            
            }
            else
            {
                json.Message = "发生了未知内部错误";
                if (_env.IsDevelopment())
                {
                    json.Message = context.Exception.Message;
                    json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息                }
                    context.Result = new InternalServerErrorObjectResult(json);
                }

                throw new System.Exception("此处需修改，记录日志");
                System.Console.WriteLine(json.Message);
            }
        }

        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }

    /// <summary>
    /// 错误返回结果
    /// </summary>
    public class JsonErrorResponse
    {
        public bool IsSuccess { get; } = false;

        public object Body { get; set; }

        /// <summary>
        /// 生产环境的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 开发环境的消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }

}