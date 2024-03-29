using Data.Entity;
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

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zipcode { get; set; }

        public string billingContact {  get; set; }

        public string businessName { get; set; }

        public string businessSite { get; set; }

        public List<Physicianregion> physicianRegionlist { get; set; }

        public bool IsAgreementDoc { get; set; }

        public bool IsBackgroundDoc { get; set; }

        public bool IsCredentialDoc { get; set; }

        public bool IsNonDisclosureDoc { get; set; }

        public bool IsLicenseDoc { get; set; }

        public int[] selectedregion { get; set; }

        public string photo { get; set; }

        public string signature { get; set; }
        
        public string adminnote { get; set; }
    }
}
