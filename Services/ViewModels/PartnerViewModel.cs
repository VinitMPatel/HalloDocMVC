using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class PartnerViewModel
    {
        public List<Healthprofessional> professionList {  get; set; }

        public List<Healthprofessionaltype> professionTypeList { get; set; }
    }
}
