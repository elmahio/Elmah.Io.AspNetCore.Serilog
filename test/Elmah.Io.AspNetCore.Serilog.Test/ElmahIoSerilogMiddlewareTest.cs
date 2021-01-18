using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Elmah.Io.AspNetCore.Serilog.Test
{
    public class ElmahIoSerilogMiddlewareTest
    {
        [Test]
        public async Task MiddlewareCanDecorateLogContext()
        {
            // Arrange
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.TestCorrelator()
                .CreateLogger();
            var requestDelegate = new RequestDelegate((innerContext) =>
            {
                Log.Information("Test event");
                return Task.CompletedTask;
            });
            var middleware = new ElmahIoSerilogMiddleware(requestDelegate);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = new PathString("/test");
            httpContext.Request.Method = "GET";
            httpContext.Response.StatusCode = 404;
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "User")
            }));
            httpContext.Request.Headers.Add("RequestKey", new StringValues("RequestValue"));

            // Act
            IEnumerable<LogEvent> logEvents = null;
            using (TestCorrelator.CreateContext())
            {
                await middleware.Invoke(httpContext);

                logEvents = TestCorrelator.GetLogEventsFromCurrentContext();
            }

            // Assert
            Assert.That(logEvents, Is.Not.Null);
            Assert.That(logEvents.Count(), Is.EqualTo(1));
            var logEvent = logEvents.First();
            Assert.That(logEvent.Properties.Count, Is.EqualTo(8));
            Assert.That(logEvent.Properties.ContainsKey("url"));
            Assert.That(((dynamic)logEvent.Properties["url"]).Value, Is.EqualTo("/test"));
            Assert.That(logEvent.Properties.ContainsKey("method"));
            Assert.That(((dynamic)logEvent.Properties["method"]).Value, Is.EqualTo("GET"));
            Assert.That(logEvent.Properties.ContainsKey("statuscode"));
            Assert.That(((dynamic)logEvent.Properties["statuscode"]).Value, Is.EqualTo(404));
            Assert.That(logEvent.Properties.ContainsKey("user"));
            Assert.That(((dynamic)logEvent.Properties["user"]).Value, Is.EqualTo("User"));
            Assert.That(logEvent.Properties.ContainsKey("servervariables"));
            var serverVariable = ((DictionaryValue)logEvent.Properties["servervariables"]).Elements.FirstOrDefault();
            Assert.That(serverVariable, Is.Not.Null);
            Assert.That(serverVariable.Key.Value, Is.EqualTo("RequestKey"));
            Assert.That(((dynamic)serverVariable.Value).Value, Is.EqualTo("RequestValue"));
            Assert.That(logEvent.Properties.ContainsKey("cookies"));
            Assert.That(logEvent.Properties.ContainsKey("form"));
            Assert.That(logEvent.Properties.ContainsKey("querystring"));
        }
    }
}
