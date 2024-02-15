using Common.Enum;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class Validation : IValidation
    {
        private readonly HelloDocDbContext _context;

        public Validation(HelloDocDbContext context)
        {
            _context = context;
        }

        public PatientLogin Validate(Aspnetuser user)
        {
            var x = _context.Aspnetusers.Where(u => u.Email == user.Email).FirstOrDefault();
            if (x.Email == null)
            {
                return new PatientLogin { Status=ResponseStautsEnum.Failed, Message = "Enter valid Email" };
            }
            if (x.Passwordhash == null)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, Message = "Enter valid password" };
            }
            if (x.Passwordhash != x.Passwordhash)
            {
                return new PatientLogin {Status = ResponseStautsEnum.Failed, Message = "Password is incorrect" };
            }
            return new PatientLogin {Status = ResponseStautsEnum.Success, Message = "Login Success" };
        }
    }
}
