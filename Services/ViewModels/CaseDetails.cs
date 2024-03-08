using Data.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public  class CaseActionDetails
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

        public ICollection<Requestwisefile> requestwisefile { get; set; }

        public List<IFormFile> Upload { get; set; }
    }

    public class Orders
    {
        public int requestId { get; set; }

        public int reqid { get; set; }

        public int vendorid { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string prescription { get; set; }

        public int refil { get; set; }

        public int professionid { get; set; }

        public string createdby { get; set; }

    }
}
