using Microsoft.AspNetCore.Http;

namespace Services.ViewModels
{
    public class FamilyFriendRequest
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Mnumber { get; set; }

        public String Email { get; set; }

        public String Relation { get; set; }

        public String Symptoms { get; set; }

        public String PatientFirstName { get; set; }

        public String PatientLastName { get; set; }

        public DateOnly DOB { get; set; }

        public String PatientEmail { get; set; }

        public String PatientMobileNumber { get; set; }

        public String Street { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String ZipCode { get; set; }

        public String Room { get; set; }

        public List<IFormFile> Upload { get; set; }
    }
}
