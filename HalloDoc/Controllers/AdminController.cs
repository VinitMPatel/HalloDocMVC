using Common.Enum;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication.PgOutput.Messages;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Net.Mail;
using System.Net;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDashboardData dashboardData;
        private readonly IValidation validation;
        private readonly ICaseActions caseActions;
        private readonly HelloDocDbContext _context;
        private readonly IJwtRepository _jwtRepository;
        public AdminController(IDashboardData dashboardData , HelloDocDbContext context , ICaseActions caseActions , IValidation validation , IJwtRepository _jwtRepository) {
            this.dashboardData = dashboardData;
            _context = context;
            this.caseActions = caseActions;
            this.validation = validation;
            _jwtRepository = _jwtRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard() 
        {
            if(HttpContext.Session.GetString("AdminName") != null)
            {
            AdminDashboard obj = dashboardData.AllData();
            return View(obj);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        public IActionResult ActiveState()
        {
            AdminDashboard data = dashboardData.ActiveStateData();
            return View(data);
        }

        public IActionResult ConcludeState()
        {
            AdminDashboard data = dashboardData.ConcludeStateData();
            return View(data);
        }

        public IActionResult NewState(String status , String requesttype)
        {
                AdminDashboard data = dashboardData.NewStateData(status,requesttype);
                return View(data);
        }

        public IActionResult PendingState()
        {
            AdminDashboard data = dashboardData.PendingStateData();
            return View(data);
        }

        public IActionResult ToCloseState()
        {
            AdminDashboard data = dashboardData.ToCloseStateData();
            return View(data);
        }

        public IActionResult UnpaidState()
        {
            AdminDashboard data = dashboardData.UnpaidStateData();
            return View(data);
        }

        public IActionResult ViewCase(int requestId)
        {
            CaseDetails obj = dashboardData.ViewCaseData(requestId);
            return PartialView("_ViewCase" , obj);
        }

        public IActionResult ViewNotes(int requestId)
        {
            var requestData = _context.Requests.FirstOrDefault(a=>a.Requestid == requestId);
            var requestnote = _context.Requestnotes.FirstOrDefault(a=> a.Requestid == requestId);
            CaseDetails obj = new CaseDetails();
            if (requestnote != null)
            {
                obj.adminNote = requestnote.Adminnotes;
            }

            
            obj.requestId = requestId;
            var transferNote = _context.Requeststatuslogs.FirstOrDefault(a => a.Requestid == obj.requestId && a.Status == 2);
            if (transferNote != null)
            {
                var physicianName = _context.Physicians.FirstOrDefault(a=>a.Physicianid == transferNote.Physicianid).Firstname;
                obj.adminName = HttpContext.Session.GetString("AdminName");
                obj.physicianName = physicianName;
                obj.assignTime = transferNote.Createddate;
            }

            //bol jaldi bol ungh aave he

            //CaseDetails obj = dashboardData.ViewCaseData(requestId);
            return PartialView("_ViewNotes",obj);
        }

        public List<Physician> FilterData(int regionid)
        {
            List<Physician> physicianList= dashboardData.PhysicianList(regionid);
            return physicianList;
        }

        public IActionResult AssignCase(int requestId)
        {

            CaseActionsDetails obj =  caseActions.AssignCase(requestId);
            return PartialView("_AssignCase",obj);
        }
        public IActionResult SubmitAssign(CaseActionsDetails obj)
        {
            caseActions.SubmitAssign(obj);
            return RedirectToAction("AdminDashboard");
        }


        public IActionResult CancelCase(int requestId)
        {
            CaseActionsDetails obj = caseActions.CancelCase(requestId);
            return PartialView("_CancelCase", obj);
        }

        public IActionResult SubmitCancel(int requestId , int caseId , string cancelNote)
        {
            
            caseActions.SubmitCancel(requestId , caseId , cancelNote);
            return RedirectToAction("AdminDashboard");
        }


        public IActionResult BlockCase(int requestId)
        {
            CaseActionsDetails obj = caseActions.BlockCase(requestId);
            return PartialView("_BlockCase", obj);
        }

        public IActionResult SubmitBlock(int requestId, string blockNote)
        {

            caseActions.SubmitBlock(requestId, blockNote);
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult SubmitNotes(int requestId, string notes , CaseDetails obj)
        {
            caseActions.SubmitNotes(requestId, notes , obj);
            return RedirectToAction("AdminDashboard");
        }


        public IActionResult ViewUploads(int requestId)
        {
            CaseDetails obj = dashboardData.ViewUploads(requestId);
            return PartialView("_ViewUploads", obj);
        }

        public IActionResult UploadDocument(List<IFormFile> myfile , int reqid)
        {
            dashboardData.UplodingDocument(myfile, reqid);
            return RedirectToAction("ViewUploads", new { requestId = reqid });
        }

        public IActionResult SingleDelete(int reqfileid , int reqid)
        {
            dashboardData.SingleDelete(reqfileid);
            return RedirectToAction("ViewUploads", new { requestId = reqid });
        }

        [HttpPost]
        public IActionResult DeleteAll(List<int> reqwiseid, int reqid)
        {
            foreach (var obj in reqwiseid)
            {
                dashboardData.SingleDelete(obj);
            }
            return RedirectToAction("ViewUploads" , new {requestId = reqid});
        }

        public IActionResult AdminLogin()
        {

            return View();
        }

        public IActionResult AdminValidate(Aspnetuser obj)
        {
            try
            {
                var result = validation.AdminValidate(obj);
                TempData["Email"] = result.emailError;
                TempData["Password"] = result.passwordError;
                var check = _context.Aspnetusers.Where(u => u.Email == obj.Email).FirstOrDefault();
                var admindata = _context.Admins.Where(u => u.Aspnetuserid == check.Id).FirstOrDefault();
                HttpContext.Session.SetString("AdminName", admindata.Firstname);
                if (result.Status == ResponseStautsEnum.Success)
                {
                    TempData["success"] = "Login successfully";
                    HttpContext.Session.SetInt32("AdminId", admindata.Adminid);
                    return RedirectToAction("AdminDashboard", "Admin");
                }
                TempData["error"] = "Incorrect Email or password";
                return RedirectToAction("AdminLogin", "Admin");
            }
            catch (Exception ex)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminId");
            HttpContext.Session.Remove("AdminName");
            return RedirectToAction("AdminLogin", "Admin");
        }

        public IActionResult SendMail(List<int> reqwiseid, int reqid)
        {

            List<string> filenames = new List<string>();
            foreach (var item in reqwiseid)
            {
                var file = _context.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == item).Filename;
                filenames.Add(file);
            }
          
            Sendemail("vinit2273@gmail.com", "Your Attachments", "Please Find Your Attachments Here", filenames);
            return PartialView("_ViewDocument", reqid);

        }
        public async Task Sendemail(string email, string subject, string message, List<string> attachmentPaths)
        {
            try
            {
                var mail = "tatva.dotnet.vinitpatel@outlook.com";
                var password = "016@ldce";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Set to true if your message contains HTML
                };

                mailMessage.To.Add(email);

                foreach (var attachmentPath in attachmentPaths)
                {
                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        var attachment = new Attachment(attachmentPath);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                await client.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
