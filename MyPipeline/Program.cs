using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPipeline
{
    /*  管道设计思路
        1. 定义一个处理HTTP请求的方法"RequestDelegate", "Context" 为简化的HTTP请求报文
        2. 定义一个"层层"转包的规则 "Func<RequestDelegate, RequestDelegate>" 
           即上一管道处理过的请求,到下一管道接着处理. 所以入出参都为 "RequestDelegate"
        3. 把这个层层转包的规则拼接起来
        4. 执行最后的规则
     */
    class Program
    {
        static List<Func<RequestDelegate, RequestDelegate>>
            _list = new List<Func<RequestDelegate, RequestDelegate>>();

        static void Main(string[] args)
        {
            
            use(request =>
            {
                return context =>
                {
                    Console.WriteLine("1");
                    // return Task.CompletedTask;
                    return request.Invoke(context);
                };
            });

            use(request =>
            {
                return context =>
                {
                    Console.WriteLine("2");
                    return request.Invoke(context);
                };
            });
            
            RequestDelegate end = (context) =>
            {
                Console.WriteLine("end......");

                return Task.CompletedTask;
            };

             _list.Reverse();
            foreach (var middleware in _list)
            {
                end = middleware.Invoke(end);
            }

            end.Invoke(new Context());
            Console.WriteLine("Hello World!");
        }

        static void use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _list.Add(middleware);
        }
    }
}
