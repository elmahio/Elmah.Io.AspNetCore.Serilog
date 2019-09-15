using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elmah.Io.AspNetCore.Serilog
{
    public class ElmahIoSerilogMiddleware
    {
        private readonly RequestDelegate _next;

        public ElmahIoSerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("url", context.Request?.Path.Value))
            using (LogContext.PushProperty("method", context.Request?.Method))
            using (LogContext.PushProperty("statuscode", context.Response.StatusCode))
            using (LogContext.PushProperty("user", context.User?.Identity?.Name))
            using (LogContext.PushProperty("servervariables", ServerVariables(context)))
            using (LogContext.PushProperty("cookies", Cookies(context)))
            using (LogContext.PushProperty("form", Form(context)))
            using (LogContext.PushProperty("querystring", QueryString(context)))
                await _next.Invoke(context);
        }

        private Dictionary<string, string> QueryString(HttpContext context)
        {
            return context.Request?.Query?.Keys.ToDictionary(k => k, k => context.Request.Query[k].ToString());
        }

        private Dictionary<string, string> Form(HttpContext context)
        {
            try
            {
                return context.Request?.Form?.Keys.ToDictionary(k => k, k => context.Request.Form[k].ToString());
            }
            catch (InvalidOperationException)
            {
                // Request not a form POST or similar
            }

            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> Cookies(HttpContext context)
        {
            return context.Request?.Cookies?.Keys.ToDictionary(k => k, k => context.Request.Cookies[k].ToString());
        }

        private Dictionary<string, string> ServerVariables(HttpContext context)
        {
            return context.Request?.Headers?.Keys.ToDictionary(k => k, k => context.Request.Headers[k].ToString());
        }
    }
}
