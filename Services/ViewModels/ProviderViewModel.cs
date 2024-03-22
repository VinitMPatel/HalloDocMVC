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
}
