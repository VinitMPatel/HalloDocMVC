using Common.Helper;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NuGet.Protocol;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;
using static NPOI.HSSF.Util.HSSFColor;

namespace Services.Implementation
{
    public class DashboardData : IDashboardData
    {

        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public DashboardData(HalloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<AdminDashboard> AllData()
        {
            List<Requestclient> reqc = await _context.Requestclients.Include(a => a.Request).ToListAsync();
            List<Region> regionList = await _context.Regions.ToListAsync();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            obj.regionlist = regionList;
            return obj;
        }

        public async Task<AdminDashboard> AllStateData(AdminDashboard obj)
        {

            if (obj.requestType == 0 && obj.regionId == 0)
            {
                List<Requestclient> reqc = new List<Requestclient>();
                AdminDashboard dataobj = new AdminDashboard();

                if (obj.requeststatus == 4)
                {
                    reqc = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == 4 || a.Request.Status == 5).ToListAsync();
                }
                else if (obj.requeststatus == 3)
                {
                    reqc = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.User.Region).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == 3 || a.Request.Status == 7 || a.Request.Status == 8).ToListAsync();
                }
                else
                {
                    reqc = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == obj.requeststatus).ToListAsync();
                }
                if (!string.IsNullOrWhiteSpace(obj.searchKey))
                {
                    reqc = reqc.Where(a => a.Firstname.ToLower().Contains(obj.searchKey.ToLower()) || a.Lastname.ToLower().Contains(obj.searchKey.ToLower())).ToList();
                }

                if (obj.requestedPage != 0)
                {
                    dataobj.totalPages = (int)Math.Ceiling(reqc.Count() / (double)obj.totalEntity);
                    dataobj.currentpage = obj.requestedPage;
                    reqc = reqc.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();
                }
                dataobj.requestclients = reqc;
                return dataobj;
            }
            else if (obj.requestType != 0 && obj.regionId == 0)
            {
                List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(r => r.Request.User.Region).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == obj.requeststatus && a.Request.Requesttypeid == obj.requestType).ToList();
                AdminDashboard dataobj = new AdminDashboard();

                if (!string.IsNullOrWhiteSpace(obj.searchKey))
                {
                    reqc = reqc.Where(a => a.Firstname.ToLower().Contains(obj.searchKey.ToLower()) || a.Lastname.ToLower().Contains(obj.searchKey.ToLower())).ToList();
                }
                if (obj.requestedPage != 0)
                {
                    dataobj.totalPages = (int)Math.Ceiling(reqc.Count() / (double)obj.totalEntity);
                    dataobj.currentpage = obj.requestedPage;
                    reqc = reqc.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();
                }
                dataobj.requestclients = reqc;
                return dataobj;
            }
            else if (obj.regionId != 0 && obj.requestType == 0)
            {
                List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(r => r.Request.User.Region).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == obj.requeststatus && a.Regionid == obj.regionId).ToList();
                AdminDashboard dataobj = new AdminDashboard();

                if (!string.IsNullOrWhiteSpace(obj.searchKey))
                {
                    reqc = reqc.Where(a => a.Firstname.ToLower().Contains(obj.searchKey.ToLower()) || a.Lastname.ToLower().Contains(obj.searchKey.ToLower())).ToList();
                }
                if (obj.requestedPage != 0)
                {
                    dataobj.totalPages = (int)Math.Ceiling(reqc.Count() / (double)obj.totalEntity);
                    dataobj.currentpage = obj.requestedPage;
                    reqc = reqc.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();
                }
                dataobj.requestclients = reqc;
                return dataobj;
            }
            else
            {
                List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(r => r.Request.User.Region).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == obj.requeststatus && a.Request.Requesttypeid == obj.requestType && a.Regionid == obj.regionId).ToList();
                AdminDashboard dataobj = new AdminDashboard();

                if (!string.IsNullOrWhiteSpace(obj.searchKey))
                {
                    reqc = reqc.Where(a => a.Firstname.ToLower().Contains(obj.searchKey.ToLower()) || a.Lastname.ToLower().Contains(obj.searchKey.ToLower())).ToList();
                }
                if (obj.requestedPage != 0)
                {
                    dataobj.totalPages = (int)Math.Ceiling(reqc.Count() / (double)obj.totalEntity);
                    dataobj.currentpage = obj.requestedPage;
                    reqc = reqc.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();
                }
                dataobj.requestclients = reqc;
                return dataobj;
            }
        }

        public async Task<bool> CheckEmail(string Email)
        {
            Admin? admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == Email);
            Physician? physician = await _context.Physicians.FirstOrDefaultAsync(x => x.Email == Email);
            if (admin == null && physician == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<CaseActionDetails> ViewCaseData(int requestId)
        {
            if (requestId == 0)
            {
                return new CaseActionDetails();
            }
            Data.Entity.Request? request = await _context.Requests.Include(m => m.Physician).FirstOrDefaultAsync(m => m.Requestid == requestId);
            Requestclient? requestclient = await _context.Requestclients.FirstOrDefaultAsync(m => m.Requestid == requestId);
            Region? regiondata = await _context.Regions.FirstOrDefaultAsync(m => m.Regionid == requestclient.Regionid);
            List<Region>? regionList = await _context.Regions.ToListAsync();

            if (requestclient != null && request != null && regiondata != null)
            {
                CaseActionDetails obj = new CaseActionDetails();
                obj.requestId = requestId;
                obj.PatientNotes = requestclient.Notes;
                obj.FirstName = requestclient.Firstname;
                obj.LastName = requestclient.Lastname;
                obj.Email = requestclient.Email;
                obj.DOB = new DateTime(Convert.ToInt32(requestclient.Intyear), DateTime.ParseExact(requestclient.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(requestclient.Intdate));
                obj.PhoneNumber = requestclient.Phonenumber;
                obj.Region = regiondata.Name;
                obj.regionList = regionList;
                obj.Address = requestclient.Address;
                obj.requestType = request.Requesttypeid;
                obj.ConfirmationNumber = request.Confirmationnumber;
                if (request.Physician != null)
                {
                    obj.physician = request.Physician.Firstname;
                }
                obj.status = request.Status;
                return obj;
            }
            return new CaseActionDetails();
        }

        public async Task<CaseActionDetails> ViewNotes(int requestId)
        {
            CaseActionDetails obj = new CaseActionDetails();
            obj.requestId = requestId;

            var requestnote = await _context.Requestnotes.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if (requestnote != null)
            {
                obj.adminNote = requestnote.Adminnotes!;
                obj.physicianNote = requestnote.Physiciannotes!;
            }

            List<Requeststatuslog> requeststatuslogs = await _context.Requeststatuslogs.Where(a => a.Requestid == obj.requestId).ToListAsync();
            obj.requeststatuslogs = requeststatuslogs;
            return obj;
        }


        public async Task<List<Physician>> PhysicianList(int regionid)
        {
            if(regionid == 0)
            {
                return await _context.Physicians.ToListAsync();
            }
            List<Physician> physicianList = await _context.Physicians.Where(a => a.Regionid == regionid).ToListAsync();
            return physicianList;
        }

        public async Task<CaseActionDetails> ViewUploads(int requestId)
        {
            CaseActionDetails obj = new CaseActionDetails();
            //List<Requestwisefile> files = _context.Requestwisefiles(a => a.requestId == requestId).ToList();
            List<Requestwisefile> files = await (from m in _context.Requestwisefiles where m.Requestid == requestId && m.Isdeleted != new BitArray(new[] { true }) select m).ToListAsync();
            var patientName = await _context.Requestclients.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if (patientName != null)
            {
                obj.FirstName = patientName.Firstname;
            }
            obj.requestId = requestId;
            obj.requestwisefile = files;
            return obj;
        }

        public async Task UplodingDocument(List<IFormFile> myfile, int requestId)
        {
            if (myfile.Count() > 0)
            {
                await uploadFile(myfile, requestId);
            }
        }

        public async Task<List<Healthprofessionaltype>> GetProfessions()
        {
            return await _context.Healthprofessionaltypes.ToListAsync();
        }

        public async Task<List<Healthprofessional>> GetBusinesses(int professionId)
        {
            return await _context.Healthprofessionals.Where(u => u.Profession == professionId).ToListAsync();
        }

        public async Task<Healthprofessional> GetBusinessesDetails(int businessid)
        {
            return await _context.Healthprofessionals.FirstOrDefaultAsync(u => u.Vendorid == businessid);
        }

        public async Task uploadFile(List<IFormFile> upload, int id)
        {
            foreach (var item in upload)
            {
                string path = _env.WebRootPath + "/upload/" + item.FileName;
                FileStream stream = new FileStream(path, FileMode.Create);

                item.CopyTo(stream);
                Requestwisefile requestwisefile = new Requestwisefile
                {
                    Requestid = id,
                    Filename = item.FileName,
                    Createddate = DateTime.Now,

                };
                stream.Close();
                await _context.AddAsync(requestwisefile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SingleDelete(int reqfileid)
        {
            var requestwisefile = _context.Requestwisefiles.FirstOrDefault(u => u.Requestwisefileid == reqfileid);
            if (requestwisefile != null)
            {
                int reqid = requestwisefile.Requestid;
                requestwisefile.Isdeleted = new BitArray(new[] { true });
                _context.Requestwisefiles.Update(requestwisefile);
                await _context.SaveChangesAsync();
            }
        }

        public byte[] DownloadExcle(AdminDashboard model)
        {
            using (var workbook = new XSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet("FilteredRecord");
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Sr No.");
                headerRow.CreateCell(1).SetCellValue("Request Id");
                headerRow.CreateCell(2).SetCellValue("Patient Name");
                headerRow.CreateCell(3).SetCellValue("Patient DOB");
                headerRow.CreateCell(4).SetCellValue("RequestorName");
                headerRow.CreateCell(5).SetCellValue("RequestedDate");
                headerRow.CreateCell(6).SetCellValue("PatientPhone");
                headerRow.CreateCell(7).SetCellValue("TransferNotes");
                headerRow.CreateCell(8).SetCellValue("RequestorPhone");
                headerRow.CreateCell(9).SetCellValue("RequestorEmail");
                headerRow.CreateCell(10).SetCellValue("Address");
                headerRow.CreateCell(11).SetCellValue("Notes");
                headerRow.CreateCell(12).SetCellValue("ProviderEmail");
                headerRow.CreateCell(13).SetCellValue("PatientEmail");
                headerRow.CreateCell(14).SetCellValue("RequestType");
                //headerRow.CreateCell(15).SetCellValue("Region");
                headerRow.CreateCell(16).SetCellValue("PhysicainName");
                headerRow.CreateCell(17).SetCellValue("Status");

                for (int i = 0; i < model.requestclients.Count; i++)
                {
                    var reqclient = model.requestclients.ElementAt(i);
                    var type = "";
                    if (reqclient.Request.Requesttypeid == 1)
                    {
                        type = "Patient";
                    }
                    else if (reqclient.Request.Requesttypeid == 2)
                    {
                        type = "Family";
                    }
                    else if (reqclient.Request.Requesttypeid == 4)
                    {
                        type = "Business";
                    }
                    else if (reqclient.Request.Requesttypeid == 3)
                    {
                        type = "Concierge";
                    }
                    IRow row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(i + 1);
                    row.CreateCell(1).SetCellValue(reqclient.Request.Requestid);
                    row.CreateCell(2).SetCellValue(reqclient.Firstname);
                    row.CreateCell(3).SetCellValue(reqclient.Intdate + "/" + reqclient.Strmonth + "/" + reqclient.Intyear);
                    row.CreateCell(4).SetCellValue(reqclient.Request.Firstname);
                    row.CreateCell(5).SetCellValue(reqclient.Request.Createddate);
                    row.CreateCell(6).SetCellValue(reqclient.Phonenumber);
                    if (reqclient.Request.Requeststatuslogs.Count() == 0)
                    {
                        row.CreateCell(7).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(7).SetCellValue(reqclient.Request.Requeststatuslogs.ElementAt(0).Notes);
                    }
                    row.CreateCell(8).SetCellValue(reqclient.Request.Phonenumber);
                    row.CreateCell(9).SetCellValue(reqclient.Request.Email);
                    row.CreateCell(10).SetCellValue(reqclient.Request.Requestclients.ElementAt(0).Address);
                    row.CreateCell(11).SetCellValue(reqclient.Notes);
                    if (reqclient.Request.Physician == null)
                    {
                        row.CreateCell(12).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue(reqclient.Request.Physician.Email);
                    }
                    row.CreateCell(13).SetCellValue(reqclient.Email);
                    row.CreateCell(14).SetCellValue(type);
                    //row.CreateCell(15).SetCellValue(reqclient.Region.Name);
                    if (reqclient.Request.Physician == null)
                    {
                        row.CreateCell(16).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(16).SetCellValue(reqclient.Request.Physician.Firstname);
                    }
                    row.CreateCell(17).SetCellValue(reqclient.Request.Status);
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        public async Task<AdminProfile> AdminProfileData(string aspNetUserId)
        {
            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Id == aspNetUserId);

            AdminProfile adminData = new AdminProfile();

            Admin? admin = await _context.Admins.FirstOrDefaultAsync(a => a.Aspnetuserid == aspNetUserId);
            if (admin != null)
            {
                adminData.admin = admin;
            }
            if (aspnetuser != null)
            {
                adminData.userName = aspnetuser.Username;
                string decryptedPassword = EncryptDecryptHelper.Decrypt(aspnetuser.Passwordhash!);
                adminData.password = decryptedPassword;
            }
            adminData.regionlist = await _context.Regions.ToListAsync();
            adminData.adminregionlist = await _context.Adminregions.Where(a => a.Adminid == admin.Adminid).ToListAsync();
            adminData.rolesList = await _context.Roles.Where(a => a.Accounttype == 1).ToListAsync();
            return adminData;
        }

        public async Task UpdateAdminPassword(string aspNetUserId, string password)
        {
            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Id == aspNetUserId);
            if (aspnetuser != null)
            {
                string encryptedPassword = EncryptDecryptHelper.Encrypt(password);
                aspnetuser.Passwordhash = encryptedPassword;
                _context.Update(aspnetuser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAdminInfo(string aspNetUserId, AdminInfo obj)
        {
            Admin? admin = _context.Admins.FirstOrDefault(a => a.Aspnetuserid == aspNetUserId);
            Aspnetuser? aspnetuser = _context.Aspnetusers.FirstOrDefault(a => a.Id == aspNetUserId);
            List<Adminregion> adminRegions = _context.Adminregions.Where(a => a.Adminid == admin.Adminid).ToList();
            foreach (var item in adminRegions)
            {
                _context.Adminregions.Remove(item);
            }

            if (obj.selectedregion != null)
            {
                foreach (var item in obj.selectedregion)
                {
                    Adminregion newAdminRegion = new Adminregion();
                    newAdminRegion.Adminid = admin.Adminid;
                    newAdminRegion.Regionid = item;
                    _context.Adminregions.Add(newAdminRegion);
                }
            }
            if (admin != null && aspnetuser != null)
            {
                admin.Firstname = obj.firstName;
                admin.Lastname = obj.lastName;
                admin.Email = obj.email;
                admin.Mobile = obj.contact;
                admin.Modifieddate = DateTime.Now;
                admin.Modifiedby = aspnetuser.Id;
                _context.Update(admin);
            }

            if (aspnetuser != null)
            {
                aspnetuser.Phonenumber = obj.contact;
                aspnetuser.Username = obj.firstName + obj.lastName;
                aspnetuser.Email = obj.email;
                aspnetuser.Modifieddate = DateTime.Now;
                _context.Update(aspnetuser);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBillingInfo(string aspNetUserId, BillingInfo obj)
        {
            Admin? admin = _context.Admins.FirstOrDefault(a => a.Aspnetuserid == aspNetUserId);
            Aspnetuser? aspnetuser = _context.Aspnetusers.FirstOrDefault(a => a.Id == aspNetUserId);
            if (admin != null && aspnetuser != null)
            {
                admin.Address1 = obj.address1;
                admin.Address2 = obj.address2;
                admin.City = obj.city;
                admin.Zip = obj.zip;
                admin.Modifieddate = DateTime.Now;
                admin.Modifiedby = aspnetuser.Id;
                admin.Altphone = obj.billingContact;

                _context.Update(admin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RoleAccess> CreateAccessRole(int accountType, int roleId)
        {
            List<Menu> menuList = new List<Menu>();
            if (accountType == 0)
            {
                menuList = await _context.Menus.ToListAsync();
            }
            else
            {
                menuList = await _context.Menus.Where(a => a.Accounttype == accountType).ToListAsync();
            }
            RoleAccess roleAccess = new RoleAccess();
            roleAccess.menuList = menuList;
            roleAccess.roleId = roleId;
            List<Rolemenu> rolemenus = await _context.Rolemenus.Where(a => a.Roleid == roleId).ToListAsync();
            List<Menu> selectedMenus = new List<Menu>();
            foreach (var item in rolemenus)
            {
                selectedMenus.Add(await _context.Menus.FirstOrDefaultAsync(a => a.Menuid == item.Menuid));
            }
            roleAccess.selectedMenu = selectedMenus;
            return roleAccess;
        }

        public RoleAccess AddedRoles()
        {
            RoleAccess obj = new RoleAccess();
            obj.rolemenuList = _context.Roles.ToList();
            return obj;
        }

        public async Task AddNewRole(List<int> menus, short accountType, string roleName, string aspNetUserId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Role role = new Role
                    {
                        Name = roleName,
                        Accounttype = accountType,
                        Createdby = aspNetUserId,
                        Createddate = DateTime.Now,
                        Modifieddate = DateTime.Now,
                        Isdeleted = new BitArray(new[] { false })
                    };
                    await _context.AddAsync(role);
                    await _context.SaveChangesAsync();

                    foreach (var item in menus)
                    {
                        Rolemenu rolemenu = new Rolemenu();
                        rolemenu.Roleid = role.Roleid;
                        rolemenu.Menuid = item;
                        await _context.AddAsync(rolemenu);
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

        }

        public RoleAccess EditRole(int roleId)
        {
            RoleAccess roleAccess = new RoleAccess();
            roleAccess.menuList = _context.Menus.ToList();
            roleAccess.roleId = roleId;
            roleAccess.roleName = _context.Roles.FirstOrDefault(a => a.Roleid == roleId).Name;
            roleAccess.accountType = _context.Roles.FirstOrDefault(a => a.Roleid == roleId).Accounttype;
            //_context.Aspnetroles.FirstOrDefault(a => a.Id == roleId.ToString()).Name
            return roleAccess;
        }


        public async Task SaveRoleChanges(int roleid, List<int> selectedRole)
        {
            if(roleid != 0)
            {
                List<Rolemenu> rolemenus = await _context.Rolemenus.Where(a => a.Roleid == roleid).ToListAsync();
                foreach(var item in rolemenus)
                {
                    _context.Rolemenus.Remove(item);
                }
                
                if(selectedRole.Count() > 0)
                {
                    foreach (var item in selectedRole)
                    {
                        Rolemenu rolemenu = new Rolemenu
                        {
                            Roleid = roleid,
                            Menuid = item
                        };
                        await _context.AddAsync(rolemenu);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
        //public async Task<UserAccessData> GetUserAccessData()
        //{
        //    List<Admin>? adminList = await _context.Admins.ToListAsync();
        //    List<Physician>? physiciansList = await _context.Physicians.ToListAsync();
        //    foreach (var admin in adminList)
        //    {

        //    }
        //}


        public async Task<PartnerViewModel> PartnerData(int professionType, string searchKey)
        {
            PartnerViewModel obj = new PartnerViewModel();
            if (professionType == 0)
            {
                obj.professionList = await _context.Healthprofessionals.Include(a => a.ProfessionNavigation).Where(a => a.Isdeleted == new BitArray(new[] { false })).ToListAsync();
            }
            else
            {
                obj.professionList = await _context.Healthprofessionals.Include(a => a.ProfessionNavigation).Where(a => a.ProfessionNavigation.Healthprofessionalid == professionType && a.Isdeleted == new BitArray(new[] { false })).ToListAsync();

            }

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                obj.professionList = obj.professionList.Where(a => a.Vendorname.ToLower().Contains(searchKey.ToLower())).ToList();
            }
            return obj;
        }

        public async Task<PartnerViewModel> Partners()
        {
            PartnerViewModel obj = new PartnerViewModel();
            obj.professionTypeList = await _context.Healthprofessionaltypes.ToListAsync();
            return obj;
        }

        public async Task<BusinessData> GetProfessionsTypes()
        {
            BusinessData obj = new BusinessData();
            obj.professionTypeList = await _context.Healthprofessionaltypes.ToListAsync();
            return obj;
        }

        public string GetFilesNames(int reqwiseid)
        {
            return _context.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == reqwiseid)!.Filename;
        }
        public async Task AddNewBusiness(BusinessData obj)
        {
            if (obj != null)
            {
                Healthprofessional healthprofessional = new Healthprofessional
                {
                    Vendorname = obj.businessName,
                    Profession = obj.professionType,
                    Faxnumber = obj.faxNumber,
                    Address = obj.street + ", " + obj.city,
                    City = obj.city,
                    State = obj.state,
                    Zip = obj.zip,
                    Createddate = DateTime.Now,
                    Modifieddate = DateTime.Now,
                    Phonenumber = obj.phoneNumber,
                    Email = obj.email,
                    Businesscontact = obj.businessContact,
                    Isdeleted = new BitArray(new[] { false })
                };
                await _context.AddAsync(healthprofessional);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<BusinessData> ExistingBusinessData(int professionId)
        {
            Healthprofessional? healthprofessional = await _context.Healthprofessionals.FirstOrDefaultAsync(a => a.Vendorid == professionId);
            BusinessData businessData = new BusinessData();
            if (healthprofessional != null)
            {
                businessData.businessName = healthprofessional.Vendorname;
                businessData.professionType = (int)healthprofessional.Profession!;
                businessData.faxNumber = healthprofessional.Faxnumber;
                businessData.phoneNumber = healthprofessional.Phonenumber!;
                businessData.email = healthprofessional.Email!;
                businessData.businessContact = healthprofessional.Businesscontact!;
                businessData.city = healthprofessional.City!;
                businessData.state = healthprofessional.State!;
                businessData.zip = healthprofessional.Zip!;
                businessData.street = healthprofessional.Address!.Split(',')[0];
                businessData.professionTypeList = await _context.Healthprofessionaltypes.ToListAsync();
                businessData.vendorId = healthprofessional.Vendorid;
            }
            return businessData;
        }

        public async Task DeleteBusiness(int profesionId)
        {
            Healthprofessional? healthprofessional = await _context.Healthprofessionals.FirstOrDefaultAsync(a => a.Vendorid == profesionId);
            if (healthprofessional != null)
            {
                healthprofessional.Isdeleted = new BitArray(new[] { true });
                _context.Update(healthprofessional);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBusiness(BusinessData obj)
        {
            Healthprofessional? healthprofessional = await _context.Healthprofessionals.FirstOrDefaultAsync(a => a.Vendorid == obj.vendorId);
            if (healthprofessional != null)
            {
                healthprofessional.Vendorname = obj.businessName;
                healthprofessional.Profession = obj.professionType;
                healthprofessional.Faxnumber = obj.faxNumber;
                healthprofessional.Phonenumber = obj.phoneNumber;
                healthprofessional.Email = obj.email;
                healthprofessional.Businesscontact = obj.businessContact;
                healthprofessional.City = obj.city;
                healthprofessional.State = obj.state;
                healthprofessional.Zip = obj.zip;
                healthprofessional.Address = obj.street + ", " + obj.city;
                healthprofessional.Modifieddate = DateTime.Now;
                _context.Update(healthprofessional);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RecordsViewModel> SearchRecordsService()
        {
            RecordsViewModel obj = new RecordsViewModel();
            obj.requestTypeList = await _context.Requesttypes.ToListAsync();
            return obj;
        }

        public async Task<SearchRecordsData> GetSearchRecordData(SearchRecordsData obj)
        {
            SearchRecordsData dataObj = new SearchRecordsData();
            List<Requestclient> requestclients = new List<Requestclient>();

            requestclients = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Requestnotes).Include(a => a.Request.Requeststatuslogs).Include(a => a.Request.Requesttype).Where(a=>a.Request.Isdeleted == new BitArray(new[] {false})).ToListAsync();

            requestclients = requestclients.Where(a =>
                                (obj.selectedStatus == 0 || a.Request.Status == obj.selectedStatus) &&
                                (string.IsNullOrWhiteSpace(obj.searchedPatient) || a.Firstname.ToLower().Contains(obj.searchedPatient.ToLower()) || a.Lastname.ToLower().Contains(obj.searchedPatient.ToLower())) &&
                                (obj.selectedType == 0 || a.Request.Requesttypeid == obj.selectedType) &&
                                (string.IsNullOrWhiteSpace(obj.searchedProvider) || a.Request.Physicianid != null && a.Request.Physician.Firstname.ToLower().Contains(obj.searchedProvider.ToLower())) &&
                                (string.IsNullOrWhiteSpace(obj.searchedEmail) || a.Email.ToLower().Contains(obj.searchedEmail.ToLower())) &&
                                (string.IsNullOrWhiteSpace(obj.searchedPhone) || a.Phonenumber.Contains(obj.searchedPhone))).ToList();

            if (obj.toDate.Year != 1 && obj.toDate.Month != 1 && obj.toDate.Day != 1)
            {
                requestclients = requestclients.Where(u => u.Request.Accepteddate != null).ToList();
                requestclients = requestclients.Where(u => u.Request.Accepteddate!.Value.Date <= obj.toDate!).ToList();
            }
            if (obj.fromDate.Year != 1 && obj.fromDate.Month != 1 && obj.fromDate.Day != 1)
            {
                requestclients = requestclients.Where(u => u.Request.Accepteddate != null).ToList();
                requestclients = requestclients.Where(u => u.Request.Accepteddate! >= obj.fromDate!).ToList();
            }

            if (obj.requestedPage != 0)
            {
                dataObj.totalPages = (int)Math.Ceiling(requestclients.Count() / (double)obj.totalEntity);
                dataObj.currentpage = obj.requestedPage;
                requestclients = requestclients.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();
            }

            dataObj.requestclients = requestclients;
            return dataObj;
        }

        public async Task DeleteRequest(int requestId)
        {
            Data.Entity.Request? request = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if(request != null)
            {
                request.Isdeleted = new BitArray(new[] { true });
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PatientHistory> GetPatientHistoryData(PatientHistory obj)
        {
            List<User> userList = new List<User>();
            PatientHistory dataObj = new PatientHistory();
            userList = await _context.Users.ToListAsync();

            userList = userList.Where(a =>
            ((string.IsNullOrWhiteSpace(obj.firstName)) || a.Firstname.ToLower().Contains(obj.firstName.ToLower())) &&
            ((string.IsNullOrWhiteSpace(obj.lastName)) || a.Lastname.ToLower().Contains(obj.lastName.ToLower())) &&
            ((string.IsNullOrWhiteSpace(obj.mobile)) || a.Mobile.ToLower().Contains(obj.mobile.ToLower())) &&
            ((string.IsNullOrWhiteSpace(obj.email)) || a.Email.ToLower().Contains(obj.email.ToLower()))).ToList();

            dataObj.userList = userList;
            return dataObj;
        }

        public async Task<ExplorePatientHistory> ExplorePatientHistory(int patientId)
        {
            ExplorePatientHistory dataObj = new ExplorePatientHistory();
            dataObj.reqcList = await _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Userid == patientId).Include(a => a.Request.Physician).ToListAsync();
            return dataObj;
        }

        public async Task<List<Physician>> GetPhysicianData()
        {
            return await _context.Physicians.ToListAsync();
        }

        public async Task<List<Physicianlocation>> GetPhysicianLocation()
        {
            return await _context.Physicianlocations.ToListAsync();
        }

        public async Task<List<Role>> GetAdminRoles()
        {
            return await _context.Roles.Where(a => a.Accounttype == 1).ToListAsync();
        }

        public async Task CreateAdmin(CreateAdminModel obj, List<int> selectedRegion)
        {
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (obj != null)
                    {
                        string encryptPassword = EncryptDecryptHelper.Encrypt(obj.password);
                        Aspnetuser aspnetuser = new Aspnetuser
                        {
                            Id = Guid.NewGuid().ToString(),
                            Username = obj.firstName + obj.lastName,
                            Passwordhash = encryptPassword,
                            Email = obj.email,
                            Phonenumber = obj.contactNumber,
                            Createddate = DateTime.Now,
                            Modifieddate = DateTime.Now
                        };
                        await _context.Aspnetusers.AddAsync(aspnetuser);
                        await _context.SaveChangesAsync();

                        Admin admin = new Admin
                        {
                            Aspnetuserid = aspnetuser.Id,
                            Firstname = obj.firstName,
                            Lastname = obj.lastName,
                            Email = obj.email,
                            Mobile = obj.contactNumber,
                            Address1 = obj.address1,
                            Address2 = obj.address2,
                            Regionid = obj.adminRegion,
                            Zip = obj.zipcode,
                            Altphone = obj.billingContact,
                            Createdby = aspnetuser.Id,
                            Createddate = DateTime.Now,
                            Modifieddate = DateTime.Now,
                            Isdeleted = new BitArray(new[] { false }),
                            Roleid = obj.role,
                            City = obj.city
                        };
                        await _context.Admins.AddAsync(admin);
                        await _context.SaveChangesAsync();

                        if (selectedRegion.Count() > 0)
                        {
                            foreach (var item in selectedRegion)
                            {
                                Adminregion adminregion = new Adminregion();
                                adminregion.Adminid = admin.Adminid;
                                adminregion.Regionid = item;
                                await _context.AddAsync(adminregion);
                            }
                        }

                        Aspnetuserrole aspnetuserrole = new Aspnetuserrole
                        {
                            Userid = aspnetuser.Id,
                            Roleid = "1"
                        };
                        await _context.Aspnetuserroles.AddAsync(aspnetuserrole);
                        await _context.SaveChangesAsync();

                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task<BlockedHistory> GetBlockHistoryData(BlockedHistory obj)
        {
            BlockedHistory dataObj = new BlockedHistory();
            List<Requestclient> requestclients = new List<Requestclient>();

            requestclients = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Blockrequests).Where(a => a.Request.Status == 11).ToListAsync();

            requestclients = requestclients.Where(a =>
                                (string.IsNullOrWhiteSpace(obj.name) || a.Firstname.ToLower().Contains(obj.name.ToLower()) || a.Lastname!.ToLower().Contains(obj.name.ToLower())) &&
                                (string.IsNullOrWhiteSpace(obj.email) || a.Email!.ToLower().Contains(obj.email.ToLower())) &&
                                (string.IsNullOrWhiteSpace(obj.phone) || a.Phonenumber!.Contains(obj.phone))).ToList();

            dataObj.totalPages = (int)Math.Ceiling(requestclients.Count() / (double)obj.totalEntity);
            dataObj.currentpage = obj.requestedPage;
            requestclients = requestclients.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();
            dataObj.totalEntity = obj.totalEntity;
            dataObj.requestclients = requestclients;

            return dataObj;
        }

        public async Task UnblockPatient(int requestId)
        {
            Blockrequest? blockData = await _context.Blockrequests.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if (blockData != null)
            {
                blockData.Isactive = new BitArray(new[] { true });
                _context.Update(blockData);
                await _context.SaveChangesAsync();
            }
        }

        public Scheduling Scheduling()
        {
            Scheduling modal = new Scheduling();
            modal.regions = _context.Regions.ToList();
            return modal;
        }
        public List<Physician> GetPhysicians()
        {
            return _context.Physicians.ToList();
        }
        public DayWiseScheduling Daywise(int regionid, DateTime currentDate, List<Physician> physician)
        {
            DayWiseScheduling day = new DayWiseScheduling
            {
                date = currentDate,
                physicians = physician,
                shiftdetails = _context.Shiftdetailregions.Include(u => u.Shiftdetail).ThenInclude(u => u.Shift).Where(u => u.Regionid == regionid).Select(u => u.Shiftdetail).ToList()
            };
            if (regionid == 0)
            {
                day.shiftdetails = _context.Shiftdetails.Include(u => u.Shift).Where(u => u.Isdeleted == new BitArray(new[] { false })).ToList();
            }
            return day;
        }

        public WeekWiseScheduling Weekwise(int regionid, DateTime currentDate, List<Physician> physician)
        {
            WeekWiseScheduling week = new WeekWiseScheduling
            {
                date = currentDate,
                physicians = physician,
                shiftdetails = _context.Shiftdetailregions.Include(u => u.Shiftdetail).ThenInclude(u => u.Shift).ThenInclude(u => u.Physician).Where(u => u.Isdeleted == new BitArray(new[] { false })).Where(u => u.Regionid == regionid).Select(u => u.Shiftdetail).ToList()
            };
            if (regionid == 0)
            {
                week.shiftdetails = _context.Shiftdetails.Include(u => u.Shift).ThenInclude(u => u.Physician).Where(u => u.Isdeleted == new BitArray(new[] { false })).ToList();
            }
            return week;
        }

        public MonthWiseScheduling Monthwise(int regionid, DateTime currentDate, List<Physician> physician)
        {
            MonthWiseScheduling month = new MonthWiseScheduling
            {
                date = currentDate,
                shiftdetails = _context.Shiftdetailregions.Include(u => u.Shiftdetail).ThenInclude(u => u.Shift).ThenInclude(u => u.Physician).Where(u => u.Isdeleted == new BitArray(new[] { false })).Where(u => u.Regionid == regionid).Select(u => u.Shiftdetail).ToList()
            };
            if (regionid == 0)
            {
                month.shiftdetails = _context.Shiftdetails.Include(u => u.Shift).ThenInclude(u => u.Physician).Where(u => u.Isdeleted == new BitArray(new[] { false })).ToList();
            }
            return month;
        }


        public async Task<bool> AddShift(Scheduling model, string aspNetUserId, List<string> chk)
        {
            Admin? admin = await _context.Admins.FirstOrDefaultAsync(a => a.Aspnetuserid == aspNetUserId);
            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Id == aspNetUserId);
            var shiftid = await _context.Shifts.Where(u => u.Physicianid == model.physicianid).Select(u => u.Shiftid).ToListAsync();
            if (shiftid.Count() > 0)
            {
                foreach (var obj in shiftid)
                {
                    var shiftdetailchk = await _context.Shiftdetails.Where(u => u.Shiftid == obj && u.Shiftdate == model.shiftdate && u.Isdeleted == new BitArray(new[] { false })).ToListAsync();
                    if (shiftdetailchk.Count() > 0)
                    {
                        foreach (var item in shiftdetailchk)
                        {
                            if (model.starttime <= item.Starttime && model.endtime >= item.Endtime)
                            {
                                return false;
                            }
                            if (((model.starttime >= item.Starttime && model.starttime < item.Endtime) || (model.endtime > item.Starttime && model.endtime <= item.Endtime)))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            Shift shift = new Shift
            {
                Physicianid = model.physicianid,
                Startdate = DateOnly.FromDateTime(model.shiftdate),
                Repeatupto = model.repeatcount,
                Createddate = DateTime.Now,
                Createdby = aspnetuser!.Id
            };
            foreach (var obj in chk)
            {
                shift.Weekdays += obj;
            }
            if (model.repeatcount > 0)
            {
                shift.Isrepeat = new BitArray(new[] { true });
            }
            else
            {
                shift.Isrepeat = new BitArray(new[] { false });
            }
            await _context.Shifts.AddAsync(shift);
            await _context.SaveChangesAsync();

            DateTime curdate = model.shiftdate;

            Shiftdetail shiftdetail = new Shiftdetail();
            shiftdetail.Shiftid = shift.Shiftid;
            shiftdetail.Shiftdate = curdate;
            shiftdetail.Regionid = model.regionid;
            shiftdetail.Starttime = model.starttime;
            shiftdetail.Endtime = model.endtime;
            shiftdetail.Isdeleted = new BitArray(new[] { false });
            await _context.Shiftdetails.AddAsync(shiftdetail);
            await _context.SaveChangesAsync();

            Shiftdetailregion shiftregion = new Shiftdetailregion
            {
                Shiftdetailid = shiftdetail.Shiftdetailid,
                Regionid = model.regionid,
                Isdeleted = new BitArray(new[] { false })
            };
            await _context.Shiftdetailregions.AddAsync(shiftregion);
            await _context.SaveChangesAsync();
            
            var dayofweek = model.shiftdate.DayOfWeek.ToString();
            int valueforweek;
            if (dayofweek == "Sunday")
            {
                valueforweek = 0;
            }
            else if (dayofweek == "Monday")
            {
                valueforweek = 1;
            }
            else if (dayofweek == "Tuesday")
            {
                valueforweek = 2;
            }
            else if (dayofweek == "Wednesday")
            {
                valueforweek = 3;
            }
            else if (dayofweek == "Thursday")
            {
                valueforweek = 4;
            }
            else if (dayofweek == "Friday")
            {
                valueforweek = 5;
            }
            else
            {
                valueforweek = 6;
            }

            if (shift.Isrepeat[0] == true)
            {
                for (int j = 0; j < shift.Weekdays.Count(); j++)
                {
                    var z = shift.Weekdays;
                    var p = shift.Weekdays.ElementAt(j).ToString();
                    int ele = Int32.Parse(p);
                    int x;
                    if (valueforweek > ele)
                    {
                        x = 6 - valueforweek + 1 + ele;
                    }
                    else
                    {
                        x = ele - valueforweek;
                    }
                    if (x == 0)
                    {
                        x = 7;
                    }
                    DateTime newcurdate = model.shiftdate.AddDays(x);
                    for (int i = 0; i < model.repeatcount; i++)
                    {

                        if (shiftid.Count() > 0)
                        {
                            foreach (var obj in shiftid)
                            {
                                var shiftdetailchk = await _context.Shiftdetails.Where(u => u.Shiftid == obj && u.Shiftdate == newcurdate && u.Isdeleted == new BitArray(new[] { false })).ToListAsync();
                                if (shiftdetailchk.Count() > 0)
                                {
                                    foreach (var item in shiftdetailchk)
                                    {
                                        if (model.starttime <= item.Starttime && model.endtime >= item.Endtime)
                                        {
                                            return false;
                                        }
                                        if (((model.starttime >= item.Starttime && model.starttime < item.Endtime) || (model.endtime > item.Starttime && model.endtime <= item.Endtime)))
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }

                        Shiftdetail shiftdetailnew = new Shiftdetail
                        {
                            Shiftid = shift.Shiftid,
                            Shiftdate = newcurdate,
                            Regionid = model.regionid,
                            Starttime = model.starttime,
                            Endtime = model.endtime,
                            Isdeleted = new BitArray(new[] { false })
                        };
                        await _context.Shiftdetails.AddAsync(shiftdetailnew);
                        await _context.SaveChangesAsync();

                        Shiftdetailregion shiftregionnew = new Shiftdetailregion
                        {
                            Shiftdetailid = shiftdetailnew.Shiftdetailid,
                            Regionid = model.regionid,
                            Isdeleted = new BitArray(new[] { false })
                        };
                        await _context.Shiftdetailregions.AddAsync(shiftregionnew);
                        await _context.SaveChangesAsync();
                        newcurdate = newcurdate.AddDays(7);
                    }
                }
            }
            return true;
        }

        public async Task<Scheduling> viewshift(int shiftdetailid)
        {
            Scheduling modal = new Scheduling();
            Shiftdetail shiftdetail = await _context.Shiftdetails.Include(u => u.Shift).ThenInclude(u => u.Physician).FirstOrDefaultAsync(u => u.Shiftdetailid == shiftdetailid);
            modal.regionid = (int)shiftdetail.Regionid;
            modal.physicianname = shiftdetail.Shift.Physician.Firstname + " " + shiftdetail.Shift.Physician.Lastname;
            modal.modaldate = shiftdetail.Shiftdate.ToString("yyyy-MM-dd");
            modal.starttime = shiftdetail.Starttime;
            modal.endtime = shiftdetail.Endtime;
            modal.shiftdetailid = shiftdetailid;
            return modal;
        }

        public async Task ViewShiftreturn(int shiftdetailid, string aspNetUserId)
        {
            var shiftdetail = await _context.Shiftdetails.FirstOrDefaultAsync(u => u.Shiftdetailid == shiftdetailid);
            if (shiftdetail.Status == 0)
            {
                shiftdetail.Status = 1;
            }
            else
            {
                shiftdetail.Status = 0;
            }
            shiftdetail.Modifieddate = DateTime.Now;
            shiftdetail.Modifiedby = aspNetUserId;

            _context.Shiftdetails.Update(shiftdetail);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ViewShiftedit(Scheduling modal, string aspNetUserId)
        {
            var shiftdetail = await _context.Shiftdetails.FirstOrDefaultAsync(u => u.Shiftdetailid == modal.shiftdetailid);
            var checkshift = await _context.Shiftdetails.Where(u => u.Shiftdate == shiftdetail.Shiftdate).ToListAsync();
            if (checkshift.Count() > 0)
            {
                foreach (var obj in checkshift)
                {
                    if (((modal.starttime >= obj.Starttime && modal.starttime < obj.Endtime) || (modal.endtime > obj.Starttime && modal.endtime <= obj.Endtime)) && modal.shiftdetailid != obj.Shiftdetailid)
                    {
                        return false;
                    }
                }
            }

            shiftdetail.Shiftdate = modal.shiftdate;
            shiftdetail.Starttime = modal.starttime;
            shiftdetail.Endtime = modal.endtime;
            shiftdetail.Modifieddate = DateTime.Now;
            shiftdetail.Modifiedby = aspNetUserId;
            _context.Shiftdetails.Update(shiftdetail);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteShift(int shiftdetailid, string aspNetUserId)
        {
            var shiftdetail = await _context.Shiftdetails.FirstOrDefaultAsync(u => u.Shiftdetailid == shiftdetailid);
            shiftdetail.Isdeleted = new BitArray(new[] { true });
            shiftdetail.Modifieddate = DateTime.Now;
            shiftdetail.Modifiedby = aspNetUserId;
            _context.Shiftdetails.Update(shiftdetail);
            await _context.SaveChangesAsync();

            var shiftregion = await _context.Shiftdetailregions.FirstOrDefaultAsync(u => u.Shiftdetailid == shiftdetailid);
            shiftregion.Isdeleted = new BitArray(new[] { true });
            _context.Shiftdetailregions.Update(shiftregion);
            await _context.SaveChangesAsync();
        }

        public async Task<Scheduling> ProvidersOnCall(Scheduling modal)
        {
            var currentDate = DateTime.Parse(modal.curdate.ToString());
            modal.regions = await _context.Regions.ToListAsync();
            HashSet<int> phyid = new HashSet<int>();
            HashSet<int> offdutyphyid =  _context.Physicians.Select(u => u.Physicianid).ToHashSet();
            List<Physician> phyoncall = new List<Physician>();
            List<Physician> phyoffduty = new List<Physician>();
            if (modal.wisetype == "_DayWise")
            {
                if (modal.regionid == 0)
                {
                    phyid =  _context.Shiftdetails.Include(u => u.Shift).Where(u => u.Shiftdate == currentDate && u.Isdeleted == new BitArray(new[] { false })).Select(u => u.Shift.Physicianid).ToHashSet();
                }
                else
                {
                    phyid =  _context.Shiftdetails.Include(u => u.Shift).Where(u => u.Shiftdate == currentDate && u.Regionid == modal.regionid && u.Isdeleted == new BitArray(new[] { false })).Select(u => u.Shift.Physicianid).ToHashSet();
                }
            }
            else if (modal.wisetype == "_WeekWise")
            {
                if (modal.regionid == 0)
                {
                    phyid =  _context.Shiftdetails.Include(u => u.Shift).Where(u => u.Shiftdate >= currentDate && u.Shiftdate <= currentDate.AddDays(6) && u.Isdeleted == new BitArray(new[] { false })).Select(u => u.Shift.Physicianid).ToHashSet();
                }
                else
                {
                    phyid =  _context.Shiftdetails.Include(u => u.Shift).Where(u => u.Shiftdate >= currentDate && u.Shiftdate <= currentDate.AddDays(6) && u.Regionid == modal.regionid && u.Isdeleted == new BitArray(new[] { false })).Select(u => u.Shift.Physicianid).ToHashSet();
                }
            }
            else
            {
                if (modal.regionid == 0)
                {
                    phyid =  _context.Shiftdetails.Include(u => u.Shift).Where(u => u.Shiftdate.Month == currentDate.Month && u.Shiftdate.Year == currentDate.Year && u.Isdeleted == new BitArray(new[] { false })).Select(u => u.Shift.Physicianid).ToHashSet();
                }
                else
                {
                    phyid =  _context.Shiftdetails.Include(u => u.Shift).Where(u => u.Shiftdate.Month == currentDate.Month && u.Shiftdate.Year == currentDate.Year && u.Regionid == modal.regionid && u.Isdeleted == new BitArray(new[] { false })).Select(u => u.Shift.Physicianid).ToHashSet();
                }
            }

            offdutyphyid.ExceptWith(phyid);
            foreach (var obj in phyid)
            {
                var physician = await _context.Physicians.FirstOrDefaultAsync(u => u.Physicianid == obj);
                if (physician != null)
                {
                    phyoncall.Add(physician);
                }
            }
            foreach (var obj in offdutyphyid)
            {
                var physician = await _context.Physicians.FirstOrDefaultAsync(u => u.Physicianid == obj);
                if (physician != null)
                {
                    phyoffduty.Add(physician);
                }
            }
            modal.Phyoffduty = phyoffduty;
            modal.Phyoncall = phyoncall;
            return modal;
        }

        public async Task<Scheduling> ProvidersOnCallbyRegion(int regionid, List<int> oncall, List<int> offcall)
        {
            Scheduling modal = new Scheduling();
            var listoncall = new List<Physician>();
            var listoffcall = new List<Physician>();
            foreach (var obj in oncall)
            {
                var phy = new Physician();
                if (regionid == 0)
                {
                    phy = await _context.Physicians.FirstOrDefaultAsync(u => u.Physicianid == obj);
                }
                else
                {
                    phy = await _context.Physicians.FirstOrDefaultAsync(u => u.Physicianid == obj && u.Regionid == regionid);
                }
                if (phy != null)
                {
                    listoncall.Add(phy);
                }
            }
            foreach (var obj in offcall)
            {
                var phy = new Physician();
                if (regionid == 0)
                {
                    phy = await _context.Physicians.FirstOrDefaultAsync(u => u.Physicianid == obj);
                }
                else
                {
                    phy = await _context.Physicians.FirstOrDefaultAsync(u => u.Physicianid == obj && u.Regionid == regionid);
                }
                if (phy != null)
                {
                    listoffcall.Add(phy);
                }
            }
            modal.Phyoncall = listoncall;
            modal.Phyoffduty = listoffcall;
            return modal;
        }

        public async Task<ShiftforReviewModal> ShiftForReview()
        {
            ShiftforReviewModal modal = new ShiftforReviewModal();
            modal.regions = await _context.Regions.ToListAsync();
            modal.shiftdetail = await _context.Shiftdetails.Include(u => u.Shiftdetailregions).ThenInclude(u => u.Region).Include(u => u.Shift).ThenInclude(u => u.Physician).Where(u => u.Status == 0 && u.Isdeleted == new BitArray(new[] { false })).ToListAsync();
            modal.totalpages = (int)Math.Ceiling(modal.shiftdetail.Count() / 10.00);
            modal.shiftdetail = modal.shiftdetail.Skip((1 - 1) * 10).Take(10).ToList();
            modal.currentpage = 1;
            return modal;
        }
        public async Task<ShiftforReviewModal> ShiftReviewTable(int currentPage, int regionid)
        {
            ShiftforReviewModal modal = new ShiftforReviewModal();
            modal.regions = await _context.Regions.ToListAsync();
            modal.shiftdetail = await _context.Shiftdetails.Include(u => u.Shiftdetailregions).ThenInclude(u => u.Region).Include(u => u.Shift).ThenInclude(u => u.Physician).Where(u => u.Status == 0 && u.Isdeleted == new BitArray(new[] { false })).ToListAsync();
            if (regionid != 0)
            {
                modal.shiftdetail = modal.shiftdetail.Where(u => u.Regionid == regionid && u.Status == 0).ToList();
            }
            modal.totalpages = (int)Math.Ceiling(modal.shiftdetail.Count() / 10.00);
            modal.shiftdetail = modal.shiftdetail.Skip((currentPage - 1) * 10).Take(10).ToList();
            modal.currentpage = currentPage;
            modal.regionid = regionid;
            return modal;
        }

        public async Task ApproveSelected(int[] shiftchk, string aspNetUserId)
        {
            foreach (var obj in shiftchk)
            {
                var shiftdetail = await _context.Shiftdetails.FirstOrDefaultAsync(u => u.Shiftdetailid == obj);
                shiftdetail.Status = 1;
                shiftdetail.Modifieddate = DateTime.Now;
                shiftdetail.Modifiedby = aspNetUserId;
                _context.Shiftdetails.Update(shiftdetail);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteSelected(int[] shiftchk, string aspNetUserId)
        {
            foreach (var obj in shiftchk)
            {
                var shiftdetail = await _context.Shiftdetails.FirstOrDefaultAsync(u => u.Shiftdetailid == obj);
                shiftdetail.Isdeleted = new BitArray(new[] { true });
                shiftdetail.Modifieddate = DateTime.Now;
                shiftdetail.Modifiedby = aspNetUserId;
                _context.Shiftdetails.Update(shiftdetail);
                await _context.SaveChangesAsync();
                
                var shiftregion = await _context.Shiftdetailregions.FirstOrDefaultAsync(u => u.Shiftdetailid == obj);
                shiftregion.Isdeleted = new BitArray(new[] { true });
                _context.Shiftdetailregions.Update(shiftregion);
                await _context.SaveChangesAsync();
            }
        }

        public byte[] DownloadSearchRecordExcle(SearchRecordsData model)
        {
            using (var workbook = new XSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet("FilteredRecord");
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Sr No.");
                headerRow.CreateCell(1).SetCellValue("Patient Name");
                headerRow.CreateCell(2).SetCellValue("Requestor");
                headerRow.CreateCell(3).SetCellValue("Date Of Service");
                headerRow.CreateCell(4).SetCellValue("Close Case Date");
                headerRow.CreateCell(5).SetCellValue("Email");
                headerRow.CreateCell(6).SetCellValue("Phone Number");
                headerRow.CreateCell(7).SetCellValue("Address");
                headerRow.CreateCell(8).SetCellValue("Zip");
                headerRow.CreateCell(9).SetCellValue("Request Status");
                headerRow.CreateCell(10).SetCellValue("Physician");
                headerRow.CreateCell(11).SetCellValue("Physician Note");
                headerRow.CreateCell(12).SetCellValue("Cancelled By Provider Note");
                headerRow.CreateCell(13).SetCellValue("Admin Note");
                headerRow.CreateCell(14).SetCellValue("Patient Note");

                for (int i = 0; i < model.requestclients.Count; i++)
                {
                    var reqclient = model.requestclients.ElementAt(i);
                    var status = "";
                    if (reqclient.Request.Status == 1)
                    {
                        status = "Unassigned";
                    }
                    else if (reqclient.Request.Status == 2)
                    {
                        status = "Accepted";
                    }
                    else if (reqclient.Request.Status == 3)
                    {
                        status = "cancelled";
                    }
                    else if (reqclient.Request.Status == 4)
                    {
                        status = "MDEnroute";
                    }
                    else if (reqclient.Request.Status == 5)
                    {
                        status = "MDOnSite";
                    }
                    else if (reqclient.Request.Status == 6)
                    {
                        status = "Conclude";
                    }
                    else if (reqclient.Request.Status == 7)
                    {
                        status = "Cancelled By Patient";
                    }
                    else if (reqclient.Request.Status == 8)
                    {
                        status = "Closed";
                    }
                    else if (reqclient.Request.Status == 9)
                    {
                        status = "Unpaid";
                    }
                    else if (reqclient.Request.Status == 10)
                    {
                        status = "Clear";
                    }

                    IRow row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(i + 1);
                    row.CreateCell(1).SetCellValue(reqclient.Firstname);
                    row.CreateCell(2).SetCellValue(reqclient.Request.Firstname);
                    if (reqclient.Request.Accepteddate != null)
                    {
                        row.CreateCell(3).SetCellValue(reqclient.Request.Accepteddate.Value.ToString("MMM dd yyyy"));
                    }
                    else
                    {
                        row.CreateCell(3).SetCellValue("-");
                    }
                    if (reqclient.Request.Status == 3 || reqclient.Request.Status == 7 || reqclient.Request.Status == 8)
                    {
                        row.CreateCell(4).SetCellValue(reqclient.Request.Requeststatuslogs.LastOrDefault(u => u.Requestid == reqclient.Request.Requestid)!.Createddate.ToString("MMM dd yyyy"));
                    }
                    else
                    {
                        row.CreateCell(4).SetCellValue("-");
                    }

                    row.CreateCell(5).SetCellValue(reqclient.Email);
                    row.CreateCell(6).SetCellValue(reqclient.Phonenumber);
                    row.CreateCell(7).SetCellValue(reqclient.Address);
                    row.CreateCell(8).SetCellValue(reqclient.Zipcode);
                    row.CreateCell(9).SetCellValue(status);
                    if (reqclient.Request.Physician != null)
                    {
                        row.CreateCell(10).SetCellValue(reqclient.Request.Physician.Firstname);
                    }
                    else
                    {
                        row.CreateCell(10).SetCellValue("-");
                    }
                    if (reqclient.Request.Requestnotes.Count() > 0 && reqclient.Request.Requestnotes.ElementAt(0).Physiciannotes != null)
                    {
                        row.CreateCell(11).SetCellValue(reqclient.Request.Requestnotes.ElementAt(0).Physiciannotes);
                    }
                    else
                    {
                        row.CreateCell(11).SetCellValue("-");
                    }
                    if (reqclient.Request.Status == 3 || reqclient.Request.Status == 7 || reqclient.Request.Status == 8)
                    {
                        row.CreateCell(12).SetCellValue(reqclient.Request.Requeststatuslogs.LastOrDefault(u => u.Requestid == reqclient.Request.Requestid)!.Notes);
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue("-");
                    }
                    if (reqclient.Request.Requestnotes.Count() > 0)
                    {
                        row.CreateCell(13).SetCellValue(reqclient.Request.Requestnotes.ElementAt(0).Adminnotes);
                    }
                    else
                    {
                        row.CreateCell(13).SetCellValue("-");
                    }
                    if (reqclient.Notes != null)
                    {
                        row.CreateCell(14).SetCellValue(@reqclient.Notes);
                    }
                    else
                    {
                        row.CreateCell(14).SetCellValue("-");
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        public async Task<(string, string)> FindUser(LoginPerson model)
        {
            if (model == null)
            {
                return ("", "");
            }

            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(u => u.Email == model.email);

            return (aspnetuser!.Id, aspnetuser.Email)!;

        }

        public async Task UpdatePassword(LoginPerson model)
        {
            if (model == null)
            {
                return;
            }
            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Id == model.aspNetUserId);
            string encryptedPassword = EncryptDecryptHelper.Encrypt(model.password);
            aspnetuser.Passwordhash = encryptedPassword;
            _context.Update(aspnetuser);
            await _context.SaveChangesAsync();
        }

    }
}
