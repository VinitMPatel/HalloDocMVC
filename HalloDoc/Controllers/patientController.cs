using Services.ViewModels;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using Services.Implementation;
using Common.Enum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Runtime.CompilerServices;
using NPOI.SS.Formula.Functions;

namespace HalloDoc.Controllers
{
    public class patientController : Controller
    {

        private readonly HalloDocDbContext _context;
        private readonly IPatientRequest patientRequest;
        private readonly IValidation validation;
        private readonly IDashboard dashboard;
        private readonly IEmailSender emailSender;
        private readonly ICaseActions caseActions;
        private readonly IJwtRepository _jwtRepository;

        public patientController(HalloDocDbContext context , IPatientRequest patientRequest, IValidation validation, IDashboard dashboard, IEmailSender emailSender, ICaseActions caseActions, IJwtRepository jwtRepository)
        {
            _context = context;
            this.patientRequest = patientRequest;
            this.validation = validation;
            this.dashboard = dashboard;
            this.emailSender = emailSender;
            this.caseActions = caseActions;
            _jwtRepository = jwtRepository;
        }

        [HttpPost]
        public IActionResult patient_login(LoginPerson obj)
        {
            try
            {
                (PatientLogin result, LoggedInPersonViewModel loggedInPerson) = validation.Validate(obj);

                TempData["Email"] = result.emailError;
                TempData["Password"] = result.passwordError;

                if (result.Status == ResponseStautsEnum.Success)
                {
                    Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPerson));
                    HttpContext.Session.SetString("UserName", loggedInPerson.username);
                    HttpContext.Session.SetString("aspNetUserId", loggedInPerson.aspuserid);
                    TempData["success"] = "Login successfully";
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

        public async Task<IActionResult> PatientDashboard()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
                return View(await dashboard.PatientDashboard(aspNetUserId));
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        public async Task<IActionResult> editing(patient_dashboard r)
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");            
            String temp =  await dashboard.editing(r, id);
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

        public async Task<IActionResult> ViewDocument(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
                return View(await dashboard.ViewDocuments(aspNetUserId, id));
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
        public async Task<IActionResult> Insert(PatientInfo r)
        {
            await patientRequest.Insert(r);
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

        public IActionResult NewAccount(LoginPerson model)
        {
            patientRequest.NewAccount(model);

            return RedirectToAction("patient_login", "Home");
        }

        public async Task AgreeAgreement(int requestId)
        {
            await caseActions.AgreeAgreement(requestId);
        }

        public async Task CancelAgreement(int requestId)
        {
             await caseActions.CancelAgreement(requestId);
        }

        public async Task<bool> EmailValidate(string Email)
        {
            Aspnetuser aspnetuser = await patientRequest.CheckEmail(Email);

            if (aspnetuser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
