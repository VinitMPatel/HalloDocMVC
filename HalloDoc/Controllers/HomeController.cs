using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult patient_first()
        {
            return View();
        }

        public IActionResult patient_request_screen()
        {
            return View();
        }

        public IActionResult resetpwd()
        {
            return View();
        }

        public IActionResult patient_request()
        {
            return View();
        }

        public IActionResult family_friend_request()
        {
            return View();
        }

        public IActionResult concierge_request()
        {
            return View();
        }

        public IActionResult business_request()
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