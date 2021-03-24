using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApp
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sw = new Stopwatch();
            sw.Start();

            string name = httpContext.Request.Query["name"];
            if (!string.IsNullOrWhiteSpace(name))
            {
                httpContext.Request.Headers.Add("name", name);
            }
            await httpContext.Response.WriteAsync($"<h1>URL: {httpContext.Request.Path} {httpContext.Request.QueryString}</h1>");
            await httpContext.Response.WriteAsync($"<h1>Name: {name}</h1><h1>Took: {sw.ElapsedMilliseconds} Milliseconds</h1>");
            await _next(httpContext);
        }

    }
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}
