using Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class PartnerViewModel
    {
        public List<Healthprofessional> professionList { get; set; }

        public List<Healthprofessionaltype> professionTypeList { get; set; }
    }

    public class BusinessData
    {
        public int vendorId { get; set; }

        [Required(ErrorMessage = "*First Name is required")]
        public string businessName { get; set; }

        [Required(ErrorMessage = "Select profession type")]
        public int professionType { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit fax number")]
        [Required(ErrorMessage = "fax number is required")]
        public string faxNumber { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Mobile number is required")]
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit contact number")]
        [Required(ErrorMessage = "business contact is required")]
        public string businessContact { get; set; }

        [Required(ErrorMessage = "*Street is required")]
        public string street { get; set; }

        [Required(ErrorMessage = "*City is required")]
        public string city { get; set; }

        [Required(ErrorMessage = "*State is required")]
        public string state { get; set; }

        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*zip code is required")]
        public string zip { get; set; }

        public List<Healthprofessionaltype> professionTypeList {  get; set; }
    }
}
