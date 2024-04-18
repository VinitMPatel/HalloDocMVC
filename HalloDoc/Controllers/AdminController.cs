using Common.Enum;
using Data.DataContext;
using Data.Entity;
using Common.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication.PgOutput.Messages;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Net.Mail;
using System.Net;
using Authorization = Services.Implementation.Authorization;
using System.Web.Helpers;
using System.Security.Policy;
using Org.BouncyCastle.Asn1.Ocsp;
using NPOI.SS.Formula.Functions;
using Microsoft.AspNetCore.Authorization;

namespace HalloDoc.Controllers
{

    [Authorization("1")]
    public class AdminController : Controller
    {
        private readonly IDashboardData dashboardData;
        private readonly IValidation validation;
        private readonly ICaseActions caseActions;
        private readonly HalloDocDbContext _context;
        private readonly IJwtRepository _jwtRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public AdminController(IDashboardData dashboardData, HalloDocDbContext context, ICaseActions caseActions, IValidation validation, IJwtRepository jwtRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.dashboardData = dashboardData;
            _context = context;
            this.caseActions = caseActions;
            this.validation = validation;
            _jwtRepository = jwtRepository;
            _env = env;
        }

        [AllowAnonymous]
        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult ProviderLocation()
        {
            return View();
        }

    
        public async Task<IActionResult> AdminDashboard()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                AdminDashboard obj = await dashboardData.AllData();
                return View(obj);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

      
        public async Task<IActionResult> AllState(AdminDashboard obj)
        {
            AdminDashboard data =  await dashboardData.AllStateData(obj);
            switch (obj.requeststatus)
            {
                case 1:
                    return View("NewState", data);
                    break;
                case 2:
                    return View("PendingState", data);
                    break;
                case 3:
                    return View("ToCloseState", data);
                    break;
                case 4:
                    return View("ActiveState", data);
                    break;
                case 5:
                    return View("ActiveState", data);
                    break;
                case 6:
                    return View("ConcludeState", data);
                    break;
                case 7:
                    return View("ToCloseState", data);
                    break;
                case 8:
                    return View("ToCloseState", data);
                    break;
                case 9:
                    return View("UnpaidState", data);
                    break;
                default: return View();
            }
        }

        public async Task<IActionResult> ExportData(AdminDashboard obj)
        {
            AdminDashboard data = await dashboardData.AllStateData(obj);
            var record = dashboardData.DownloadExcle(data);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"{obj.requeststatus}_{strDate}.xlsx";
            return File(record, contentType, filename);
        }

        public async Task<IActionResult> ExportAllData()
        {
            AdminDashboard data = await dashboardData.AllData();
            var record = dashboardData.DownloadExcle(data);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = "All data " + strDate +".xlsx";
            return File(record, contentType, filename);
        }

        
       
        public async Task<IActionResult> ViewCase(int requestId)
        {
            CaseActionDetails obj = await dashboardData.ViewCaseData(requestId);
            return PartialView("AdminCaseAction/_ViewCase", obj);
        }

        public async Task<IActionResult> ViewNotes(int requestId)
        {
            CaseActionDetails obj = await dashboardData.ViewNotes(requestId);
            return PartialView("AdminCaseAction/_ViewNotes", obj);
        }

        public async Task<List<Physician>> FilterData(int regionid)
        {
            List<Data.Entity.Physician> physicianList = await dashboardData.PhysicianList(regionid);
            return physicianList;
        }


        public async Task<IActionResult> AssignCase(int requestId)
        {
            
            Services.ViewModels.CaseActions obj = await caseActions.AssignCase(requestId);
            return PartialView("AdminCaseAction/_AssignCase", obj);
        }
        public async Task SubmitAssign(int requestId, int physicianId, string assignNote)
        {   
            await caseActions.SubmitAssign(requestId, physicianId, assignNote);
        }



        public async Task<IActionResult> CancelCase(int requestId)
        {
            Services.ViewModels.CaseActions obj = await caseActions.CancelCase(requestId);
            return PartialView("AdminCaseAction/_CancelCase", obj);
        }
        public async Task<IActionResult> SubmitCancel(int requestId, int caseId, string cancelNote)
        {
            await caseActions.SubmitCancel(requestId, caseId, cancelNote);
            return RedirectToAction("AdminDashboard");
        }



        public async Task<IActionResult> BlockCase(int requestId)
        {
            Services.ViewModels.CaseActions obj = await caseActions.BlockCase(requestId);
            return PartialView("AdminCaseAction/_BlockCase", obj);
        }
        public async Task<IActionResult> SubmitBlock(int requestId, string blockNote)
        {
            await caseActions.SubmitBlock(requestId, blockNote);
            return RedirectToAction("AdminDashboard");
        }


        public async Task SubmitNotes(int requestId, string notes, CaseActionDetails obj)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            string role = HttpContext.Session.GetString("Role")!;
            await caseActions.SubmitNotes(requestId, notes, obj , aspNetUserId , role);
           
        }


        public async Task<IActionResult> ViewUploads(int requestId)
        {
            CaseActionDetails obj = await dashboardData.ViewUploads(requestId);
            return PartialView("AdminCaseAction/_ViewUploads", obj);
        }
        public async Task<IActionResult> UploadDocument(List<IFormFile> myfile, int reqid)
        {
            await dashboardData.UplodingDocument(myfile, reqid);
            return RedirectToAction("ViewUploads", new { requestId = reqid });
        }
        public async Task<IActionResult> SingleDelete(int reqfileid, int reqid)
        {
            await dashboardData.SingleDelete(reqfileid);
            return RedirectToAction("ViewUploads", new { requestId = reqid });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll(List<int> reqwiseid, int reqid)
        {
            foreach (var obj in reqwiseid)
            {
                await dashboardData.SingleDelete(obj);
            }
            return RedirectToAction("ViewUploads", new { requestId = reqid });
        }


        public async Task<IActionResult> Orders(int requestId)
        {
            Orders obj = new Orders();
            obj.requestId = requestId;
            return PartialView("AdminCaseAction/_Orders", obj);
        }

        [HttpPost]
        public async Task SubmitOrder(Orders obj)
        {
            obj.createdby = HttpContext.Session.GetString("aspNetUserId");
            await caseActions.SubmitOrder(obj);
            TempData["success"] = "*Order Sent successfully.";
        }

        public async Task<List<Healthprofessionaltype>> GetProfessions()
        {
            return await dashboardData.GetProfessions();
        }

        public async Task<List<Healthprofessional>> GetBusinesses(int professionId)
        {
            return await dashboardData.GetBusinesses(professionId);
        }

        public async Task<Healthprofessional> GetBusinessesDetails(int businessid)
        {
            return await dashboardData.GetBusinessesDetails(businessid);
        }


        public async Task<IActionResult> TransferCase(int requestId)
        {
            Services.ViewModels.CaseActions obj = await caseActions.AssignCase(requestId);
            return PartialView("AdminCaseAction/_TransferCase", obj);
        }
        public async Task<IActionResult> SubmitTransfer(int requestId, int physicianId, string transferNote)
        {
            await caseActions.SubmitTransfer(requestId, physicianId, transferNote);
            return RedirectToAction("AdminDashboard");
        }



        public IActionResult ClearCase(int requestId)
        {
            Services.ViewModels.CaseActions obj = new Services.ViewModels.CaseActions();
            obj.requestId = requestId;
            return PartialView("AdminCaseAction/_ClearCase", obj);
        }
        public async Task<IActionResult> SubmitClearCase(int requestId)
        {
            await caseActions.SubmitClearCase(requestId);
            return RedirectToAction("AdminDashboard");
        }


        public async Task<IActionResult> Agreement(int requestId)
        {
            AgreementDetails obj = await caseActions.Agreement(requestId);
            obj.requestId = requestId;
            return PartialView("AdminCaseAction/_Agreement", obj);
        }
        public async Task SendAgreement(int requestId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var encryptReqId = EncryptDecryptHelper.Encrypt(requestId.ToString());
            string resetPasswordPath = Url.Action("ViewAgreement", "Home", new { requestId = encryptReqId });
            string url = baseUrl + resetPasswordPath;
            await caseActions.SendingAgreement(requestId, url);
        }


        public async Task<IActionResult> CloseCase(int requestId)
        {
            CloseCase obj = await caseActions.CloseCase(requestId);
            return PartialView("AdminCaseAction/_CloseCase", obj);
        }
        public async Task<IActionResult> CloseCaseChanges(string email , int requestId , string phone)
        {
            await caseActions.CloseCaseChanges(email , requestId , phone);
            return RedirectToAction("CloseCase", new {requestId = requestId});
        }

        public async Task<IActionResult> AdminProfile()
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            AdminProfile adminData = await dashboardData.AdminProfileData(aspNetUserId);
            return View(adminData);
        }

        public async Task AdminPasswordReset(string password)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            await dashboardData.UpdateAdminPassword(aspNetUserId, password);
        }

        public async Task UpdateAdminInfo(AdminInfo obj)
        {
           string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            await dashboardData.UpdateAdminInfo(aspNetUserId, obj);
        }

        public async Task UpdateBillingInfo(BillingInfo obj)
        {
           string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            await dashboardData.UpdateBillingInfo(aspNetUserId, obj);
        }

        public IActionResult RoleAccess()
        {
            RoleAccess obj = dashboardData.AddedRoles();
            return View(obj);
        }
        public IActionResult CreateRoleAccess()
        {
            return View();
        }

        public async Task<IActionResult> RolesList(int accountType , int roleId)
        {
            RoleAccess obj = await dashboardData.CreateAccessRole(accountType, roleId);
            return PartialView("AdminCaseAction/_RolesList", obj);
        }

        [HttpPost]
        public async Task AddNewRole(List<int> menus , short accountType, string roleName)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            await dashboardData.AddNewRole(menus , accountType , roleName , aspNetUserId);
        }

        public IActionResult EditRole(int roleId)
        {
            RoleAccess obj = dashboardData.EditRole(roleId);
            return PartialView("AdminCaseAction/_EditRole", obj);
        }

        //public async Task<IActionResult> UserAccess()
        //{

        //}


        [HttpPost]
        [AllowAnonymous]
        public IActionResult AdminLogin(LoginPerson obj)
        {
            try
            {
                (PatientLogin result , LoggedInPersonViewModel loggedInPerson) = validation.AdminValidate(obj);

                TempData["Email"] = result.emailError;
                TempData["Password"] = result.passwordError;

                if (result.Status == ResponseStautsEnum.Success)
                {
                    Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPerson));
                    if(loggedInPerson.role == "1")
                    {
                        HttpContext.Session.SetString("UserName", loggedInPerson.username);
                        HttpContext.Session.SetString("aspNetUserId", loggedInPerson.aspuserid);
                        HttpContext.Session.SetString("Role", "Admin");
                        TempData["success"] = "Login successfully";
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    if(loggedInPerson.role == "2")
                    {
                        HttpContext.Session.SetString("UserName", loggedInPerson.username);
                        HttpContext.Session.SetString("aspNetUserId", loggedInPerson.aspuserid);
                        HttpContext.Session.SetString("Role", "Physician");
                        TempData["success"] = "Login successfully";
                        return RedirectToAction("ProviderDashboard", "ProviderSide");
                    }
                }
                TempData["error"] = "Incorrect Email or password";
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("aspNetUserId");
            HttpContext.Session.Remove("UserName");
            Response.Cookies.Delete("jwt");
            return RedirectToAction("AdminLogin", "Admin");
        }


        public async Task<IActionResult> Partners()
        {
            PartnerViewModel obj = await dashboardData.Partners();
            return View(obj);
        }

        public async Task<IActionResult> GetPartnerData(int professionType , string searchKey)
        {
            PartnerViewModel obj = await dashboardData.PartnerData(professionType , searchKey);
            return PartialView("AdminCaseAction/_PartnerTable", obj);
        }

        public async Task<IActionResult> AddBusiness()
        {
            BusinessData obj = await dashboardData.GetProfessionsTypes();
            return View(obj);
        }

        public async Task<IActionResult> AddNewBusiness(BusinessData obj)
        {
            await dashboardData.AddNewBusiness(obj);
            return RedirectToAction("Partners");
        }

        public async Task<IActionResult> EditProfession(int professionId)
        {
            BusinessData obj = await dashboardData.ExistingBusinessData(professionId);
            return PartialView("AdminCaseAction/_EditProfession", obj);
        }

        public async Task<IActionResult> DeleteProfession(int professionId)
        {
            await dashboardData.DeleteBusiness(professionId);
            return RedirectToAction("GetPartnerData");
        }

        public async Task<IActionResult> UpdateBusiness(BusinessData obj)
        {
            await dashboardData.UpdateBusiness(obj);
            TempData["success"] = "Updated successfully";
            return RedirectToAction("Partners");
        }

        [HttpPost]
        public IActionResult SendMail(List<int> reqwiseid, int reqid)
        {

            List<string> filenames = new List<string>();
            foreach (var item in reqwiseid)
            {
                var file = _context.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == item).Filename;
                filenames.Add(file);
            }

            Sendemail("vinit2273@gmail.com", "Your Attachments", "Please Find Your Attachments Here", filenames);
            return RedirectToAction("ViewUploads", new { requestId = reqid });

        }
        public void Sendemail(string email, string subject, string message, List<string> attachmentPaths)
        {
            try
            {
                var mail = "tatva.dotnet.vinitpatel@outlook.com";
                var password = "016@ldce";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Set to true if your message contains HTML
                };

                mailMessage.To.Add(email);

                foreach (var attachmentPath in attachmentPaths)
                {
                    string path = "C:\\Users\\pca14\\source\\repos\\HalloDoc\\HalloDoc\\wwwroot\\upload\\" + attachmentPath;
                    if (!string.IsNullOrEmpty(path))
                    {
                        var attachment = new Attachment(path);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public void SendMailForRequest(string firstName, string email)
        {
            var mail = "tatva.dotnet.vinitpatel@outlook.com";
            var password = "016@ldce";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = "Agreement",
                Body = "You can view agreement by using this link : " + firstName,
                IsBodyHtml = true // Set to true if your message contains HTML
            };

            mailMessage.To.Add(email);
            client.SendMailAsync(mailMessage);
        }

        public async Task<IActionResult> SearchRecords()
        {
            RecordsViewModel obj = await dashboardData.SearchRecordsService();
            return View(obj);
        }

        public async Task<IActionResult> SearchRecordTable(SearchRecordsData obj)
        {
            SearchRecordsData newObj = await dashboardData.GetSearchRecordData(obj);
            return PartialView("AdminCaseAction/_SearchRecordTable", newObj);
        }

        public IActionResult PatientRecords()
        {
            return View();
        }

        public async Task<IActionResult> GetPatientHistory(PatientHistory obj)
        {
            PatientHistory dataObj = await dashboardData.GetPatientHistoryData(obj);
            return PartialView("AdminCaseAction/_PatientHistoryTable" , dataObj);
        }

        public async Task<IActionResult> ExplorePatientHistory(int patientId)
        {
            ExplorePatientHistory dataObj = await dashboardData.ExplorePatientHistory(patientId);
            return PartialView("AdminCaseAction/_ExplorePatientHistory" , dataObj);
        }

        public IActionResult BlockHistory()
        {
            return View();
        }

        public async Task<IActionResult> BlockHistoryData(BlockedHistory obj)
        {
            BlockedHistory dataObj = await dashboardData.GetBlockHistoryData(obj);
            return PartialView("AdminCaseAction/_BlockHistoryData", dataObj);
        }

        public async Task<IActionResult> UnblockPatient(int requestId)
        {
            await dashboardData.UnblockPatient(requestId);
            return RedirectToAction("BlockHistoryData", new { requestedPage = 1 , totalEntity = 3});
        }


        public IActionResult Scheduling()
        {
            return View(dashboardData.Scheduling());
        }

        public IActionResult LoadSchedulingPartial(string PartialName, string date, int regionid)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = _context.Physicians.ToList();

            switch (PartialName)
            {
                case "_DayWise":
                    return PartialView("Provider/_DayWise", dashboardData.Daywise(regionid, currentDate, physician));

                case "_WeekWise":
                    return PartialView("Provider/_WeekWise", dashboardData.Weekwise(regionid, currentDate, physician));

                case "_MonthWise":
                    return PartialView("Provider/_MonthWise", dashboardData.Monthwise(regionid, currentDate, physician));

                default:
                    return PartialView("Provider/_DayWise");
            }
        }
        public IActionResult AddShift(Scheduling model)
        {
            if (model.starttime > model.endtime)
            {
                TempData["error"] = "Starttime Must be Less than Endtime";
                return RedirectToAction("Scheduling");
            }
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            var chk = Request.Form["repeatdays"].ToList();
            bool f = dashboardData.AddShift(model, aspNetUserId, chk);
            if (f == false)
            {
                TempData["error"] = "Shift is already assigned in this time";
            }
            return RedirectToAction("Scheduling");
        }


        public Scheduling viewshift(int shiftdetailid)
        {
            return dashboardData.viewshift(shiftdetailid);
        }
        public void ViewShiftreturn(int shiftdetailid)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            dashboardData.ViewShiftreturn(shiftdetailid, aspNetUserId);
        }
        public bool ViewShiftedit(Scheduling modal)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            return dashboardData.ViewShiftedit(modal, aspNetUserId);
        }
        public void DeleteShift(int shiftdetailid)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            dashboardData.DeleteShift(shiftdetailid, aspNetUserId);
        }
        public IActionResult ProvidersOnCall(Scheduling modal)
        {
            return View(dashboardData.ProvidersOnCall(modal));
        }
        [HttpPost]
        public IActionResult ProvidersOnCallbyRegion(int regionid, List<int> oncall, List<int> offcall)
        {
            return PartialView("AdminLayout/_ProviderOnCallData", dashboardData.ProvidersOnCallbyRegion(regionid, oncall, offcall));
        }

        public IActionResult ShiftForReview()
        {
            return View(dashboardData.ShiftForReview());
        }
        public IActionResult ShiftReviewTable(int currentPage, int regionid)
        {
            return PartialView("AdminLayout/_ShiftForReviewTable", dashboardData.ShiftReviewTable(currentPage, regionid));
        }
        public IActionResult ApproveSelected(int[] shiftchk)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            if (shiftchk.Length == 0)
            {
                TempData["error"] = "Please select atleast 1 shift";
            }
            else
            {
                dashboardData.ApproveSelected(shiftchk, aspNetUserId);
                TempData["success"] = "Shifts Approved Successfuly";
            }
            return RedirectToAction("ShiftForReview");
        }
        public IActionResult DeleteSelected(int[] shiftchk)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            if (shiftchk.Length == 0)
            {
                TempData["error"] = "Please select atleast 1 shift";
            }
            else
            {
                dashboardData.DeleteSelected(shiftchk, aspNetUserId);
                TempData["success"] = "Shifts Deleted Successfuly";
            }
            return RedirectToAction("ShiftForReview");
        }

    }
}
