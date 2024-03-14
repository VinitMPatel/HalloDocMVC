using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class BusinessRequestData
    {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First name")]
        [Required(ErrorMessage = "*First Name is required")]
        public String FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        [Required(ErrorMessage = "*Last Name is required")]
        public String LastName { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public String Mnumber { get; set; }

        public String Email { get; set; }

        public String Business { get; set; }

        public String CaseNumber {  get; set; }

        public String Symptoms { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid name")]
        [Required(ErrorMessage = "*Patient First Name is required")]
        public String PatientFirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid last name")]
        [Required(ErrorMessage = "*Patient Last Name is required")]
        public String PatientLastName { get; set; }

        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "*Patient Email is required")]
        public String PatientEmail { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "*Patient mobile number is required")]
        public String PatientMobileNumber { get; set; }

        public String Street { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        [StringLength(6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [Required(ErrorMessage = "*Zip Code is required")]
        public String ZipCode { get; set; }

        public String Room { get; set; }
    }
}