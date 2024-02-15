using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class PatientLogin
    {
        public ResponseStautsEnum Status { get; set; }

        public String Message { get; set; }

        public String Token {  get; set; }

    }
}
