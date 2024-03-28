using Data.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IDashboardData dashboardData;
        private readonly IValidation validation;
        private readonly ICaseActions caseActions;
        private readonly HalloDocDbContext _context;
        private readonly IJwtRepository _jwtRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;


        public ProviderController(IDashboardData dashboardData, HalloDocDbContext context, ICaseActions caseActions, IValidation validation, IJwtRepository jwtRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.dashboardData = dashboardData;
            _context = context;
            this.caseActions = caseActions;
            this.validation = validation;
            _jwtRepository = jwtRepository;
            _env = env;
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
    }
}
