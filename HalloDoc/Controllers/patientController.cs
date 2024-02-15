using Services.ViewModels;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
            if (HttpContext.Session.GetString("UserId") != null)
            {
                patient_dashboard dash = new patient_dashboard();

                var id = HttpContext.Session.GetInt32("UserId");
                var userdata = _context.Users.Where(u => u.Userid == id).FirstOrDefault();
                var req = from m in _context.Requests where m.Userid == id select m;

                dash.user = userdata;
                TempData["User"] = userdata.Firstname + " "+userdata.Lastname;
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

         public IActionResult editing(patient_dashboard r)
         {
             int id = (int)HttpContext.Session.GetInt32("userid");
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

             return RedirectToAction("patientdashboard","patient");
         }

        public IActionResult temp() { 
            return View();
            
        }
    }
}
