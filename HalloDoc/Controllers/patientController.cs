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
using Common.Helper;

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
        public IActionResult PatientLogin(LoginPerson obj)
        {
            try
            {
                (PatientLogin result, LoggedInPersonViewModel loggedInPerson) = validation.Validate(obj);

                TempData["Email"] = result.emailError;
                TempData["Password"] = result.passwordError;
                if(loggedInPerson.role == "3")
                {
                    if (result.Status == ResponseStautsEnum.Success)
                    {
                        Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPerson));
                        HttpContext.Session.SetString("UserName", loggedInPerson.username);
                        HttpContext.Session.SetString("aspNetUserId", loggedInPerson.aspuserid);
                        TempData["success"] = "Login successfully";
                        return RedirectToAction("PatientDashboard", "patient");
                    }

                    TempData["error"] = "Incorrect Email or password";
                    return RedirectToAction("PatientLogin", "Home");
                }
                else
                {
                    TempData["warning"] = "*Please use Patirent Email.";
                    return RedirectToAction("PatientLogin", "Home");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("PatientLogin", "Home");
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
                return RedirectToAction("PatientLogin","Home");
            }
        }

        public async Task<IActionResult> Editing(patient_dashboard obj)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            string temp =  await dashboard.EditProfile(obj, aspNetUserId);
            HttpContext.Session.SetString("UserName", temp);
            TempData["success"] = "Data updated successfully.";
            return RedirectToAction("patientdashboard", "patient");
        }

        public IActionResult NewRequest()
        {
            var radio = Request.Form["flexRadioDefault"].ToList();
     
            if(radio.Count() > 0)
            {
                if (radio.ElementAt(0) == "Me")
                {
                    return RedirectToAction("RequestForSelf");
                }
                else if (radio.ElementAt(0) == "else")
                {
                    return RedirectToAction("RequestForElse");
                }
            }
            return NoContent();
        }

        public async Task<IActionResult> RequestForSelf()
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            PatientInfo model = await dashboard.RequestForSelfData(aspNetUserId);
            return View(model);
        }

        public async Task<IActionResult> RequestForElse()
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            FamilyFriendRequest model = await dashboard.RequestForElseData(aspNetUserId);
            return View(model);
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
            return RedirectToAction("PatientLogin","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Insert(PatientInfo r)
        {
            await patientRequest.Insert(r);
            TempData["success"] = "Request Created successfully.";
            return RedirectToAction("PatientLogin", "Home");
        }

        [Route("/Patient/patient_request/checkmail/{email}")]
        [HttpGet]
        public async Task<bool> CheckEmail(string email)
        {
            User user = await patientRequest.CheckEmail(email);

            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public IActionResult SubmitDocument(patient_dashboard obj)
        {
            dashboard.UplodingDocument(obj,obj.reqId);
            return RedirectToAction("ViewDocument", new { id = obj.reqId });
        }

        public async Task<IActionResult> NewAccount(LoginPerson model)
        {
            await patientRequest.NewAccount(model);
            return RedirectToAction("PatientLogin", "Home");
        }

        public async Task AgreeAgreement(int requestId)
        {
            await caseActions.AgreeAgreement(requestId);
        }

        public async Task CancelAgreement(int requestId)
        {
             await caseActions.CancelAgreement(requestId);
        }
    }
}
