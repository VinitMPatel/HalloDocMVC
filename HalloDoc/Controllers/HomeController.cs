
using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HelloDocDbContext _context;


        public HomeController(ILogger<HomeController> logger, HelloDocDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult patient_request()
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

        public IActionResult patient_login()
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

        public IActionResult patient_dashboard()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> validate(Aspnetuser u)
        {
                var x = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == u.Email);
            try
            {
                if (x.Passwordhash == u.Passwordhash)
                {

                    return RedirectToAction("patient_dashboard");
                }

                TempData["password"] = "*enter valid password";
                return RedirectToAction("patient_login", "home");

            }
            catch (Exception e)
            {
                TempData["email"] = "*enter valid email";
                return RedirectToAction("patient_login", "home");
                {
                }

            }
        }
    }
}