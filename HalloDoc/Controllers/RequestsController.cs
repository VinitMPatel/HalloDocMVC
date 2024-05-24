using Data.DataContext;
using Data.Entity;
using HalloDoc.Views.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System.Net.Mail;
using System.Net;
using Services.ViewModels;
using Common.Helper;
using Services.Implementation;

namespace HalloDoc.Controllers
{
    public class RequestsController : Controller
    {
        private readonly IFamilyRequest familyRequest;
        private readonly IConciergeRequest conciergeRequest;
        private readonly IBusinessRequest businessRequest;
        private readonly IPatientRequest patientRequest;
        public RequestsController(IFamilyRequest familyRequest , IConciergeRequest conciergeRequest, IBusinessRequest businessRequest, IPatientRequest patientRequest)
        {
            this.familyRequest = familyRequest;
            this.conciergeRequest = conciergeRequest;
            this.businessRequest = businessRequest;
            this.patientRequest = patientRequest;
        }

        public IActionResult FamilyFriendRequest()
        {
            return View();
        }

        public IActionResult ConciergeRequest()
        {
            return View();
        }
        public IActionResult BusinessRequest()
        {
            return View();
        }

        public async Task<IActionResult> FamilyInsert(FamilyFriendRequest r)
        {
            await familyRequest.FamilyInsert(r);
            TempData["success"] = "Request created successfully.";
            return RedirectToAction("PatientLogin", "Home");
        }

        public async Task<IActionResult> ConciergeInsert(ConciergeRequestData r)
        {
            await conciergeRequest.CnciergeInsert(r);
            TempData["success"] = "Request created successfully.";
            return RedirectToAction("PatientLogin", "Home");
        }

        public async Task<IActionResult> BusinessInsert(BusinessRequestData r)
        {
            await businessRequest.BusinessInsert(r);
            TempData["success"] = "Request created successfully.";
            return RedirectToAction("PatientLogin", "Home");
        }

        public async Task<bool> EmailValidate(string Email)
        {
            User user = await patientRequest.CheckEmail(Email);

            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateNewAccountLink(string email)
        {
            string encryptEmail = EncryptDecryptHelper.Encrypt(email);
            string Url = GenerateUrl(encryptEmail);
            SendEmail(email, "Create Your Account", $"Hello, create your account by using this link : {Url}");
        }

        private string GenerateUrl(string email)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("CreateAccount", "Requests" , new { email = email});
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

            patientRequest.EmailLog(email, message, subject);

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }

        public IActionResult CreateAccount(string email)
        {
            LoginPerson model = new LoginPerson();
            string decryptEmail = EncryptDecryptHelper.Decrypt(email);
            model.email = decryptEmail;
            return View(model);
        }
    }
}
