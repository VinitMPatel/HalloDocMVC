using Common.Enum;
using Common.Helper;
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
        private readonly HalloDocDbContext _context;

        public Validation(HalloDocDbContext context)
        {
            _context = context;
        }

        public PatientLogin Validate(Aspnetuser user)
        {
            Aspnetuser? x = _context.Aspnetusers.Where(u => u.Email == user.Email).FirstOrDefault();
            string decryptPassword = EncryptDecryptHelper.Decrypt(x.Passwordhash);
                if (user.Email == null && user.Passwordhash == null)
                {
                    return new PatientLogin { Status = ResponseStautsEnum.Failed, emailError = "*Enter Email", passwordError = "*Enter password" };
                }
                else if (user.Passwordhash == null)
                {
                    return new PatientLogin { Status = ResponseStautsEnum.Failed, passwordError = "*Enter Password" };
                }
                if (x == null)
                {
                    return new PatientLogin { Status = ResponseStautsEnum.Failed, emailError = "*Email not found" };
                }
                else if (user.Passwordhash != decryptPassword)
                {
                    return new PatientLogin { Status = ResponseStautsEnum.Failed, passwordError = "*Enter correct password" };
                }
                else 
                { 
                    return new PatientLogin { Status = ResponseStautsEnum.Success }; 
                }
        }

        public PatientLogin AdminValidate(Aspnetuser user)
        {
            var x = _context.Aspnetusers.Where(u => u.Email == user.Email).FirstOrDefault();
            string decryptPassword = EncryptDecryptHelper.Decrypt(x.Passwordhash);
            if (user.Email == null && user.Passwordhash == null)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, emailError = "*Enter Email", passwordError = "*Enter password" };
            }
            else if (user.Passwordhash == null)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, passwordError = "*Enter Password" };
            }
            if (x == null)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, emailError = "*Email not found" };
            }
            else if (user.Passwordhash != decryptPassword)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, passwordError = "*Enter correct password" };
            }
            else
            {
                return new PatientLogin { Status = ResponseStautsEnum.Success };
            }
        }
    }
}
