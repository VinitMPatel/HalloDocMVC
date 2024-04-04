using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
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

        public async Task SubmitCreateProvider(EditProviderViewModel obj, List<int> selectedRegion, int adminId)
        {
            Physician physician = new Physician
            {
                Aspnetuserid = adminId.ToString(),
                Firstname = obj.firstName,
                Lastname = obj.lastName,
                Email = obj.email,
                Mobile = obj.contactNumber,
                Medicallicense = obj.medicalLecense,
                Photo = obj.photo.FileName,
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
            await _context.AddAsync(physician);
            await _context.SaveChangesAsync();
            if (obj.agreementDoc != null)
            {
                physician.Isagreementdoc = new BitArray(new[] { true });
            }
            if (obj.backgroundDoc != null)
            {
                physician.Isbackgrounddoc = new BitArray(new[] { true });
            }
            if (obj.nonDisclosureDoc != null)
            {
                physician.Isnondisclosuredoc = new BitArray(new[] { true });
            }
            
        }
    }
}
