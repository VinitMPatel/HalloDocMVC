
using Common.Enum;
using Data.DataContext;
using Data.Entity;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HelloDocDbContext _context;
        private readonly IValidation validation;

        public HomeController(ILogger<HomeController> logger, HelloDocDbContext context, IValidation validation)
        {
            _logger = logger;
            _context = context;
            this.validation = validation;
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

        [HttpPost]
        public IActionResult patient_login(Aspnetuser aspnetuser)
        {
            var result = validation.Validate(aspnetuser);
            var check = _context.Aspnetusers.Where(u => u.Email == aspnetuser.Email).FirstOrDefault();
            var userdata = _context.Users.Where(u => u.Aspnetuserid == check.Id).FirstOrDefault();
            if (result.Status==ResponseStautsEnum.Success)
            {
                
                HttpContext.Session.SetInt32("UserId", userdata.Userid);
                return RedirectToAction("PatientDashboard", "patient");
            }
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