﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Services.ViewModels {
    public class PatientInfo
    {
        public String Symptoms { get; set; }

        [Required]
        public String FirstName { get; set; }

        public String LastName { get; set; }

        [Required]
        public DateOnly DOB { get; set; }

        [Required]
        public String Email { get; set; }

        public String Password { get; set; }

        [Compare("Password")]
        public String ConfirmPassword { get; set; }

        [Required]
        public String PhoneNumber { get; set; }

        [Required]
        public String Street { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        [Required]
        public String ZipCode { get; set; }

        public String Room { get; set; }

        public List<IFormFile> Upload { get; set; }

    }
}

