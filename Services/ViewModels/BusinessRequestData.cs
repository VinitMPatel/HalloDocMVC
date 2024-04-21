using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class BusinessRequestData
    {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First name")]
        [Required(ErrorMessage = "*First Name is required")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public string LastName { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string Mnumber { get; set; }

        [Required(ErrorMessage = "*Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Business is required")]
        public string Business { get; set; }

        public string? CaseNumber {  get; set; }

        public string? Symptoms { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid name")]
        [Required(ErrorMessage = "*Patient First Name is required")]
        public string PatientFirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid last name")]
        [Required(ErrorMessage = "*Patient Last Name is required")]
        public string PatientLastName { get; set; }

        [Required(ErrorMessage = "*Date of birth is required")]
        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "*Patient Email is required")]
        public string PatientEmail { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "*Patient mobile number is required")]
        public string PatientMobileNumber { get; set; }

        [Required(ErrorMessage = "*Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "*City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "*State is required")]
        public string State { get; set; }

        [StringLength(6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Zip Code is required")]
        public string ZipCode { get; set; }

        public string? Room { get; set; }
    }
}