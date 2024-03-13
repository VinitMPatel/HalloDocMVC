using Data.Entity;
using Humanizer.Localisation.TimeToClockNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class CaseActions
    {
        public List<Region> regionList { get; set; }

        public List<Physician> physiciansList { get; set; }

        public List<Casetag> cancelList { get; set; }

        public String region {  get; set; }

        public int physicianId { get; set; }

        public String assignNotes { get; set; }

        public String cancelReason { get; set; }

        public String patietName { get; set;}

        public int requestId { get; set; }
    }

    public class AgreementDetails { 

        public string mobile { get; set; }

        public string email { get; set; }

        public int type { get; set; }

        public int requestId {  get; set; }
    }

}
