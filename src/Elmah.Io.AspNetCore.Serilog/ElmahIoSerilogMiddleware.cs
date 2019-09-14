using Microsoft.AspNetCore.Http;
using Serilog.Context;
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
                await _next.Invoke(context);
        }
    }
}
