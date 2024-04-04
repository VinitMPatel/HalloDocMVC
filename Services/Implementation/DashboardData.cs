using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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


        public AdminDashboard AllData()
        {
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).ToList();
            List<Region> regionList = _context.Regions.ToList();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            obj.regionlist = regionList;
            return obj;
        }

        public AdminDashboard AllStateData(AdminDashboard obj)
        {

            if (obj.requestType == 0 && obj.regionId == 0)
            {
                List<Requestclient> reqc = new List<Requestclient>();
                AdminDashboard dataobj = new AdminDashboard();

                if (obj.requeststatus == 4)
                {
                    reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == 4 || a.Request.Status == 5).ToList();
                }
                else if (obj.requeststatus == 3)
                {
                    reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.User.Region).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == 3 || a.Request.Status == 7 || a.Request.Status == 8).ToList();
                }
                else
                {
                    reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status == obj.requeststatus).ToList();
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

        public CaseActionDetails ViewCaseData(int requestId)
        {
            var request = _context.Requests.FirstOrDefault(m => m.Requestid == requestId);
            var requestclient = _context.Requestclients.FirstOrDefault(m => m.Requestid == requestId);
            var regiondata = _context.Regions.FirstOrDefault(m => m.Regionid == requestclient.Regionid);
            var regionList = _context.Regions.ToList();
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

        public List<Physician> PhysicianList(int regionid)
        {
            List<Physician> physicianList = _context.Physicians.Where(a => a.Regionid == regionid).ToList();
            return physicianList;
        }

        public CaseActionDetails ViewUploads(int requestId)
        {
            CaseActionDetails obj = new CaseActionDetails();
            //List<Requestwisefile> files = _context.Requestwisefiles(a => a.requestId == requestId).ToList();
            List<Requestwisefile> files = (from m in _context.Requestwisefiles where m.Requestid == requestId && m.Isdeleted != new BitArray(new[] { true }) select m).ToList();
            var patientName = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault().Firstname;
            obj.FirstName = patientName;
            obj.requestId = requestId;
            obj.requestwisefile = files;
            return obj;
        }

        public void UplodingDocument(List<IFormFile> myfile, int requestId)
        {
            if (myfile.Count() > 0)
            {
                uploadFile(myfile, requestId);
            }
        }

        public void uploadFile(List<IFormFile> upload, int id)
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
                _context.Add(requestwisefile);
                _context.SaveChanges();
            }
        }

        public void SingleDelete(int reqfileid)
        {
            var requestwisefile = _context.Requestwisefiles.FirstOrDefault(u => u.Requestwisefileid == reqfileid);
            int reqid = requestwisefile.Requestid;
            requestwisefile.Isdeleted = new BitArray(new[] { true });
            _context.Requestwisefiles.Update(requestwisefile);
            _context.SaveChanges();
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

        public AdminProfile AdminProfileData(int adminId)
        {
            AdminProfile adminData = new AdminProfile();
            adminData.admin = _context.Admins.FirstOrDefault(a => a.Adminid == adminId);
            List<Region> regionList = _context.Regions.ToList();
            adminData.regionlist = regionList;
            List<Adminregion> adminRegions = _context.Adminregions.Where(a => a.Adminid == adminId).ToList();
            adminData.adminregionlist = adminRegions;
            return adminData;
        }
        public void UpdateAdminInfo(int adminId, AdminInfo obj)
        {
            Admin admin = _context.Admins.FirstOrDefault(a => a.Adminid == adminId);
            Aspnetuser aspnetuser = _context.Aspnetusers.FirstOrDefault(a => a.Id == admin.Aspnetuserid);
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
            admin.Firstname = obj.firstName;
            admin.Lastname = obj.lastName;
            admin.Email = obj.email;
            admin.Mobile = obj.contact;
            admin.Modifieddate = DateTime.Now;
            admin.Modifiedby = aspnetuser.Id;

            aspnetuser.Phonenumber = obj.contact;
            aspnetuser.Username = obj.firstName + obj.lastName;
            aspnetuser.Email = obj.email;
            aspnetuser.Modifieddate = DateTime.Now;

            _context.Update(admin);
            _context.Update(aspnetuser);
            _context.SaveChanges();
        }

        public void UpdateBillingInfo(int adminId, BillingInfo obj)
        {
            Admin admin = _context.Admins.FirstOrDefault(a => a.Adminid == adminId);
            Aspnetuser aspnetuser = _context.Aspnetusers.FirstOrDefault(a => a.Id == admin.Aspnetuserid);

            admin.Address1 = obj.address1;
            admin.Address2 = obj.address2;
            admin.City = obj.city;
            admin.Zip = obj.zip;
            admin.Modifieddate = DateTime.Now;
            admin.Modifiedby = aspnetuser.Id;
            admin.Altphone = obj.billingContact;

            _context.Update(admin);
            _context.SaveChanges();
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

        public void ToStopNotification(List<int> toStopNotifications, List<int> toNotification)
        {
            foreach (var item in toStopNotifications)
            {
                Physiciannotification physiciannotification = _context.Physiciannotifications.FirstOrDefault(a => a.Pysicianid == item);
                physiciannotification.Isnotificationstopped = new BitArray(new[] { true });
                _context.Physiciannotifications.Update(physiciannotification);
            }
            foreach (var item in toNotification)
            {
                Physiciannotification physiciannotification = _context.Physiciannotifications.FirstOrDefault(a => a.Pysicianid == item);
                physiciannotification.Isnotificationstopped = new BitArray(new[] { false });
                _context.Physiciannotifications.Update(physiciannotification);
            }
            _context.SaveChanges();
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
            return editProviderViewModel;
        }

        public void UpdatePhysicianInfo(EditProviderViewModel obj, List<int> selectedRegion)
        {
            Physician physician = _context.Physicians.FirstOrDefault(a => a.Physicianid == obj.providerId);
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
            _context.SaveChanges();
        }

        public void UpdateBillingInfo(EditProviderViewModel obj)
        {
            Physician? physician = _context.Physicians.FirstOrDefault(a => a.Physicianid == obj.providerId);
            if(physician != null)
            {
                physician.Address1 = obj.address1;
                physician.Address2 = obj.address2;
                physician.City = obj.city;
                physician.Zip = obj.zipcode;
                physician.Altphone = obj.billingContact;
                physician.Modifieddate = DateTime.Now;
                _context.Update(physician);
                _context.SaveChanges();
            }
        }

        [HttpPost]
        public void UpdateProfile(EditProviderViewModel obj)
        {
            Physician? physician = _context.Physicians.FirstOrDefault(a => a.Physicianid == obj.providerId);
            if(physician != null)
            {
                physician.Businessname = obj.businessName;
                physician.Businesswebsite = obj.businessSite;
                physician.Adminnotes = obj.adminnote;
                physician.Modifieddate = DateTime.Now;
                physician.Photo = obj.photo.FileName;
                physician.Signature = obj.signature.FileName;

                string path = _env.WebRootPath + "/upload/" + obj.photo.FileName;
                FileStream stream = new FileStream(path, FileMode.Create);
                obj.photo.CopyTo(stream);

                string signpath = _env.WebRootPath + "/upload/" + obj.signature.FileName;
                FileStream signstream = new FileStream(signpath, FileMode.Create);
                obj.signature.CopyTo(signstream);

                _context.Update(physician);
                _context.SaveChanges();
            }
        }
    }
}
