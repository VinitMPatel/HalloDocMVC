using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Services.ViewModels {
    public class PatientInfo
    {
        public String? Symptoms { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*First Name is required")]
        public String FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "*Date Of Birth is required")]
        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        public String Email { get; set; }

        public String Password { get; set; }

        [Compare("Password")]
        public String ConfirmPassword { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Please enter your Phone Number")]
        public String PhoneNumber { get; set; }

        [Required]
        public String Street { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        [StringLength(6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Zip Code is required")]
        public String ZipCode { get; set; }

        public String? Room { get; set; }

        public List<IFormFile> Upload { get; set; }

    }
}

