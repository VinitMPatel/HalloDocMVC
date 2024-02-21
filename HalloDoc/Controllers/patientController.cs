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
        private readonly IDashboard dashboard;
        private readonly IEmailSender emailSender;

        public patientController(HelloDocDbContext context , IPatientRequest patientRequest , IValidation validation , IDashboard dashboard, IEmailSender emailSender)
        {
            _context = context;
            this.patientRequest = patientRequest;
            this.validation = validation;
            this.dashboard = dashboard;
            this.emailSender = emailSender;
        }

        [HttpPost]
        public IActionResult patient_login(Aspnetuser aspnetuser)
        {
            try
            {
                var result = validation.Validate(aspnetuser);
                TempData["Email"] = result.emailError;
                TempData["Password"] = result.passwordError;
                var check = _context.Aspnetusers.Where(u => u.Email == aspnetuser.Email).FirstOrDefault();
                var userdata = _context.Users.Where(u => u.Aspnetuserid == check.Id).FirstOrDefault();
                HttpContext.Session.SetString("UserName", userdata.Firstname);
                if (result.Status == ResponseStautsEnum.Success)
                {
                    TempData["success"] = "Login successfully";
                    HttpContext.Session.SetInt32("UserId", userdata.Userid);
                    return RedirectToAction("PatientDashboard", "patient");
                }
                TempData["error"] = "Incorrect Email or password";
                return RedirectToAction("patient_login", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("patient_login", "Home");
            }
        }

        public IActionResult PatientDashboard()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserId");
                return View(dashboard.PatientDashboard(id));
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        public IActionResult editing(patient_dashboard r)
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");            
            String temp =  dashboard.editing(r, id);
            HttpContext.Session.SetString("UserName", temp);
            return RedirectToAction("patientdashboard", "patient");
        }

        public IActionResult NewRequest()
        {
            var radio = Request.Form["flexRadioDefault"].ToList();
            if (radio.ElementAt(0) == "Me")
            {
                return RedirectToAction("RequestForSelf");
            }
            else if (radio.ElementAt(0) == "else")
            {
                return RedirectToAction("RequestForElse");
            }
            return NoContent();
        }

        public IActionResult RequestForSelf()
        {
            return View();
        }

        public IActionResult RequestForElse()
        {
            return View();
        }

        public IActionResult ViewDocument(int id)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var userId = (int)HttpContext.Session.GetInt32("UserId");
                return View(dashboard.ViewDocuments(userId , id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout() {     
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("patient_login","Home");
        }

        [HttpPost]
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
        public IActionResult SubmitDocument(patient_dashboard obj)
        {
            
            dashboard.UplodingDocument(obj,obj.reqId);
            return RedirectToAction("ViewDocument", new { id = obj.reqId });
            
        }
        public void SendingMail()
        {
            //_commonRepository.Insert(user);
            //var result = _commonRepository.SaveChanges();
            //emailSender.SendEmail("vmpatel2273@gmail.com", "User Regitered");
            
            emailSender.SendEmail("vinit.patel@etatvasoft.com", "Hello", "Please <a href=\"https://localhost:7021/PatientSite/PatientLogin\">login</a>");
            //if (result > 0)
            //{
            //return CreatedAtAction("GetUser", new { id = user.id }, user);
            //}
            //return BadRequest();
        }
    }
}
