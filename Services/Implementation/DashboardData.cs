using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
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
                    dataobj.totalPages = (int)Math.Ceiling(reqc.Count() / 2.00);
                    dataobj.currentpage = obj.requestedPage;
                    reqc = reqc.Skip((obj.requestedPage - 1) * 2).Take(2).ToList();
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
                    reqc = reqc.Where(a => a.Request.Firstname.ToLower().Contains(obj.searchKey.ToLower()) || a.Request.Lastname.ToLower().Contains(obj.searchKey.ToLower())).ToList();
                }
                if (obj.requestedPage != 0)
                {
                    dataobj.totalPages = (int)Math.Ceiling(reqc.Count() / 2.00);
                    dataobj.currentpage = obj.requestedPage;
                    reqc = reqc.Skip((obj.requestedPage - 1) * 2).Take(2).ToList();
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
                    reqc = reqc.Where(a => a.Request.Firstname.ToLower().Contains(obj.searchKey.ToLower()) || a.Request.Lastname.ToLower().Contains(obj.searchKey.ToLower())).ToList();
                }
                if (obj.requestedPage != 0)
                {
                    dataobj.totalPages = (int)Math.Ceiling(reqc.Count() / 2.00);
                    dataobj.currentpage = obj.requestedPage;
                    reqc = reqc.Skip((obj.requestedPage - 1) * 2).Take(2).ToList();
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
                    reqc = reqc.Where(a => a.Request.Firstname.ToLower().Contains(obj.searchKey.ToLower()) || a.Request.Lastname.ToLower().Contains(obj.searchKey.ToLower())).ToList();
                }
                if (obj.requestedPage != 0)
                {
                    dataobj.totalPages = (int)Math.Ceiling(reqc.Count() / 2.00);
                    dataobj.currentpage = obj.requestedPage;
                    reqc = reqc.Skip((obj.requestedPage - 1) * 2).Take(2).ToList();
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
            if(patientName != null)
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
            adminData.admin = await _context.Admins.FirstOrDefaultAsync(a => a.Adminid == adminId);
            List<Region> regionList = await _context.Regions.ToListAsync();
            adminData.regionlist = regionList;
            List<Adminregion> adminRegions = await _context.Adminregions.Where(a => a.Adminid == adminId).ToListAsync();
            adminData.adminregionlist = adminRegions;
            return adminData;
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

        public ProviderViewModel ProviderData(int regionId)
        {
            List<Physician> physicinaData = _context.Physicians.Include(a => a.Role).ToList();
            if (regionId != 0)
            {
                physicinaData = physicinaData.Where(a => a.Regionid == regionId).ToList();
            }
            List<int> phynotificationid = _context.Physiciannotifications.Where(u => u.Isnotificationstopped == new BitArray(new[] { true })).Select(u => u.Pysicianid).ToList();
            ProviderViewModel obj = new ProviderViewModel();
            obj.physician = physicinaData;
            obj.physiciannotificationid = phynotificationid;
            return obj;
        }

        public async Task ToStopNotification(List<int> toStopNotifications, List<int> toNotification)
        {
            foreach (var item in toStopNotifications)
            {
                Physiciannotification physiciannotification = _context.Physiciannotifications.FirstOrDefault(a => a.Pysicianid == item);
                if (physiciannotification != null)
                {
                    physiciannotification.Isnotificationstopped = new BitArray(new[] { true });
                    _context.Physiciannotifications.Update(physiciannotification);
                }
            }
            foreach (var item in toNotification)
            {
                Physiciannotification physiciannotification = _context.Physiciannotifications.FirstOrDefault(a => a.Pysicianid == item);
                if (physiciannotification != null)
                {
                    physiciannotification.Isnotificationstopped = new BitArray(new[] { false });
                    _context.Physiciannotifications.Update(physiciannotification);
                }
            }
            await _context.SaveChangesAsync();
        }

        public EditProviderViewModel EditProvider(int physicianId)
        {
            Physician physician = _context.Physicians.FirstOrDefault(a => a.Physicianid == physicianId);
            List<Region> regions = _context.Regions.ToList();
            List<Physicianregion> physicianregions = _context.Physicianregions.Where(a => a.Physicianid == physicianId).ToList();
            EditProviderViewModel editProviderViewModel = new EditProviderViewModel
            {
                firstName = physician.Firstname,
                lastName = physician.Lastname,
                email = physician.Email,
                contactNumber = physician.Mobile,
                medicalLecense = physician.Medicallicense,
                NPINumber = physician.Npinumber,
                syncEmail = physician.Syncemailaddress,
                address1 = physician.Address1,
                address2 = physician.Address2,
                city = physician.City,
                state = _context.Regions.FirstOrDefault(a => a.Regionid == physician.Regionid).Name,
                zipcode = physician.Zip,
                billingContact = physician.Altphone,
                businessName = physician.Businessname,
                businessSite = physician.Businesswebsite,
                regionList = regions,
                physicianRegionlist = physicianregions,
                providerId = physicianId,
                photoName = physician.Photo,
                signName = physician.Signature
            };
            if (physician.Isagreementdoc[0] == true)
            {
                editProviderViewModel.IsAgreementDoc = true;
            }
            if (physician.Isnondisclosuredoc[0] == true)
            {
                editProviderViewModel.IsNonDisclosureDoc = true;
            }
            return editProviderViewModel;
        }

        public async Task UpdatePhysicianInfo(EditProviderViewModel obj, List<int> selectedRegion)
        {
            Physician physician = _context.Physicians.FirstOrDefault(a => a.Physicianid == obj.providerId);
            if (physician != null)
            {
                physician.Firstname = obj.firstName;
                physician.Lastname = obj.lastName;
                physician.Email = obj.email;
                physician.Mobile = obj.contactNumber;
                physician.Syncemailaddress = obj.syncEmail;
                physician.Medicallicense = obj.medicalLecense;
                physician.Npinumber = obj.NPINumber;
                physician.Modifieddate = DateTime.Now;
                List<Physicianregion> physicianRegions = _context.Physicianregions.Where(a => a.Physicianid == obj.providerId).ToList();
                foreach (var item in physicianRegions)
                {
                    _context.Physicianregions.Remove(item);
                }

                if (selectedRegion.Count() > 0)
                {
                    foreach (var item in selectedRegion)
                    {
                        Physicianregion newphysicianRegions = new Physicianregion();
                        newphysicianRegions.Physicianid = obj.providerId;
                        newphysicianRegions.Regionid = item;
                        _context.Physicianregions.Add(newphysicianRegions);
                    }
                }
                _context.Update(physician);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBillingInfo(EditProviderViewModel obj)
        {
            Physician? physician = _context.Physicians.FirstOrDefault(a => a.Physicianid == obj.providerId);
            if (physician != null)
            {
                physician.Address1 = obj.address1;
                physician.Address2 = obj.address2;
                physician.City = obj.city;
                physician.Zip = obj.zipcode;
                physician.Altphone = obj.billingContact;
                physician.Modifieddate = DateTime.Now;
                _context.Update(physician);
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task UpdateProfile(EditProviderViewModel obj)
        {
            Physician? physician = _context.Physicians.FirstOrDefault(a => a.Physicianid == obj.providerId);
            if (physician != null)
            {
                physician.Businessname = obj.businessName;
                physician.Businesswebsite = obj.businessSite;
                physician.Adminnotes = obj.adminnote;
                physician.Modifieddate = DateTime.Now;
                if (obj.photo != null)
                {
                    physician.Photo = obj.photo.FileName;
                    string path = _env.WebRootPath + "/upload/" + obj.photo.FileName;
                    FileStream stream = new FileStream(path, FileMode.Create);
                    obj.photo.CopyTo(stream);
                }
                if (obj.signature != null)
                {
                    physician.Signature = obj.signature.FileName;
                    string signpath = _env.WebRootPath + "/upload/" + obj.signature.FileName;
                    FileStream signstream = new FileStream(signpath, FileMode.Create);
                    obj.signature.CopyTo(signstream);
                }
                _context.Update(physician);
                await _context.SaveChangesAsync();
            }
        }

        public RoleAccess CreateAccessRole(int accountType)
        {
            List<Menu> menuList = new List<Menu>();
            if (accountType == 0)
            {
                menuList = _context.Menus.ToList();
            }
            else
            {
                menuList = _context.Menus.Where(a => a.Accounttype == accountType).ToList();
            }
            RoleAccess roleAccess = new RoleAccess();
            roleAccess.menuList = menuList;
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

        public async Task<PartnerViewModel> PartnerData(int professionType)
        {
            PartnerViewModel obj = new PartnerViewModel();
            if(professionType == 0)
            {
                obj.professionList = await _context.Healthprofessionals.Include(a=>a.ProfessionNavigation).ToListAsync();
            }
            else
            {
                obj.professionList = await _context.Healthprofessionals.Include(a => a.ProfessionNavigation).Where(a=>a.ProfessionNavigation.Healthprofessionalid == professionType).ToListAsync();

            }
            return obj;
        }

        public async Task<PartnerViewModel> Partners()
        {
            PartnerViewModel obj = new PartnerViewModel();
            obj.professionTypeList  = await _context.Healthprofessionaltypes.ToListAsync();
            return obj;
        }
    }
}
