using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Services.ViewModels {
    public class PatientInfo
    {
        public string? Symptoms { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*Please Enter first name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Please Enter last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*Please enter Date of birth")]
        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "*Please enter your Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Please enter password")]
        //(?=.*\d.*)(?=.*[a-zA-Z].*)(?=.*[!#\$%&\?].*).{8,}
        [RegularExpression(@"(?=.*\d.*)(?=.*[a-zA-Z].*)(?=.*[!#\$%&\?@].*).{8,20}", ErrorMessage = "Password should contain atleast one number , one upper and one lower alphabet , one special character and having length minimum 8.")]
        public string Password { get; set; }

        [Compare("Password" , ErrorMessage = "*Both password must be same.")]
        public string ConfirmPassword { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "*Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "*Please enter your Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "*Please enter street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "*Please enter city")]
        public string City { get; set; }

        [Required(ErrorMessage = "*Please enter state")]
        public string State { get; set; }

        [StringLength(6, ErrorMessage = "*Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "*Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Please enter zip code")]
        public string ZipCode { get; set; }

        public string? Room { get; set; }

        public List<IFormFile>? Upload { get; set; }

    }
}

