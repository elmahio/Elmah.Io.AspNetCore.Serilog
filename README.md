# Elmah.Io.AspNetCore.Serilog

Middleware that uses Serilog's `LogContext` to decorate all log messages from ASP.NET Core with HTTP context information like URL and status code. The middleware is not specific to [elmah.io](https://elmah.io) but is typically used with the `Serilog.Sinks.ElmahIo` package and ASP.NET Core.

Documentation: [https://docs.elmah.io/logging-to-elmah-io-from-serilog/#aspnet-core](https://docs.elmah.io/logging-to-elmah-io-from-serilog/#aspnet-core)