using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HalloDocMvcContext _context;

        public HomeController(ILogger<HomeController> logger , HalloDocMvcContext context)
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

        public async Task<IActionResult> validate(Aspnetuser u)
        {
            try
            {
                var x = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == u.Email);
            if (x.Passwordhash == u.Passwordhash) 
                { return RedirectToAction("patient_request", "Home"); }
            else
                { return RedirectToAction("index", "Home"); }

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}