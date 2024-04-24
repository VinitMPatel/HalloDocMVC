using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class StudentForm
    {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid First Name")]
        [Required(ErrorMessage = "*Please Enter first name")]
        public string firstName {  get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Last Name")]
        public string lastName { get; set; }


        [Required(ErrorMessage = "*Please Enter email")]
        public string email { get; set; }


        public string? gender { get; set; }

        [Required(ErrorMessage = "*Please Enter course")]
        public string course { get; set; }

        public string? grade { get; set; }

        public int flag { get; set; }

        public int studentId {  get; set; }

        public string age { get; set; }
    }
}
