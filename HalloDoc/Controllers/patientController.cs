using HalloDoc.DataContext;
using HalloDoc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class patientController : Controller
    {

        private readonly HelloDocDbContext _context;

        public patientController(HelloDocDbContext context)
        {
            _context = context;
        }

        public IActionResult PatientDashboard(int id)
        {
            patient_dashboard dash = new patient_dashboard();

            var userdata = _context.Users.FirstOrDefault(m => m.Userid == id);
            TempData["User"] = userdata.Firstname;
            dash.user = userdata;
            var request = from m in _context.Requests
                          where m.Userid == id
                          select m;
            dash.request = request.ToList();
            return View(dash);
        }
    }
}
