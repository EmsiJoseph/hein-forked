using System.Diagnostics;
using Hein.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hein.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hein.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET /Home
        [HttpGet("")]
        [HttpGet("/")]
        public IActionResult Index()
        {
            TempDataHelper.RemoveTempData(this, "Email");
            TempDataHelper.RemoveTempData(this, "Phone");
            TempDataHelper.RemoveTempData(this, "FirstName");
            TempDataHelper.RemoveTempData(this, "LastName");
            return View();
        }

        // GET /Home/Privacy
        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}