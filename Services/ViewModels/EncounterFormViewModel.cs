using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class EncounterFormViewModel
    {
        public int RequestId { get; set; }
        public string role { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        public string Firstname { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        public string Lastname { get; set; }

        public string Location { get; set; }

        public DateTime DOB { get; set; }

        public DateTime? Dateofservice { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string Mobile { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public  string Email { get; set; }

        
        public  string HistoryOfIllness { get; set; }

       
        public  string? MedicalHistory { get; set; }

        
        public  string? Medication { get; set; }

        
        public  string? Allergies { get; set; }

        [Required]
        public decimal? Temp { get; set; }

        
        public decimal? HR { get; set; }

      
        public decimal? RR { get; set; }

        [Required]
        public int? BPs { get; set; }

        [Required]
        public int? BPd { get; set; }

        [Required]
        public decimal? O2 { get; set; }

        public string? Pain { get; set; }

        public string? Heent { get; set; }

        public string? CV { get; set; }

        public string? Chest { get; set; }

        public string? ABD { get; set; }

        public string? Extr { get; set; }

        public string? Skin { get; set; }

        public string? Neuro { get; set; }

        public string? Other { get; set; }

        public string? Diagnosis { get; set; }

        public string? TreatmentPlan { get; set; }

        public string? MedicationsDispended { get; set; }

        public string? Procedure { get; set; }

        public string Followup { get; set; }

        public bool isFinaled { get; set; }
    }
}

