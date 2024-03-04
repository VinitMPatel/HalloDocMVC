using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public  class CaseDetails
    {
        public int requestId { get; set; }

        public String ConfirmationNumber { get; set; }

        public String PatientNotes { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public DateTime DOB { get; set; }

        public String PhoneNumber { get; set; }

        public String Email { get; set; }

        public String Region { get; set;}

        public String Address { get; set; }

        public String Room { get; set; }

        public List<Region> regionList { get; set; }

        public int requestType { get; set; }

        public String adminName { get; set; }

        public String physicianName { get; set; }

        public DateTime assignTime { get; set; }

        public String adminNote { get; set; }
    }
}
