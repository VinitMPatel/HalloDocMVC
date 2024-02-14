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

        public IActionResult PatientDashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                patient_dashboard dash = new patient_dashboard();
                int id = (int)HttpContext.Session.GetInt32("UserId");
                var userdata = _context.Users.FirstOrDefault(m => m.Userid == id);
                dash.user = userdata;
                TempData["User"] = userdata.Firstname;
               
                var req = from m in _context.Requests
                              where m.Userid == id
                              select m;
                
                dash.request = req.ToList();
                return View(dash);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
           
        }

        public IActionResult Logout() { 
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("patient_login","Home");
        }

        public IActionResult Editing(patient_dashboard r)
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");
            var userdata = _context.Users.FirstOrDefault(m => m.Userid == id);
            userdata.Firstname = r.user.Firstname;
            userdata.Lastname = r.user.Lastname;
            userdata.Street = r.user.Street;
            userdata.City = r.user.City;
            userdata.State = r.user.State;
            userdata.Zip = r.user.Zip;
            userdata.Modifieddate = DateTime.Now;

            _context.Users.Update(userdata);
            _context.SaveChanges();

            return RedirectToAction("patientDashboard","patient");
        }
    }
}
