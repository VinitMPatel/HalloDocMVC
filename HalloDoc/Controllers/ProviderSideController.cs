using Common.Helper;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;
using Authorization = Services.Implementation.Authorization;

namespace HalloDoc.Controllers
{
    [Authorization("2")]
    public class ProviderSideController : Controller
    {

        private readonly IProviderSideServices providerSideServices;
        private readonly IDashboardData dashboardData;
        private readonly ICaseActions caseActions;
        private readonly HalloDocDbContext _context;


        public ProviderSideController(IProviderSideServices providerSideServices, IDashboardData dashboardData, ICaseActions caseActions, HalloDocDbContext context)
        {
            this.providerSideServices = providerSideServices;
            this.dashboardData = dashboardData;
            this.caseActions = caseActions;
            context = _context;
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

        public async Task AcceptCase(int requestId)
        {
            await providerSideServices.AcceptCase(requestId);
            TempData["success"] = "Case accepted.";
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

        public async Task SubmitTransferRequest(int requestId, string note)
        {
            await providerSideServices.SubmitTransferReqquest(requestId, note);
            TempData["success"] = "Case transferred successfully";
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
            TempData["success"] = "Data saved successfully.";
            return RedirectToAction("ProviderDashboard");
        }
    }
}
