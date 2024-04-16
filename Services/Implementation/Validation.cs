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

        public PatientLogin Validate(LoginPerson user)
        {
            Aspnetuser? x = _context.Aspnetusers.Where(u => u.Email == user.email).FirstOrDefault();
            string decryptPassword = EncryptDecryptHelper.Decrypt(x.Passwordhash);
            if (user.email == null && user.password == null)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, emailError = "*Enter Email", passwordError = "*Enter password" };
            }
            else if (user.password == null)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, passwordError = "*Enter Password" };
            }
            if (x == null)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, emailError = "*Email not found" };
            }
            else if (user.password != decryptPassword)
            {
                return new PatientLogin { Status = ResponseStautsEnum.Failed, passwordError = "*Enter correct password" };
            }
            else
            {
                return new PatientLogin { Status = ResponseStautsEnum.Success };
            }
        }

        public (PatientLogin, LoggedInPersonViewModel) AdminValidate(LoginPerson user)
        {
            LoggedInPersonViewModel loggedInPerson = new LoggedInPersonViewModel();

            if (user.password == null && user.email == null)
            {
                return (new PatientLogin { Status = ResponseStautsEnum.Failed, emailError = "*Enter Email", passwordError = "*Enter Password" }, loggedInPerson);
            }

            Aspnetuser? aspNetUser = _context.Aspnetusers.FirstOrDefault(u => u.Email == user.email);
            if (aspNetUser == null)
            {
                return (new PatientLogin { Status = ResponseStautsEnum.Failed, emailError = "*Email not found" }, loggedInPerson);
            }

            string decryptPassword = EncryptDecryptHelper.Decrypt(aspNetUser.Passwordhash);

            Aspnetuserrole? userRole = _context.Aspnetuserroles.FirstOrDefault(u => u.Userid == aspNetUser!.Id);

            loggedInPerson.role = userRole!.Roleid;
            loggedInPerson.aspuserid = aspNetUser!.Id;
            loggedInPerson.username = aspNetUser.Username;
            
            if (user.password != decryptPassword)
            {
                return (new PatientLogin { Status = ResponseStautsEnum.Failed, passwordError = "*Enter correct password" }, loggedInPerson);
            }
            else
            {
                return (new PatientLogin { Status = ResponseStautsEnum.Success }, loggedInPerson);
            }
        }
    }
}
