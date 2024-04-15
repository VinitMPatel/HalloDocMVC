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

        public async Task<CaseActionDetails> ViewCaseData(int requestId)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(m => m.Requestid == requestId);
            var requestclient = await _context.Requestclients.FirstOrDefaultAsync(m => m.Requestid == requestId);
            var regiondata = await _context.Regions.FirstOrDefaultAsync(m => m.Regionid == requestclient.Regionid);
            var regionList = await _context.Regions.ToListAsync();

            var data = new CaseActionDetails
            {
                //ConfirmationNumber = request.Confirmationnumber,
                requestId = requestId,
                PatientNotes = requestclient.Notes,
                FirstName = request.Firstname,
                LastName = request.Lastname,
                Email = request.Email,
                DOB = new DateTime(Convert.ToInt32(requestclient.Intyear), DateTime.ParseExact(requestclient.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(requestclient.Intdate)),
                PhoneNumber = request.Phonenumber,
                Region = regiondata.Name,
                regionList = regionList,
                Address = requestclient.Address,
                requestType = request.Requesttypeid,
                ConfirmationNumber = request.Confirmationnumber
            };
            return data;
        }

        public async Task<CaseActionDetails> ViewNotes(int requestId)
        {
            CaseActionDetails obj = new CaseActionDetails();
            obj.requestId = requestId;

            var requestnote = await _context.Requestnotes.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if (requestnote != null)
            {
                obj.adminNote = requestnote.Adminnotes!;
            }

            List<Requeststatuslog> requeststatuslogs = await _context.Requeststatuslogs.Where(a => a.Requestid == obj.requestId).ToListAsync();
            obj.requeststatuslogs = requeststatuslogs;
            return obj;
        }
        

        public async Task<List<Physician>> PhysicianList(int regionid)
        {
            List<Physician> physicianList = await _context.Physicians.Where(a => a.Regionid == regionid).ToListAsync();
            return physicianList;
        }

        public async Task<CaseActionDetails> ViewUploads(int requestId)
        {
            CaseActionDetails obj = new CaseActionDetails();
            //List<Requestwisefile> files = _context.Requestwisefiles(a => a.requestId == requestId).ToList();
            List<Requestwisefile> files = await (from m in _context.Requestwisefiles where m.Requestid == requestId && m.Isdeleted != new BitArray(new[] { true }) select m).ToListAsync();
            var patientName = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);
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

        public async Task<AdminProfile> AdminProfileData(int adminId)
        {
            AdminProfile adminData = new AdminProfile();

            Admin? admin = await _context.Admins.FirstOrDefaultAsync(a => a.Adminid == adminId);
            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Id == admin.Aspnetuserid);
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
            adminData.adminregionlist = await _context.Adminregions.Where(a => a.Adminid == adminId).ToListAsync();
            adminData.rolesList = await _context.Roles.Where(a => a.Accounttype == 1).ToListAsync();
            return adminData;
        }

        public async Task UpdateAdminPassword(int adminId , string password)
        {
            Admin? admin = await _context.Admins.FirstOrDefaultAsync(a => a.Adminid == adminId);
            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Id == admin!.Aspnetuserid);
            if(aspnetuser != null)
            {
                string encryptedPassword = EncryptDecryptHelper.Encrypt(password);
                aspnetuser.Passwordhash = encryptedPassword;
                _context.Update(aspnetuser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAdminInfo(int adminId, AdminInfo obj)
        {
            Admin? admin = _context.Admins.FirstOrDefault(a => a.Adminid == adminId);
            Aspnetuser? aspnetuser = _context.Aspnetusers.FirstOrDefault(a => a.Id == admin.Aspnetuserid);
            List<Adminregion> adminRegions = _context.Adminregions.Where(a => a.Adminid == adminId).ToList();
            foreach (var item in adminRegions)
            {
                _context.Adminregions.Remove(item);
            }

            if (obj.selectedregion != null)
            {
                foreach (var item in obj.selectedregion)
                {
                    Adminregion newAdminRegion = new Adminregion();
                    newAdminRegion.Adminid = adminId;
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

        public async Task UpdateBillingInfo(int adminId, BillingInfo obj)
        {
            Admin? admin = _context.Admins.FirstOrDefault(a => a.Adminid == adminId);
            Aspnetuser? aspnetuser = _context.Aspnetusers.FirstOrDefault(a => a.Id == admin.Aspnetuserid);
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




        public async Task<RoleAccess> CreateAccessRole(int accountType , int roleId)
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

        public async Task AddNewRole(List<int> menus, short accountType, string roleName, int adminId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Role role = new Role
                    {
                        Name = roleName,
                        Accounttype = accountType,
                        Createdby = adminId.ToString(),
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

            requestclients = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Requestnotes).Include(a => a.Request.Requeststatuslogs).Include(a => a.Request.Requesttype).ToListAsync();

            requestclients = requestclients.Where(a =>
                                (obj.selectedStatus == 0 || a.Request.Status == obj.selectedStatus) &&
                                (string.IsNullOrWhiteSpace(obj.searchedPatient) || a.Firstname.ToLower().Contains(obj.searchedPatient.ToLower()) || a.Lastname.ToLower().Contains(obj.searchedPatient.ToLower())) &&
                                (obj.selectedType == 0 || a.Request.Requesttypeid == obj.selectedType) &&
                                (string.IsNullOrWhiteSpace(obj.searchedProvider) || a.Request.Physicianid != null && a.Request.Physician.Firstname.ToLower().Contains(obj.searchedProvider.ToLower())) &&
                                (string.IsNullOrWhiteSpace(obj.searchedEmail) || a.Email.ToLower().Contains(obj.searchedEmail.ToLower())) &&
                                (string.IsNullOrWhiteSpace(obj.searchedPhone) || a.Phonenumber.Contains(obj.searchedPhone))).ToList();

            dataObj.totalPages = (int)Math.Ceiling(requestclients.Count() / (double)obj.totalEntity);
            dataObj.currentpage = obj.requestedPage;
            requestclients = requestclients.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();

            dataObj.requestclients = requestclients;
            return dataObj;
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

        public async Task<BlockedHistory> GetBlockHistoryData(BlockedHistory obj)
        {
            BlockedHistory dataObj = new BlockedHistory();
            List<Requestclient> requestclients = new List<Requestclient>();

            requestclients = await _context.Requestclients.Include(a => a.Request).Include(a=>a.Request.Blockrequests).Where(a=>a.Request.Status == 11).ToListAsync();

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
            Blockrequest? blockData = await _context.Blockrequests.FirstOrDefaultAsync(a=>a.Requestid == requestId);
            if(blockData != null)
            {
                blockData.Isactive = new BitArray(new[] { true });
                _context.Update(blockData);
                await _context.SaveChangesAsync();
            }
        }

    }
}
