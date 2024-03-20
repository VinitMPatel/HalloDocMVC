using Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class AdminProfile
    {
        public Admin admin { get; set; }

        public int[] selectedregion { get; set; }

        public List<Adminregion> adminregionlist { get; set; }


        public List<Region> regionlist { get; set; }

    }

    public class AdminInfo
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string contact { get; set; }

        public int[] selectedregion { get; set; }

    }

    public class BillingInfo
    {
        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string zip { get; set; }

        public string billingContact { get; set; }
    }
}
