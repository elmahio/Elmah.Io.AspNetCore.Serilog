using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elmah.Io.AspNetCore.Serilog
{
    /// <summary>
    /// The elmah.io Serilog middleware for ASP.NET Core automatically pushes HTTP contextual information to Serilog's LogContext.
    /// </summary>
    public class ElmahIoSerilogMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Create a new instance of the middleware. This constructor is called by ASP.NET Core and should never be invoked manually.
        /// </summary>
        public ElmahIoSerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Called by ASP.NET Core as part of the middleware pipeline. This method should never be invoked manually.
        /// </summary>
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
