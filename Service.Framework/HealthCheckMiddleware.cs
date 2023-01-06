using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Service.Framework
{
    /// <summary>
    /// 写个中间件---扩展
    /// </summary>
    public static class HealthCheckMiddlewareExtension
    {
        /// <summary>
        /// 设置心跳响应
        /// </summary>
        /// <param name="app"></param>
        /// <param name="checkPath">默认是/Health</param>
        /// <returns></returns>
        public static void UseHealthCheckMiddleware(this IApplicationBuilder app, string checkPath = "/healthcheck")
        {
            app.Map(checkPath, applicationBuilder => applicationBuilder.Run(async context =>
            {
                Console.WriteLine($"This is Health Check");
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync("OK");
            }));
        }

    }
}
