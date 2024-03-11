using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using Services.ViewModels;
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

        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public PatientRequest(HalloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }
       
        public void Insert(PatientInfo r)
        {
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var aspnetuser = _context.Aspnetusers.Where(m => m.Email == r.Email).FirstOrDefault();
                    User user = new User();
                    if (aspnetuser == null)
                    {

                        Aspnetuser aspnetuser1 = new Aspnetuser();
                        aspnetuser1.Id = Guid.NewGuid().ToString();
                        aspnetuser1.Passwordhash = r.Password;
                        aspnetuser1.Email = r.Email;
                        String username = r.FirstName + r.LastName;
                        aspnetuser1.Username = username;
                        aspnetuser1.Phonenumber = r.PhoneNumber;
                        aspnetuser1.Createddate = DateTime.Now;
                        aspnetuser1.Modifieddate = DateTime.Now;
                        _context.Aspnetusers.Add(aspnetuser1);
                        aspnetuser1 = aspnetuser1;
                        _context.Aspnetusers.Add(aspnetuser1);

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
                        user.Intyear = int.Parse(r.DOB.ToString("yyyy"));
                        user.Intdate = int.Parse(r.DOB.ToString("dd"));
                        user.Strmonth = r.DOB.ToString("MMM");
                        user.Createddate = DateTime.Now;
                        user.Modifieddate = DateTime.Now;
                        user.Status = 1;
                        user.Regionid = 1;
                        _context.Users.Add(user);
                    }
                    else
                    {
                         user = _context.Users.Where(m => m.Email == r.Email).FirstOrDefault();
                    }
                    
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
                        User = user,
                    };
                    _context.Requests.Add(request);

                    Requestclient requestclient = new Requestclient
                    {
                        Request = request,
                        Firstname = r.FirstName,
                        Lastname = r.LastName,
                        Phonenumber = r.PhoneNumber,
                        Notes = r.Symptoms,
                        Email = r.Email,
                        Street = r.Street,
                        City = r.City,
                        State = r.State,
                        Zipcode = r.ZipCode,
                        Address = r.Street + ", " + r.City + ", " + r.State,
                        Regionid = 1,
                        Intyear = int.Parse(r.DOB.ToString("yyyy")),
                        Intdate = int.Parse(r.DOB.ToString("dd")),
                        Strmonth = r.DOB.ToString("MMM")
                    };
                    _context.Requestclients.Add(requestclient);

                    _context.SaveChanges();

                    if (r.Upload != null)
                    {
                        uploadFile(r.Upload, request.Requestid);
                    }

                    transaction.Commit();

                }
                catch (Exception ex) {
                    transaction.Rollback();
                }
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
                    Filename = item.FileName,
                    Createddate = DateTime.Now,

                };
                _context.Add(requestwisefile);
                _context.SaveChanges();
            }
        }

        public void NewAccount(Aspnetuser model)
        {
            Aspnetuser aspnetuser1 = new Aspnetuser();
            aspnetuser1.Id = Guid.NewGuid().ToString();
            aspnetuser1.Passwordhash = model.Passwordhash;
            aspnetuser1.Email = model.Email;
            aspnetuser1.Username = "Temp";

            _context.Aspnetusers.Add(aspnetuser1);
            _context.SaveChanges();

        }
    }
}
