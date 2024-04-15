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

namespace HalloDoc.Controllers
{


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

        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult ProviderLocation()
        {
            return View();
        }

        [Authorization("1")]
        public async Task<IActionResult> AdminDashboard()
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                AdminDashboard obj = await dashboardData.AllData();
                return View(obj);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        [Authorization("1")]
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
            int currentPage = 0;
            if(obj.searchKey == "null")
            {
                obj.searchKey = null;
            }
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

        public IActionResult ViewNotes(int requestId)
        {
            var requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
            var requestnote = _context.Requestnotes.FirstOrDefault(a => a.Requestid == requestId);
            CaseActionDetails obj = new CaseActionDetails();
            if (requestnote != null)
            {
                obj.adminNote = requestnote.Adminnotes;
            }

            obj.requestId = requestId;
            var transferNote = _context.Requeststatuslogs.FirstOrDefault(a => a.Requestid == obj.requestId && a.Status == 2);
            if (transferNote != null)
            {
                var physicianName = _context.Physicians.FirstOrDefault(a => a.Physicianid == transferNote.Physicianid).Firstname;
                obj.adminName = HttpContext.Session.GetString("AdminName");
                obj.physicianName = physicianName;
                obj.assignTime = transferNote.Createddate;
            }

            //bol jaldi bol ungh aave he

            //CaseDetails obj = dashboardData.ViewCaseData(requestId);
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
        public async Task<IActionResult> SubmitAssign(int requestId, int physicianId, string assignNote)
        {   
            await caseActions.SubmitAssign(requestId, physicianId, assignNote);
            return RedirectToAction("AdminDashboard");
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


        public async Task<IActionResult> SubmitNotes(int requestId, string notes, CaseActionDetails obj)
        {
            await caseActions.SubmitNotes(requestId, notes, obj);
            return RedirectToAction("AdminDashboard");
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

        public async Task<IActionResult> SubmitOrder(Orders obj)
        {
            obj.createdby = HttpContext.Session.GetString("AdminName");
            await caseActions.SubmitOrder(obj);
            return RedirectToAction("AdminDashboard");
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


        public IActionResult Agreement(int requestId)
        {
            AgreementDetails obj = caseActions.Agreement(requestId);
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
            int adminId = (int)HttpContext.Session.GetInt32("AdminId")!;
            AdminProfile adminData = await dashboardData.AdminProfileData(adminId);
            return View(adminData);
        }

        public async Task AdminPasswordReset(string password)
        {
            int adminId = (int)HttpContext.Session.GetInt32("AdminId")!;
            await dashboardData.UpdateAdminPassword(adminId, password);
        }

        public async Task UpdateAdminInfo(AdminInfo obj)
        {
            int adminId = (int)HttpContext.Session.GetInt32("AdminId")!;
            await dashboardData.UpdateAdminInfo(adminId, obj);
        }

        public async Task UpdateBillingInfo(BillingInfo obj)
        {
            int adminId = (int)HttpContext.Session.GetInt32("AdminId")!;
            await dashboardData.UpdateBillingInfo(adminId, obj);
        }
        


        public IActionResult RoleAccess()
        {
            RoleAccess obj = dashboardData.AddedRoles();
            return View(obj);
        }
        public IActionResult CreateRoleAccess(int accountType)
        {
            RoleAccess obj = dashboardData.CreateAccessRole(accountType);
            return PartialView("AdminCaseAction/_CreateRoleAccess",obj);
        }

        public IActionResult RolesList(int accountType)
        {
            RoleAccess obj = dashboardData.CreateAccessRole(accountType);
            return PartialView("AdminCaseAction/_RolesList", obj);
        }

        [HttpPost]
        public async Task AddNewRole(List<int> menus , short accountType, string roleName)
        {
            int adminId = (int)HttpContext.Session.GetInt32("AdminId")!;
            await dashboardData.AddNewRole(menus , accountType , roleName , adminId);
        }

        public IActionResult EditRole(int roleId)
        {
            RoleAccess obj = dashboardData.EditRole(roleId);
            return PartialView("AdminCaseAction/_EditRole", obj);
        }

        public IActionResult AdminValidate(Aspnetuser obj)
        {
            try
            {
                var result = validation.AdminValidate(obj);
                TempData["Email"] = result.emailError;
                TempData["Password"] = result.passwordError;
                var check = _context.Aspnetusers.Where(u => u.Email == obj.Email).FirstOrDefault();
                var admindata = _context.Admins.Where(u => u.Aspnetuserid == check.Id).FirstOrDefault();
                var userRole = _context.Aspnetuserroles.FirstOrDefault(u => u.Userid == check.Id);
                LoggedInPersonViewModel loggedInPerson = new LoggedInPersonViewModel();
                loggedInPerson.role = userRole.Roleid;
                loggedInPerson.aspuserid = check.Id;
                loggedInPerson.username = check.Username;

                Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPerson));
                if (result.Status == ResponseStautsEnum.Success)
                {
                    if(admindata != null)
                    {
                    HttpContext.Session.SetString("AdminName", admindata.Firstname);
                    HttpContext.Session.SetInt32("AdminId", admindata.Adminid);
                    TempData["success"] = "Login successfully";
                    }
                    return RedirectToAction("AdminDashboard", "Admin");
                }
                TempData["error"] = "Incorrect Email or password";
                return RedirectToAction("AdminLogin", "Admin");
            }
            catch (Exception ex)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminId");
            HttpContext.Session.Remove("AdminName");
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

            Sendemail("agrawalvishesh9271@gmail.com", "Your Attachments", "Please Find Your Attachments Here", filenames);
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


    }
}
