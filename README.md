# Elmah.Io.AspNetCore.Serilog

[![Build status](https://github.com/elmahio/Elmah.Io.AspNetCore.Serilog/workflows/build/badge.svg)](https://github.com/elmahio/Elmah.Io.AspNetCore.Serilog/actions/workflows/build.yml) [![NuGet Version](https://img.shields.io/nuget/v/Elmah.Io.AspNetCore.Serilog.svg?style=flat)](https://www.nuget.org/packages/Elmah.Io.AspNetCore.Serilog/)

Middleware that uses Serilog's `LogContext` to decorate all log messages from ASP.NET Core with HTTP context information like URL and status code. The middleware is not specific to [elmah.io](https://elmah.io) but is typically used with the `Serilog.Sinks.ElmahIo` package and ASP.NET Core.

Documentation: [https://docs.elmah.io/logging-to-elmah-io-from-serilog/#aspnet-core](https://docs.elmah.io/logging-to-elmah-io-from-serilog/#aspnet-core)