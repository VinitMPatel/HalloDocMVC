using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDashboardData dashboardData;
        public AdminController(IDashboardData dashboardData) {
            this.dashboardData = dashboardData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard() 
        {
            AdminDashboard obj = dashboardData.AllData();
            return View(obj);
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

        public IActionResult NewState()
        {
            AdminDashboard data = dashboardData.NewStateData();
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
    }
}
