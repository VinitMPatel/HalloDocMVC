using Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class AdminProfile
    {
        public Admin admin { get; set; }

        public int[] selectedregion { get; set; }

        public List<Adminregion> adminregionlist { get; set; }

        public string userName { get; set; }

        public string password { get; set; }

        public List<Region> regionlist { get; set; }

        public List<Role> rolesList { get; set; }

    }

    public class AdminInfo
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string contact { get; set; }

        public int[] selectedregion { get; set; }

    }

    public class BillingInfo
    {
        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string zip { get; set; }

        public string billingContact { get; set; }
    }

    public class CreateAdminModel
    {
        [Required(ErrorMessage = "*User Name is required")]
        public string userName { get; set; }

        [RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$!%#*?&])[A-Za-z\d$!#%*?&]{8,}", ErrorMessage = "*Password should contain atleast one number , one alphabet , one special character and having length minimum 8.")]
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

        [Compare("email", ErrorMessage = "*Both email must be same.")]
        public string confirmEmail { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Contact number is required")]
        public string contactNumber { get; set; }

        public List<Region> regionList { get; set; }

        [Required(ErrorMessage = "*State is required")]
        public int adminRegion {  get; set; }

        [Required(ErrorMessage = "*Address is required")]
        public string address1 { get; set; }

        public string? address2 { get; set; }

        [Required(ErrorMessage = "*City is required")]
        public string city { get; set; }


        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "*Enter a valid 6-digit Zip Code")]
        [Required(ErrorMessage = "*Zip code is required")]
        public string zipcode { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "*Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "*Billing contact number is required")]
        public string billingContact { get; set; }

        public List<Role> rolesList { get; set; }

        public int role { get; set; }
    }
}
