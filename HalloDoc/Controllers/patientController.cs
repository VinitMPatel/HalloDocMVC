using Services.ViewModels;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using HalloDoc.ViewModels;
using Services.Implementation;
using Common.Enum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace HalloDoc.Controllers
{
    public class patientController : Controller
    {

        private readonly HelloDocDbContext _context;
        private readonly IPatientRequest patientRequest;
        private readonly IValidation validation;

        public patientController(HelloDocDbContext context , IPatientRequest patientRequest , IValidation validation)
        {
            _context = context;
            this.patientRequest = patientRequest;
            this.validation = validation;
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

        public IActionResult Insert(PatientInfo r)
        {
            patientRequest.Insert(r);
            return RedirectToAction("patient_login", "Home");
        }

        [Route("/Patient/patient_request/checkmail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.Aspnetusers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }

        [HttpPost]
        public IActionResult patient_login(Aspnetuser aspnetuser)
        {
            var result = validation.Validate(aspnetuser);
            var check = _context.Aspnetusers.Where(u => u.Email == aspnetuser.Email).FirstOrDefault();
            var userdata = _context.Users.Where(u => u.Aspnetuserid == check.Id).FirstOrDefault();
            if (result.Status == ResponseStautsEnum.Success)
            {
                HttpContext.Session.SetInt32("UserId", userdata.Userid);
                return RedirectToAction("PatientDashboard", "patient");
            }
            return RedirectToAction("patient_login");
        }
    }
}
