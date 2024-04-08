using Common.Helper;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public EditProviderViewModel CreateProvider()
        {
            EditProviderViewModel obj = new EditProviderViewModel();
            obj.regionList = _context.Regions.ToList();
            return obj;
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
            };
            if(obj.photo != null)
            {
                physician.Photo = obj.photo.FileName;
                UploadDocument(obj.photo.FileName, obj.photo);
            }
            await _context.AddAsync(physician);
            await _context.SaveChangesAsync();

            int physicianId = physician.Physicianid;

            if(obj.agreementDoc != null)
            {
                string fileName = physicianId + "_agreement.pdf";
                UploadDocument(fileName, obj.agreementDoc);
                Physician? insertedPhysician = _context.Physicians.FirstOrDefault(a => a.Physicianid == physicianId);
                if (insertedPhysician != null)
                {
                    insertedPhysician.Isagreementdoc = new BitArray(new[] { true });
                    _context.Update(insertedPhysician);
                    _context.SaveChanges();
                }
            }
            if (obj.backgroundDoc != null)
            {
                string fileName = physicianId + "_background.pdf";
                UploadDocument(fileName, obj.backgroundDoc);
                Physician? insertedPhysician = _context.Physicians.FirstOrDefault(a => a.Physicianid == physicianId);
                if (insertedPhysician != null)
                {
                    insertedPhysician.Isagreementdoc = new BitArray(new[] { true });
                    _context.Update(insertedPhysician);
                    _context.SaveChanges();
                }
            }
            if (obj.nonDisclosureDoc != null)
            {
                string fileName = physicianId + "_nonDisclosure.pdf";
                UploadDocument(fileName, obj.nonDisclosureDoc);
                Physician? insertedPhysician = _context.Physicians.FirstOrDefault(a => a.Physicianid == physicianId);
                if (insertedPhysician != null)
                {
                    insertedPhysician.Isagreementdoc = new BitArray(new[] { true });
                    _context.Update(insertedPhysician);
                    _context.SaveChanges();
                }
            }

            Physiciannotification physiciannotification = new Physiciannotification();
            physiciannotification.Pysicianid = physicianId;
            physiciannotification.Isnotificationstopped = new BitArray(new[] { false });
            await _context.AddAsync(physiciannotification);

            if(selectedRegion.Count() > 0)
            {
                foreach(var item in selectedRegion)
                {
                    Physicianregion physicianregion = new Physicianregion();
                    physicianregion.Physicianid = physicianId;
                    physicianregion.Regionid = item;
                    await _context.AddAsync(physicianregion);
                }
            }

            await _context.SaveChangesAsync();
        }

        public void UploadDocument(string fileName , IFormFile file)
        {
            string path = _env.WebRootPath + "/upload/" + fileName;
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }
    }
}
