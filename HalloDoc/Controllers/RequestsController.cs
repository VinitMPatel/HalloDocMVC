using Data.DataContext;
using Data.Entity;
using HalloDoc.Views.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System.Net.Mail;
using System.Net;
using Services.ViewModels;

namespace HalloDoc.Controllers
{
    public class RequestsController : Controller
    {
        private readonly IFamilyRequest familyRequest;
        private readonly IConciergeRequest conciergeRequest;
        private readonly IBusinessRequest businessRequest;
        public RequestsController(IFamilyRequest familyRequest , IConciergeRequest conciergeRequest, IBusinessRequest businessRequest)
        {
            this.familyRequest = familyRequest;
            this.conciergeRequest = conciergeRequest;
            this.businessRequest = businessRequest;
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

        public IActionResult FamilyInsert(FamilyFriendRequest r)
        {
            bool temp = familyRequest.FamilyInsert(r);
            if(temp) {
                return RedirectToAction("patient_login", "Home");
            }
            else
            {
                string resetPasswordUrl = GenerateResetPasswordUrl();
                SendEmail(r.PatientEmail, "Reset Your Password", $"Hello, reset your password using this link: {resetPasswordUrl}");
                return RedirectToAction("patient_login", "Home");
            }
        }

        public IActionResult ConciergeInsert(ConciergeRequestData r)
        {
            conciergeRequest.CnciergeInsert(r);
            return RedirectToAction("patient_login", "Home");
        }

        public IActionResult BusinessInsert(BusinessRequestData r)
        {
            businessRequest.BusinessInsert(r);
            return RedirectToAction("patient_login", "Home");
        }

        private string GenerateResetPasswordUrl()
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("CreateAccount", "Home");
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
    }
}
