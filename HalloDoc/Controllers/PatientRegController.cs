using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HalloDoc.DataContext;
using HalloDoc.ViewModels;
using HalloDoc.DataModels;

namespace HalloDoc.Controllers
{
    public class PatientRegController : Controller
    {
        private readonly HelloDocDbContext _context;
        
        public PatientRegController(HelloDocDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Insert(PatientInfo r)
        {
            var Aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == r.Email);

            if (Aspnetuser == null)
            {

                Aspnetuser aspnetuser = new Aspnetuser();
                aspnetuser.Id = Guid.NewGuid().ToString();
                aspnetuser.Passwordhash = r.Password;
                aspnetuser.Email = r.Email;
                String username = r.FirstName + r.LastName;
                aspnetuser.Username = username;
                aspnetuser.Phonenumber = r.PhoneNumber;
                aspnetuser.Modifieddate = DateTime.Now;
                _context.Aspnetusers.Add(aspnetuser);
                Aspnetuser = aspnetuser;
            }

            User user = new User();
            user.Aspnetuserid = Aspnetuser.Id;
            user.Firstname = r.FirstName;
            user.Lastname = r.LastName;
            user.Email = r.Email;
            user.Mobile = r.PhoneNumber;
            user.Street = r.Street;
            user.City = r.City;
            user.State = r.State;
            user.Zip = r.ZipCode;
            user.Createdby = r.FirstName + r.LastName;
            user.Modifieddate = DateTime.Now;
            user.Status = 1;
            user.Regionid = 1;

            _context.Users.Add(user);
            _context.SaveChanges();

            Request request = new Request
            {
                Requesttypeid = 1,
                Firstname = r.FirstName,
                Lastname = r.LastName,
                Phonenumber = r.PhoneNumber,
                Email = r.Email,
                Status = 1,
                Createddate = DateTime.Now,
                Modifieddate = DateTime.Now,
                Userid = user.Userid
            };

            _context.Requests.Add(request);
            _context.SaveChanges();
            var requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == user.Email);
            Requestclient requestclient = new Requestclient
            {
                Requestid = requestdata.Requestid,
                Firstname = r.FirstName,
                Lastname = r.LastName,
                Phonenumber = r.PhoneNumber,
                Notes = r.Symptoms,
                Email = r.Email,
                Street = r.Street,
                City = r.City,
                State = r.State,
                Zipcode = r.ZipCode,
                Regionid = 1
            };

            _context.Requestclients.Add(requestclient);
            _context.SaveChanges();

            return RedirectToAction("patient_first", "Home");

        }
    }
}
