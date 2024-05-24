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
        private readonly IProviderSideServices providerSideServices;
        private readonly IJwtRepository _jwtRepository;
        private readonly IProviderServices providerServices;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public AdminController(IDashboardData dashboardData, ICaseActions caseActions, IValidation validation, IJwtRepository jwtRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment env , IProviderServices providerServices, IProviderSideServices providerSideServices)
        { 
           
            _jwtRepository = jwtRepository;
            _env = env;
            this.dashboardData = dashboardData;
            this.caseActions = caseActions;
            this.validation = validation;
            this.providerServices = providerServices;
            this.providerSideServices = providerSideServices;
        }

        [AllowAnonymous]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AdminLogin(LoginPerson obj)
        {
            try
            {
                (PatientLogin result, LoggedInPersonViewModel loggedInPerson) = validation.AdminValidate(obj);

                TempData["Email"] = result.emailError;
                TempData["Password"] = result.passwordError;

                if (result.Status == ResponseStautsEnum.Success)
                {
                    Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPerson));
                    if (loggedInPerson.role == "1")
                    {
                        HttpContext.Session.SetString("UserName", loggedInPerson.username);
                        HttpContext.Session.SetString("aspNetUserId", loggedInPerson.aspuserid);
                        HttpContext.Session.SetString("Role", "Admin");
                        TempData["success"] = "Login successfully";
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else if (loggedInPerson.role == "2")
                    {
                        HttpContext.Session.SetString("UserName", loggedInPerson.username);
                        HttpContext.Session.SetString("aspNetUserId", loggedInPerson.aspuserid);
                        HttpContext.Session.SetString("Role", "Physician");
                        TempData["success"] = "Login successfully";
                        return RedirectToAction("ProviderDashboard", "ProviderSide");
                    }
                    else
                    {
                        TempData["error"] = "Access Denied";
                        return View();
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

        [AllowAnonymous]
        public IActionResult AdminResetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> AdminResetPasswordEmail(LoginPerson model)
        {
            (string aspNetUserId, string email) = await dashboardData.FindUser(model);
            string encryptedId = EncryptDecryptHelper.Encrypt(aspNetUserId);
            string resetPasswordUrl = GenerateResetPasswordUrl(encryptedId);
            await SendEmail(email, "Reset Your Password", $"Hello, reset your password using this link: {resetPasswordUrl}");
            TempData["success"] = "Email Sent successfully.";
            return RedirectToAction("AdminLogin", "Admin");
        }

        [AllowAnonymous]
        private string GenerateResetPasswordUrl(string userId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("AdminSetPassword", "Admin", new { id = userId });
            return baseUrl + resetPasswordPath;
        }

        [AllowAnonymous]
        private Task SendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.vinitpatel@outlook.com";
            var password = "016@ldce";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }

        [AllowAnonymous]
        public async Task<IActionResult> AdminSetPassword(string id)
        {
            string decryptedId = EncryptDecryptHelper.Decrypt(id);
            LoginPerson model = new LoginPerson();
            model.aspNetUserId = decryptedId;
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword(LoginPerson model)
        {
            await dashboardData.UpdatePassword(model);
            TempData["success"] = "Password updated successfully.";
            return RedirectToAction("AdminLogin");
        }

        [AllowAnonymous]
        public async Task<bool> AdminEmailValidate(string Email)
        {
            bool flag = await dashboardData.CheckEmail(Email);

            if (flag)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IActionResult ProviderLocation()
        {
            return View();
        }


        public IActionResult CreateRequest()
        {
            return PartialView("AdminCaseAction/_CreateRequest");
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

        public async Task<IActionResult> ViewNotes(int requestId)
        {
            CaseActionDetails obj = await dashboardData.ViewNotes(requestId);
            return PartialView("AdminCaseAction/_ViewNotes", obj);
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
        public async Task CloseRequest(int requestId)
        {
            await caseActions.CloseRequest(requestId);
            TempData["success"] = "Case close successfully";
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

        public async Task<IActionResult> RoleAccess()
        {
            RoleAccess obj = await dashboardData.AddedRoles();
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

        public async Task<bool> CheckRole(string roleName)
        {
            return await dashboardData.CheckRole(roleName);
        }

        [HttpPost]
        public async Task AddNewRole(List<int> menus , short accountType, string roleName)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            await dashboardData.AddNewRole(menus , accountType , roleName , aspNetUserId);
        }

        public async Task<IActionResult> EditRole(int roleId)
        {
            RoleAccess obj = await dashboardData.EditRole(roleId);
            return PartialView("AdminCaseAction/_EditRole", obj);
        }

        public async Task<IActionResult> SaveUpdatedRole(int roleid , List<int> selectedRole)
        {
            await dashboardData.SaveRoleChanges(roleid, selectedRole);
            TempData["success"] = "Role Edited successfully.";
            return RedirectToAction("RoleAccess");
        }

        public async Task DeleteRole(int roleId)
        {
            await dashboardData.DeleteRole(roleId);
        }

        public async Task<bool> CheckAdmin(string adminEmail)
        {
            return await dashboardData.CheckAdmin(adminEmail);
        }

        public async Task<IActionResult> AdminCreateAccount()
        {
            CreateAdminModel obj = new CreateAdminModel();
            obj.regionList = await providerServices.GetRegions();
            obj.rolesList = await dashboardData.GetAdminRoles();
            return View(obj);
        }

        public async Task<IActionResult> SubmitCreateAdmin(CreateAdminModel obj, List<int> selectedRegion)
        {
            await dashboardData.CreateAdmin(obj, selectedRegion);
            TempData["success"] = "Admin Created successfully.";
            return RedirectToAction("AdminLogin");
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
                var file = dashboardData.GetFilesNames(item);
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

        public async Task SendMailForRequest(string firstName, string email)
        {
            var mail = "tatva.dotnet.vinitpatel@outlook.com";
            var password = "016@ldce";

            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId")!;
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string createRequestPath = Url.Action("PatientRequestScreen", "Home");
            string mainURL = baseUrl + createRequestPath;

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = "Create request",
                Body = " Hello "+ firstName + " , You can submit request using this link : " + mainURL,
                IsBodyHtml = true
            };

            await dashboardData.EmailLogEntry(mailMessage.Body, mailMessage.Subject, aspNetUserId, email);


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
            if(obj.requestedPage == 0)
            {
                obj.requestedPage = 1;
            }
            if(obj.totalEntity == 0)
            {
                obj.totalEntity = 3;
            }
            SearchRecordsData newObj = await dashboardData.GetSearchRecordData(obj);
            return PartialView("AdminCaseAction/_SearchRecordTable", newObj);
        }

        public async Task<IActionResult> ExportSearchRecordData(SearchRecordsData obj)
        {
            obj.requestedPage = 0;
            SearchRecordsData data = await dashboardData.GetSearchRecordData(obj);
            var record = dashboardData.DownloadSearchRecordExcle(data);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"{strDate}.xlsx";
            return File(record, contentType, filename);
        }

        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            await dashboardData.DeleteRequest(requestId);
            return RedirectToAction("SearchRecordTable");
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

        public async Task<IActionResult> DownloadEncounter(int requestId)
        {
            return await providerSideServices.DownloadEncounter(requestId);
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

        public IActionResult EmailLogData()
        {
            return View();
        }

        public async Task<IActionResult> GetEmailLogData(EmailLogViewModel obj)
        {
            EmailLogViewModel dataObj = await dashboardData.GetEmailLogData(obj);
            return PartialView("AdminCaseAction/_EmailLogTable",dataObj);
        }

        public IActionResult Scheduling()
        {
            return View(dashboardData.Scheduling());
        }

        public async Task<IActionResult> LoadSchedulingPartial(string PartialName, string date, int regionid)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician =  await dashboardData.PhysicianList(0);

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
        public async Task<IActionResult> AddShift(Scheduling model)
        {
            if (model.starttime > model.endtime)
            {
                TempData["error"] = "Starttime Must be Less than Endtime";
                return RedirectToAction("Scheduling");
            }
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            var chk = Request.Form["repeatdays"].ToList();
            bool f = await dashboardData.AddShift(model, aspNetUserId, chk);
            if (f == false)
            {
                TempData["error"] = "Shift is already assigned in this time";
            }
            return RedirectToAction("Scheduling");
        }


        public async Task<Scheduling> viewshift(int shiftdetailid)
        {
            return await dashboardData.viewshift(shiftdetailid);
        }

        public async Task ViewShiftreturn(int shiftdetailid)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            await dashboardData.ViewShiftreturn(shiftdetailid, aspNetUserId);
        }
        public async Task<bool> ViewShiftedit(Scheduling modal)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            return await dashboardData.ViewShiftedit(modal, aspNetUserId);
        }

        public async Task DeleteShift(int shiftdetailid)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            await dashboardData.DeleteShift(shiftdetailid, aspNetUserId);
        }

        public async Task<IActionResult> ProvidersOnCall(Scheduling modal)
        {
            return View(await dashboardData.ProvidersOnCall(modal));
        }


        [HttpPost]
        public async Task<IActionResult> ProvidersOnCallbyRegion(int regionid, List<int> oncall, List<int> offcall)
        {
            return PartialView("AdminLayout/_ProviderOnCallData", await dashboardData.ProvidersOnCallbyRegion(regionid, oncall, offcall));
        }

        public async Task<IActionResult> ShiftForReview()
        {
            return View(await dashboardData.ShiftForReview());
        }

        public async Task<IActionResult> ShiftReviewTable(int currentPage, int regionid)
        {
            return PartialView("AdminLayout/_ShiftForReviewTable", await dashboardData.ShiftReviewTable(currentPage, regionid));
        }

        public async Task<IActionResult> ApproveSelected(int[] shiftchk)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            if (shiftchk.Length == 0)
            {
                TempData["error"] = "Please select atleast 1 shift";
            }
            else
            {
                await dashboardData.ApproveSelected(shiftchk, aspNetUserId);
                TempData["success"] = "Shifts Approved Successfuly";
            }
            return RedirectToAction("ShiftForReview");
        }

        public async Task<IActionResult> DeleteSelected(int[] shiftchk)
        {
            string aspNetUserId = HttpContext.Session.GetString("aspNetUserId");
            if (shiftchk.Length == 0)
            {
                TempData["error"] = "Please select atleast 1 shift";
            }
            else
            {
                await dashboardData.DeleteSelected(shiftchk, aspNetUserId);
                TempData["success"] = "Shifts Deleted Successfuly";
            }
            return RedirectToAction("ShiftForReview");
        }

    }
}
