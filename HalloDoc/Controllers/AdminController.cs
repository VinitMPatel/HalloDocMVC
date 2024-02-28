using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDashboardData dashboardData;
        private readonly HelloDocDbContext _context;
        public AdminController(IDashboardData dashboardData , HelloDocDbContext context) {
            this.dashboardData = dashboardData;
            _context = context;
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
            ViewCase obj = dashboardData.ViewCaseData(requestId);
            return View(obj);
        }
    }
}
