using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class LoggedInPersonViewModel
    {
        public int userid { get; set; }

        public string aspuserid { get; set; }

        public string username { get; set; }

        public string role { get; set; }
    }

    public class LoginPerson
    {
        [Required(ErrorMessage = "*Enter Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "*Enter Password")]
        public string password { get; set; }
    }
}
