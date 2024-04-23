using Common.Helper;
using Data.DataContext;
using Data.Entity;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Mail;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       
        private readonly IDashboard dashboard;

        public HomeController(ILogger<HomeController> logger, IDashboard dashboard)
        {
            _logger = logger;
           
            this.dashboard = dashboard;
        }

        public IActionResult index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PatientRequest()
        {
            return View();
        }

        public IActionResult PatientRequestScreen()
        {
            return View();
        }

        public IActionResult ResetPasswordPage()
        {
            return View();
        }

        public IActionResult PatientLogin()
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


        
        public async Task<IActionResult> PatientResetPasswordEmail(LoginPerson model)
        {
            (string aspNetUserId , string email) = await dashboard.FindUser(model);
            string encryptedId = EncryptDecryptHelper.Encrypt(aspNetUserId);
            string resetPasswordUrl = GenerateResetPasswordUrl(encryptedId);
            await SendEmail(email, "Reset Your Password", $"Hello, reset your password using this link: {resetPasswordUrl}");
            TempData["success"] = "Email Sent successfully.";
            return RedirectToAction("PatientLogin", "Home");
        }
        
        private string GenerateResetPasswordUrl(string userId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("SetPassword", "Home", new {id = userId});
            return baseUrl + resetPasswordPath;
        }

        
        private  Task SendEmail(string email, string subject, string message)
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


        public async Task<IActionResult> SetPassword(string id)
        {
            string decryptedId = EncryptDecryptHelper.Decrypt(id);
            LoginPerson model = new LoginPerson();
            model.aspNetUserId = decryptedId;
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(LoginPerson model)
        {
            await dashboard.UpdatePassword(model);
            TempData["success"] = "Password updated successfully.";
            return RedirectToAction("PatientLogin");
        }
    }
}
