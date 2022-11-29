using Serilog;
using Serilog.Events;
using Serilog.Sinks.ElmahIo;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.ElmahIo(new ElmahIoSinkOptions("API_KEY", new Guid("LOG_ID"))
    {
        MinimumLogEventLevel = LogEventLevel.Warning
    })
    .Enrich.FromLogContext()); // <- Remember to include this since ELmah.Io.AspNetCore.Serilog uses the LogContext for HTTP contextual information

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// This is where Serilog's LogContext is enriched with HTTP contextual information to be picked up by elmah.io's sink
app.UseElmahIoSerilog();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
