using Common.Helper;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace Services.Implementation
{
    public class ProviderServices : IProviderServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public ProviderServices(HalloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<Region>> GetRegions()
        {
            return await _context.Regions.ToListAsync();
        }

        public async Task<ProviderViewModel> ProviderData(int regionId)
        {
            List<Physician> physicinaData = await _context.Physicians.Include(a => a.Role).ToListAsync();
            if (regionId != 0)
            {
                physicinaData = physicinaData.Where(a => a.Regionid == regionId).ToList();
            }
            List<int> phynotificationid = await _context.Physiciannotifications.Where(u => u.Isnotificationstopped == new BitArray(new[] { true })).Select(u => u.Pysicianid).ToListAsync();
            ProviderViewModel obj = new ProviderViewModel();
            obj.physician = physicinaData;
            obj.physiciannotificationid = phynotificationid;
            return obj;
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
            if (physician.Isagreementdoc != null && physician.Isagreementdoc[0] == true)
            {
                editProviderViewModel.IsAgreementDoc = true;
            }
            if (physician.Isnondisclosuredoc != null && physician.Isnondisclosuredoc[0] == true)
            {
                editProviderViewModel.IsNonDisclosureDoc = true;
            }
            if(physician.Isbackgrounddoc != null && physician.Isbackgrounddoc[0] == true)
            {
                editProviderViewModel.IsBackgroundDoc = true;
            }
            if(physician.Islicensedoc != null && physician.Islicensedoc[0] == true)
            {
                editProviderViewModel.IsLicenseDoc = true;
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

        public async Task UploadNewDocument(EditProviderViewModel obj)
        {
            int physicianId = obj.providerId;
            Physician? insertedPhysician = _context.Physicians.FirstOrDefault(a => a.Physicianid == physicianId);
            if (insertedPhysician != null)
            {
                if (obj.agreementDoc != null)
                {
                    string fileName = physicianId + "_agreement.pdf";
                    UploadDocument(fileName, obj.agreementDoc);
                    insertedPhysician.Isagreementdoc = new BitArray(new[] { true });
                }
                if (obj.backgroundDoc != null)
                {
                    string fileName = physicianId + "_background.pdf";
                    UploadDocument(fileName, obj.backgroundDoc);
                    insertedPhysician.Isbackgrounddoc = new BitArray(new[] { true });
                }
                if (obj.nonDisclosureDoc != null)
                {
                    string fileName = physicianId + "_nonDisclosure.pdf";
                    UploadDocument(fileName, obj.nonDisclosureDoc);
                    insertedPhysician.Isnondisclosuredoc = new BitArray(new[] { true });
                }
                if(obj.licenseDoc != null)
                {
                    string fileName = physicianId + "_license.pdf";
                    UploadDocument(fileName, obj.licenseDoc);
                    insertedPhysician.Islicensedoc = new BitArray(new[] { true });
                }
                _context.Update(insertedPhysician);
                await _context.SaveChangesAsync();
            }
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

        public async Task<EditProviderViewModel> CreateProvider()
        {
            EditProviderViewModel obj = new EditProviderViewModel();
            obj.regionList = await _context.Regions.ToListAsync();
            obj.rolesList = await _context.Roles.Where(a => a.Accounttype == 2).ToListAsync();
            return obj;
        }

        public async Task<string> GetLocations()
        {
            List<Physicianlocation> physicianlocationsList = await _context.Physicianlocations.ToListAsync();
            List<Physician> physiciansList = await _context.Physicians.ToListAsync();
            List<ProviderLocationViewModel> locations = new List<ProviderLocationViewModel>();
            foreach (var physicianLocation in physicianlocationsList)
            {
                locations.Add(new ProviderLocationViewModel
                {
                    Photo = physiciansList.FirstOrDefault(x => x.Physicianid == physicianLocation.Physicianid).Photo,
                    Lat = physicianLocation.Latitude.ToString(),
                    Long = physicianLocation.Longitude.ToString(),
                    Physicianid = physicianLocation.Physicianid.ToString(),
                    Name = physiciansList.FirstOrDefault(m => m.Physicianid == physicianLocation.Physicianid).Firstname + physiciansList.FirstOrDefault(m => m.Physicianid == physicianLocation.Physicianid).Lastname,
                });
            }

            return locations.ToJson();
        }

        public async Task CreateProviderAccount(EditProviderViewModel obj, List<int> selectedRegion, int adminId)
        {

            string encryptedPassword = EncryptDecryptHelper.Encrypt(obj.password);
            Aspnetuser aspnetuser = new Aspnetuser
            {
                Id = Guid.NewGuid().ToString(),
                Username = obj.userName,
                Passwordhash = encryptedPassword,
                Email = obj.email,
                Phonenumber = obj.contactNumber,
                Createddate = DateTime.Now,
                Modifieddate = DateTime.Now,
            };
            await _context.AddAsync(aspnetuser);
            await _context.SaveChangesAsync();

            Physician physician = new Physician
            {
                Aspnetuserid = aspnetuser.Id,
                Firstname = obj.firstName,
                Lastname = obj.lastName,
                Email = obj.email,
                Mobile = obj.contactNumber,
                Medicallicense = obj.medicalLecense,
                Adminnotes = obj.adminnote,
                Address1 = obj.address1,
                Address2 = obj.address2,
                City = obj.city,
                Regionid = obj.physicianRegion,
                Zip = obj.zipcode,
                Altphone = obj.billingContact,
                Createdby = adminId.ToString(),
                Createddate = DateTime.Now,
                Modifieddate = DateTime.Now,
                Businessname = obj.businessName,
                Businesswebsite = obj.businessSite,
                Npinumber = obj.NPINumber,
                Roleid = obj.role,
                Islicensedoc = new BitArray(new[] { false })
            };
            if (obj.photo != null)
            {
                physician.Photo = obj.photo.FileName;
                UploadDocument(obj.photo.FileName, obj.photo);
            }
            if (obj.agreementDoc == null)
            {
                physician.Isagreementdoc = new BitArray(new[] { false });
            }
            if (obj.backgroundDoc == null)
            {
                physician.Isbackgrounddoc = new BitArray(new[] { false });
            }
            if (obj.nonDisclosureDoc == null)
            {
                physician.Isnondisclosuredoc = new BitArray(new[] { false });
            }
            await _context.AddAsync(physician);
            await _context.SaveChangesAsync();

            int physicianId = physician.Physicianid;
            Physician? insertedPhysician = _context.Physicians.FirstOrDefault(a => a.Physicianid == physicianId);
            if (insertedPhysician != null)
            {

                if (obj.agreementDoc != null)
                {
                    string fileName = physicianId + "_agreement.pdf";
                    UploadDocument(fileName, obj.agreementDoc);
                    insertedPhysician.Isagreementdoc = new BitArray(new[] { true });
                }
                if (obj.backgroundDoc != null)
                {
                    string fileName = physicianId + "_background.pdf";
                    UploadDocument(fileName, obj.backgroundDoc);
                    insertedPhysician.Isbackgrounddoc = new BitArray(new[] { true });
                }
                if (obj.nonDisclosureDoc != null)
                {
                    string fileName = physicianId + "_nonDisclosure.pdf";
                    UploadDocument(fileName, obj.nonDisclosureDoc);
                    insertedPhysician.Isnondisclosuredoc = new BitArray(new[] { true });
                }
                _context.Update(insertedPhysician);
            }

            Physiciannotification physiciannotification = new Physiciannotification();
            physiciannotification.Pysicianid = physicianId;
            physiciannotification.Isnotificationstopped = new BitArray(new[] { false });
            await _context.AddAsync(physiciannotification);

            if (selectedRegion.Count() > 0)
            {
                foreach (var item in selectedRegion)
                {
                    Physicianregion physicianregion = new Physicianregion();
                    physicianregion.Physicianid = physicianId;
                    physicianregion.Regionid = item;
                    await _context.AddAsync(physicianregion);
                }
            }

            await _context.SaveChangesAsync();
        }

        public void UploadDocument(string fileName, IFormFile file)
        {
            string path = _env.WebRootPath + "/upload/" + fileName;
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }
    }
}
