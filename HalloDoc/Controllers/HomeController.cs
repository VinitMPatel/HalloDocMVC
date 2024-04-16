
using Common.Enum;
using Data.DataContext;
using Data.Entity;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Services.ViewModels;
using Common.Helper;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HalloDocDbContext _context;
       

        public HomeController(ILogger<HomeController> logger, HalloDocDbContext context)
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

        public IActionResult ViewAgreement(string requestId)
        {
            AgreementDetails obj = new AgreementDetails();
            int decryptReqId = int.Parse(EncryptDecryptHelper.Decrypt(requestId));
            obj.requestId = decryptReqId;
            return View(obj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        
        public IActionResult PatientResetPasswordEmail(Aspnetuser user)
        {
            string Id = _context.Aspnetusers.Where(x => x.Email == user.Email).Select(x => x.Id).FirstOrDefault();
            string resetPasswordUrl = GenerateResetPasswordUrl(Id);
            SendEmail(user.Email, "Reset Your Password", $"Hello, reset your password using this link: {resetPasswordUrl}");

            return RedirectToAction("patient_login", "Home");
        }

        
        private string GenerateResetPasswordUrl(string userId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("SetPassword", "Home", new {id = userId});
            return baseUrl + resetPasswordPath;
        }

        
        private Task SendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.vinitpatel@outlook.com";
            var password = "016@ldce";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }

        // Handle the reset password URL in the same controller or in a separate one


        public IActionResult SetPassword(string id)
        {
            var aspuser = _context.Aspnetusers.FirstOrDefault(x => x.Id == id);
            return View(aspuser);
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(Aspnetuser aspnetuser)
        {
            var aspuser = _context.Aspnetusers.FirstOrDefault(x => x.Id == aspnetuser.Id);
            aspuser.Passwordhash = aspnetuser.Passwordhash;
            _context.Aspnetusers.Update(aspuser);
            await _context.SaveChangesAsync();
            return RedirectToAction("patient_login");
        }
    }
}
