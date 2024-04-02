using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;

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
        public IActionResult ToStopNotification(List<int> toStopNotification, List<int> toNotification)
        {
            dashboardData.ToStopNotification(toStopNotification, toNotification);
            return RedirectToAction("Provider");
        }

        public IActionResult EditProvider(int physicianId) { 
            EditProviderViewModel obj = dashboardData.EditProvider(physicianId);
            return View(obj);
        }

        
        public void UpdatePhysicianInfo(EditProviderViewModel obj , List<int> selectedRegion)
        {
            dashboardData.UpdatePhysicianInfo(obj, selectedRegion);
            //return RedirectToAction("EditProvider", new { physicianId = obj.providerId });
            //return NoContent();
        }
        
        public void UpdateBillingInfo(EditProviderViewModel obj)
        {
            dashboardData.UpdateBillingInfo(obj);
        }

        public void UpdateProfile(EditProviderViewModel obj)
        {
            dashboardData.UpdateProfile(obj);
        }

        public IActionResult CreateProvider()
        {
            EditProviderViewModel obj = providerServices.CreateProvider();
            return View(obj);
        }
    }
}
