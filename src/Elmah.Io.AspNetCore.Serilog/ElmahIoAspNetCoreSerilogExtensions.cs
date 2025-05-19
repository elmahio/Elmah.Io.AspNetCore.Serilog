using Elmah.Io.AspNetCore.Serilog;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Builder
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// This is a helper class to help users install the elmah.io Serilog middleware for ASP.NET Core.
    /// </summary>
    public static class ElmahIoAspNetCoreSerilogExtensions
    {
        /// <summary>
        /// Install the elmah.io middleware for Serilog. The middleware will decorate Serilogs LogContext
        /// with HTTP contextual information that the elmah.io sink will pick up when logging messages.
        /// To use this middleware make sure to also call the Enrich.FromLogContext() method.
        /// </summary>
        public static IApplicationBuilder UseElmahIoSerilog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ElmahIoSerilogMiddleware>();
        }
    }
}
