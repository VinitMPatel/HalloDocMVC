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
        private readonly IProviderServices providerServices;

        public ProviderController(IProviderServices providerServices)
        {
            this.providerServices = providerServices;
        }

        public async Task<IActionResult> Provider()
        {
            ProviderViewModel obj = new ProviderViewModel();
            obj.regionlist = await providerServices.GetRegions();
            return View(obj);
        }

        public IActionResult ProviderLocation()
        {
            return View();
        }

        public async Task<IActionResult> ProviderTable(int regionId)
        {
            ProviderViewModel obj = await providerServices.ProviderData(regionId);
            return PartialView("Provider/_ProviderTable", obj);
        }

        [HttpPost]
        public async Task<IActionResult> ToStopNotification(List<int> toStopNotification, List<int> toNotification)
        {
            await providerServices.ToStopNotification(toStopNotification, toNotification);
            return RedirectToAction("Provider");
        }

        public IActionResult EditProvider(int physicianId) { 
            EditProviderViewModel obj = providerServices.EditProvider(physicianId);
            return View(obj);
        }
        
        public async Task UpdatePhysicianInfo(EditProviderViewModel obj , List<int> selectedRegion)
        {
            await providerServices.UpdatePhysicianInfo(obj, selectedRegion);
            //return RedirectToAction("EditProvider", new { physicianId = obj.providerId });
            //return NoContent();
        }
        
        public async Task UpdateBillingInfo(EditProviderViewModel obj)
        {
            await providerServices.UpdateBillingInfo(obj);
        }

        public async Task UpdateProfile(EditProviderViewModel obj)
        {
            await providerServices.UpdateProfile(obj);
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

        public async Task<string> GetLocationS()
        {
            string LocationData = await providerServices.GetLocations();
            return LocationData;
        }
    }
}
