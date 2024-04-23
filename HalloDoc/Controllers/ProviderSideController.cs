using Common.Helper;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Collections;
using System.Net.Mail;
using System.Net;
using Authorization = Services.Implementation.Authorization;

namespace HalloDoc.Controllers
{
    [Authorization("2")]
    public class ProviderSideController : Controller
    {

        private readonly IProviderSideServices providerSideServices;
        private readonly IDashboardData dashboardData;
        private readonly ICaseActions caseActions;
        private readonly IProviderServices providerServices;

        public ProviderSideController(IProviderSideServices providerSideServices, IDashboardData dashboardData, ICaseActions caseActions , IProviderServices providerServices)
        {
            this.providerSideServices = providerSideServices;
            this.dashboardData = dashboardData;
            this.caseActions = caseActions;
            this.providerServices = providerServices;
        }

        public async Task<IActionResult> AllState(ProviderDashboard obj)
        {
            string? aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            obj.aspNetUserId = aspNetUserId!;
            ProviderDashboard data = await providerSideServices.AllStateData(obj);
            switch (obj.requeststatus)
            {
                case 1:
                    return View("ProviderNewState", data);
                case 2:
                    return View("ProviderPendingState", data);
                case 4:
                    return View("ProviderActiveState", data);
                case 5:
                    return View("ProviderActiveState", data);
                case 6:
                    return View("ProviderConcludeState", data);
                default:
                    return View("ProviderNewState", data);
            }
        }

        public async Task<IActionResult> ProviderDashboard()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                string? aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
                ProviderDashboard obj = await providerSideServices.AllData(aspNetUserId!);
                return View(obj);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("aspNetUserId");
            HttpContext.Session.Remove("UserName");
            Response.Cookies.Delete("jwt");
            return RedirectToAction("AdminLogin", "Admin");
        }

        public async Task<IActionResult> AcceptCase(int requestId)
        {
            await providerSideServices.AcceptCase(requestId);
            TempData["success"] = "Case accepted.";
            return RedirectToAction("ProviderDashboard");
        }


        public async Task<IActionResult> ViewCase(int requestId)
        {
            CaseActionDetails obj = await dashboardData.ViewCaseData(requestId);
            return PartialView("AdminCaseAction/_ViewCase", obj);
        }

        public async Task<IActionResult> ViewNotes(int requestId)
        {
            CaseActionDetails obj = await dashboardData.ViewNotes(requestId);
            return PartialView("AdminCaseAction/_ViewNotes", obj);
        }
        public async Task SubmitNotes(int requestId, string notes, CaseActionDetails obj)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            string role = HttpContext.Session.GetString("Role")!;
            await caseActions.SubmitNotes(requestId, notes, obj, aspNetUserId, role);
        }


        public async Task<IActionResult> Agreement(int requestId)
        {
            AgreementDetails obj = await caseActions.Agreement(requestId);
            obj.requestId = requestId;
            return PartialView("AdminCaseAction/_Agreement", obj);
        }
        public async Task SendAgreement(int requestId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var encryptReqId = EncryptDecryptHelper.Encrypt(requestId.ToString());
            string resetPasswordPath = Url.Action("ViewAgreement", "Home", new { requestId = encryptReqId });
            string url = baseUrl + resetPasswordPath;
            await caseActions.SendingAgreement(requestId, url);
            TempData["success"] = "Email sent successfullt.";
        }


        public async Task<IActionResult> ViewUploads(int requestId)
        {
            CaseActionDetails obj = await dashboardData.ViewUploads(requestId);
            return PartialView("AdminCaseAction/_ViewUploads", obj);
        }
        public async Task<IActionResult> UploadDocument(List<IFormFile> myfile, int reqid)
        {
            await dashboardData.UplodingDocument(myfile, reqid);
            return RedirectToAction("ViewUploads", new { requestId = reqid });
        }
        public async Task<IActionResult> SingleDelete(int reqfileid, int reqid)
        {
            await dashboardData.SingleDelete(reqfileid);
            return RedirectToAction("ViewUploads", new { requestId = reqid });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll(List<int> reqwiseid, int reqid)
        {
            foreach (var obj in reqwiseid)
            {
                await dashboardData.SingleDelete(obj);
            }
            return RedirectToAction("ViewUploads", new { requestId = reqid });
        }

        public IActionResult TransferRequest(int requestId)
        {
            ProviderCaseAction providerCaseAction = new ProviderCaseAction();
            providerCaseAction.requestId = requestId;
            return PartialView("AdminCaseAction/_TransferRequest", providerCaseAction);
        }

        public async Task<IActionResult> SubmitTransferRequest(int requestId, string note)
        {
            await providerSideServices.SubmitTransferReqquest(requestId, note);
            return RedirectToAction("ProviderDashboard");
        }

        public async Task<IActionResult> Orders(int requestId)
        {
            Orders obj = new Orders();
            obj.requestId = requestId;
            return PartialView("AdminCaseAction/_Orders", obj);
        }

        public async Task<List<Healthprofessionaltype>> GetProfessions()
        {
            return await dashboardData.GetProfessions();
        }

        public async Task<List<Healthprofessional>> GetBusinesses(int professionId)
        {
            return await dashboardData.GetBusinesses(professionId);
        }

        public async Task<Healthprofessional> GetBusinessesDetails(int businessid)
        {
            return await dashboardData.GetBusinessesDetails(businessid);
        }

        [HttpPost]
        public async Task SubmitOrder(Orders obj)
        {
            obj.createdby = HttpContext.Session.GetString("aspNetUserId")!;
            await caseActions.SubmitOrder(obj);
            TempData["success"] = "*Order Sent successfully.";
        }

        public async Task<IActionResult> SubmitEncounterStatus()
        {
            var radio = Request.Form["flexRadioDefault"].ToList();
            var requestId = Request.Form["requestId"].ToList();
            if (radio.ElementAt(0) == "HouseCall")
            {
                await providerSideServices.HouseCall(int.Parse(requestId.ElementAt(0)!));
                TempData["success"] = "*Data is saved";
                return RedirectToAction("ProviderDashboard");
            }
            else if (radio.ElementAt(0) == "Consult")
            {
                await providerSideServices.Consult(int.Parse(requestId.ElementAt(0)!));
                TempData["success"] = "*Data is saved";
                return RedirectToAction("ProviderDashboard");
            }
            return NoContent();
        }

        public async Task<IActionResult> HouseCallClick(int requestId)
        {
            await providerSideServices.HouseCalling(requestId);
            return RedirectToAction("ProviderDashboard");
        }

        public async Task<IActionResult> EncounterForm(int requestId)
        {
            EncounterFormViewModel obj = new EncounterFormViewModel();
            obj = await providerSideServices.EncounterForm(requestId);
            obj.RequestId = requestId;
            return View(obj);
        }

        public async Task<IActionResult> SubmitEncounterForm(EncounterFormViewModel obj)
        {
            await providerSideServices.SubmitEncounterForm(obj);
            return NoContent() ;
        }

        public async Task FinalizeEncounter(int requestId)
        {
            await providerSideServices.FinalizeEncounter(requestId);
            TempData["success"] = "*Form finalized successfully.";
        }

        public async Task<IActionResult> ConcludeCare(int requestId)
        {
            ConcludeCare obj = await providerSideServices.ConcludeCareView(requestId);
            return PartialView("AdminCaseAction/_ConcludeCare", obj);
        }

        public async Task<IActionResult> UploadFiles(List<IFormFile> files, int requestId)
        {
            await providerSideServices.uploadFiles(files, requestId);
            return RedirectToAction("ConcludeCare",new { requestId = requestId });
            
        }

        public async Task<IActionResult> ProviderProfile()
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            EditProviderViewModel obj = await providerServices.EditProvider(aspNetUserId);
            return View(obj);
        }

        public IActionResult CreateRequest()
        {
            return PartialView("AdminCaseAction/_CreateRequest");
        }

        public async Task<IActionResult> ProviderSchedule()
        {
            return View(await providerSideServices.Scheduling());
        }

        public async Task<IActionResult> LoadSchedulingPartial(string date)
        {
            var currentDate = DateTime.Parse(date);
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            return PartialView("Provider/_MonthWise", await providerSideServices.Monthwise(currentDate, aspNetUserId));
        }

        public void SendMailForRequest(string firstName, string email)
        {
            var mail = "tatva.dotnet.vinitpatel@outlook.com";
            var password = "016@ldce";

            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string createRequestPath = Url.Action("PatientRequestScreen", "Home");
            string mainURL = baseUrl + createRequestPath;


            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = "Create request",
                Body = " Hello " + firstName + " , You can submit request using this link : " + mainURL,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);
            client.SendMailAsync(mailMessage);
        }

        public async Task ConcludeCareCase(int requestId , string note)
        {
            await providerSideServices.ConcludeCareCase(requestId , note);
            TempData["success"] = "Case concluded successfully.";
        }

        public async Task<IActionResult> DownloadEncounter(int requestId)
        {
            return await providerSideServices.DownloadEncounter(requestId);
        }
    }
}
