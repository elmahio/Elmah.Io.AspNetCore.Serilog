using Elmah.Io.AspNetCore.Serilog;

namespace Microsoft.AspNetCore.Builder
{
    public static class ElmahIoAspNetCoreSerilogExtensions
    {
        public static IApplicationBuilder UseElmahIoSerilog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ElmahIoSerilogMiddleware>();
        }
    }
}
