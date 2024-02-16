using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.ViewModels
{
    public class PatientInfo
    {
        public String Symptoms { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public String FirstName { get; set; }


        public String LastName { get; set; }

        public  DateOnly DOB { get; set; }


        public String Email { get; set; }

        public String Password { get; set; }

        public String PhoneNumber { get; set; }

        public String Street { get; set; }

        
        public String City { get; set; }


        public String State { get; set; }

        public String ZipCode { get; set; }

        public String Room { get; set; }

        public List<IFormFile> Upload { get; set; }

    }
}
