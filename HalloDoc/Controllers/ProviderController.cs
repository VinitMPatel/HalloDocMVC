using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System.Runtime.CompilerServices;

namespace HalloDoc.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IDashboardData dashboardData;
        private readonly IProviderServices providerServices;
        private readonly HalloDocDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;


        public ProviderController(IDashboardData dashboardData, HalloDocDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment env , IProviderServices providerServices)
        {
            this.dashboardData = dashboardData;
            _context = context;
            _env = env;
            this.providerServices = providerServices;
        }
        public IActionResult Provider()
        {
            ProviderViewModel obj = new ProviderViewModel();
            obj.regionlist = _context.Regions.ToList();
            return View(obj);
        }

        public IActionResult ProviderTable(int regionId)
        {
            ProviderViewModel obj = dashboardData.ProviderData(regionId);
            return PartialView("Provider/_ProviderTable", obj);
        }

        [HttpPost]
        public async Task<IActionResult> ToStopNotification(List<int> toStopNotification, List<int> toNotification)
        {
            await dashboardData.ToStopNotification(toStopNotification, toNotification);
            return RedirectToAction("Provider");
        }

        public IActionResult EditProvider(int physicianId) { 
            EditProviderViewModel obj = dashboardData.EditProvider(physicianId);
            return View(obj);
        }

        
        public async Task UpdatePhysicianInfo(EditProviderViewModel obj , List<int> selectedRegion)
        {
            await dashboardData.UpdatePhysicianInfo(obj, selectedRegion);
            //return RedirectToAction("EditProvider", new { physicianId = obj.providerId });
            //return NoContent();
        }
        
        public async Task UpdateBillingInfo(EditProviderViewModel obj)
        {
            await dashboardData.UpdateBillingInfo(obj);
        }

        public async Task UpdateProfile(EditProviderViewModel obj)
        {
            await dashboardData.UpdateProfile(obj);
        }

        public async Task<IActionResult> CreateProvider()
        {
            EditProviderViewModel obj = await providerServices.CreateProvider();
            return View(obj);
        }

        public async Task<IActionResult> SubmitCreateProvider(EditProviderViewModel obj , List<int> selectedRegion)
        {
            int adminId = (int)HttpContext.Session.GetInt32("AdminId");
            await providerServices.CreateProviderAccount(obj, selectedRegion,adminId);
            return RedirectToAction("Provider");
        }
    }
}
