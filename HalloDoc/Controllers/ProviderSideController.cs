using Data.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;

namespace HalloDoc.Controllers
{
    public class ProviderSideController : Controller
    {

        private readonly IProviderSideServices providerSideServices;
        private readonly IDashboardData dashboardData;

        public ProviderSideController(IProviderSideServices providerSideServices , IDashboardData dashboardData)
        {
            this.providerSideServices = providerSideServices;
            this.dashboardData = dashboardData;
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
                return RedirectToAction("AdminLogin","Admin");
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

        public async Task<IActionResult> TransferRequest(int requestId)
        {
            ProviderCaseAction providerCaseAction = new ProviderCaseAction();
            providerCaseAction.requestId = requestId;
            return PartialView("AdminCaseAction/_TransferRequest" , providerCaseAction);
        }
    }
}
