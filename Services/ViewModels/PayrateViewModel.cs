using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class PayrateViewModel
    {
        public int physicianId { get; set; }

        public Payrate payrate { get; set; }
    }
}
