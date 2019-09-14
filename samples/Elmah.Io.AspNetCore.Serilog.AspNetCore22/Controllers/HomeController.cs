using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Elmah.Io.AspNetCore.Serilog.AspNetCore22.Models;
using Microsoft.Extensions.Logging;

namespace Elmah.Io.AspNetCore.Serilog.AspNetCore22.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            logger.LogWarning("Someone requested the frontpage");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
