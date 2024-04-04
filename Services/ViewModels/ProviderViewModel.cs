using Data.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class ProviderViewModel
    {
        public List<Physician> physician { get; set; }

        public  List<Region> regions { get; set; }

        public List<int> physiciannotificationid { get; set;}

        public List<Region> regionlist { get; set; }
    }

    public class EditProviderViewModel
    {
        public int providerId { get; set; }

        [Required(ErrorMessage = "*User Name is required")]
        public string userName { get; set; }

        [Required(ErrorMessage = "*Password is required")]
        public string password { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*First Name is required")]
        public string firstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "*Email is required")]
        public string email { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your contact number")]
        public string contactNumber { get; set; }

        [Required(ErrorMessage = "Plese enter your medical license")]
        public string medicalLecense { get; set; }

        [Required(ErrorMessage = "Plese enter your NPI number")]
        public string NPINumber { get; set; }

        [Required(ErrorMessage = "*Sync Email is required")]
        public string syncEmail { get; set; }

        public List<Region> regionList { get; set; }

        [Required(ErrorMessage = "*Address is required")]
        public string address1 { get; set; }

        public string address2 { get; set; }

        [Required(ErrorMessage = "*City is required")]
        public string city { get; set; }

        [Required(ErrorMessage = "*State is required")]
        public string state { get; set; }

        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "*Enter a valid 6-digit Zip Code")]
        [Required(ErrorMessage = "*Plese enter zip code")]
        public string zipcode { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "*Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "*Plese enter billing contact number")]
        public string billingContact {  get; set; }

        [Required(ErrorMessage = "*Plese enter business name")]
        public string businessName { get; set; }

        [Required(ErrorMessage = "*Plese enter business site")]
        public string businessSite { get; set; }

        public List<Physicianregion> physicianRegionlist { get; set; }

        public bool IsAgreementDoc { get; set; }

        public bool IsBackgroundDoc { get; set; }

        public bool IsCredentialDoc { get; set; }

        public bool IsNonDisclosureDoc { get; set; }

        public bool IsLicenseDoc { get; set; }
        
        public IFormFile photo { get; set; }
   
        public IFormFile signature { get; set; }

        public IFormFile agreementDoc { get; set; }
        public IFormFile backgroundDoc { get; set; }
        public IFormFile HIPAADoc { get; set; }
        public IFormFile nonDisclosureDoc { get; set; }

        public string photoName { get; set; }

        public string signName { get; set; }

        [Required(ErrorMessage = "*Please add note")]
        public string adminnote { get; set; }

        [Required(ErrorMessage = "*Please select region")]
        public int physicianRegion {  get; set; }
    }
}
