using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class ProviderViewModel
    {
        public List<Physician> physician { get; set; }

        public  List<Region> regions { get; set; }

        public List<int> physiciannotificationid { get; set;}

        public List<Region> regionlist { get; set; }
    }

    public class EditProviderViewModel
    {
        public int providerId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string contactNumber { get; set; }

        public string medicalLecense { get; set; }

        public string NPINumber { get; set; }

        public string syncEmail { get; set; }

        public List<Region> regionList { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zipcode { get; set; }

        public string billingContact {  get; set; }

        public string businessName { get; set; }

        public string businessSite { get; set; }

        public List<Physicianregion> physicianRegionlist { get; set; }
    }
}
