using Data.DataContext;
using Data.Entity;
using HalloDoc.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class PatientRequest : IPatientRequest
    {

        private readonly HelloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public PatientRequest(HelloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }
       
        public void Insert(PatientInfo r)
        {
            var aspnetuser =  _context.Aspnetusers.Where(m => m.Email == r.Email).FirstOrDefault();

            if (aspnetuser == null)
            {

                Aspnetuser aspnetuser1 = new Aspnetuser();
                aspnetuser1.Id = Guid.NewGuid().ToString();
                aspnetuser1.Passwordhash = r.Password;
                aspnetuser1.Email = r.Email;
                String username = r.FirstName + r.LastName;
                aspnetuser1.Username = username;
                aspnetuser1.Phonenumber = r.PhoneNumber;
                aspnetuser1.Modifieddate = DateTime.Now;
                _context.Aspnetusers.Add(aspnetuser1);
                aspnetuser1 = aspnetuser1;

                User user = new User();
                user.Aspnetuserid = aspnetuser1.Id;
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
            }

            var user1 =  _context.Users.Where(m => m.Email == r.Email).FirstOrDefault();

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
                Userid = user1.Userid,

            };

            _context.Requests.Add(request);
            _context.SaveChanges();
            var requestdata = _context.Requests.Where(m => m.Email == user1.Email).FirstOrDefault();
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


            if (r.Upload != null)
            {
                uploadFile(r.Upload, requestdata.Requestid);
            }

           

        }

        public void uploadFile(List<IFormFile> upload, int id)
        {
            foreach (var item in upload)
            {
                string path = _env.WebRootPath + "/upload/" + item.FileName;
                FileStream stream = new FileStream(path, FileMode.Create);

                item.CopyTo(stream);
                Requestwisefile requestwisefile = new Requestwisefile
                {
                    Requestid = id,
                    Filename = path,
                    Createddate = DateTime.Now,

                };
                _context.Add(requestwisefile);
                _context.SaveChanges();
            }
        }

        //[Route("/PatientReg/patient_request/checkmail/{email}")]
        //[HttpGet]
        //public IActionResult CheckEmail(string email)
        //{
        //    var emailExists = _context.Aspnetusers.Any(u => u.Email == email);
        //    return Json(new { exists = emailExists });
        //}
    }
}
