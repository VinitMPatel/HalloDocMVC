using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.Implementation
{
    public class FamilyRequest : IFamilyRequest
    {
        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public FamilyRequest(HalloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public bool FamilyInsert(FamilyFriendRequest r)
        {
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == r.PatientEmail);
            if (aspnetuser != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == r.PatientEmail);
                if (user == null)
                {
                    String username = r.PatientFirstName + r.PatientLastName;
                    aspnetuser.Username = username;
                    aspnetuser.Phonenumber = r.PatientMobileNumber;
                    aspnetuser.Createddate = DateTime.Now;
                    aspnetuser.Modifieddate = DateTime.Now;
                    _context.Aspnetusers.Update(aspnetuser);

                    User newUser = new User
                    {
                        Aspnetuserid = aspnetuser.Id,
                        Firstname = r.PatientFirstName,
                        Lastname = r.PatientLastName,
                        Email = r.PatientEmail,
                        Mobile = r.PatientMobileNumber,
                        Street = r.Street,
                        City = r.City,
                        State = r.State,
                        Zip = r.ZipCode,
                        Createdby = r.FirstName + r.LastName,
                        Intyear = int.Parse(r.DOB.ToString("yyyy")),
                        Intdate = int.Parse(r.DOB.ToString("dd")),
                        Strmonth = r.DOB.ToString("MMM"),
                        Createddate = DateTime.Now,
                        Modifieddate = DateTime.Now,
                        Status = 1,
                        Regionid = 1
                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    Data.Entity.Request request = new Data.Entity.Request();
                    request.Requesttypeid = 2;
                    request.Userid = newUser.Userid;
                    request.Firstname = r.PatientFirstName;
                    request.Lastname = r.PatientLastName;
                    request.Phonenumber = r.PatientMobileNumber;
                    request.Email = r.PatientEmail;
                    request.Status = 1;
                    request.Createddate = DateTime.Now;
                    request.Modifieddate = DateTime.Now;
                    request.Relationname = r.Relation;

                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    Requestclient requestclient = new Requestclient
                    {
                        Requestid = request.Requestid,
                        Firstname = r.FirstName,
                        Lastname = r.LastName,
                        Phonenumber = r.Mnumber,
                        Email = r.Email,
                        Street = r.Street,
                        Address = r.Street + ", " + r.City + ", " + r.State,
                        Regionid = 1,
                        Notes = r.Symptoms,
                        City = r.City,
                        State = r.State,
                        Zipcode = r.ZipCode,
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

                    return true;
                }

                Data.Entity.Request request2 = new Data.Entity.Request();
                request2.Requesttypeid = 2;
                request2.Userid = user.Userid;
                request2.Firstname = r.PatientFirstName;
                request2.Lastname = r.PatientLastName;
                request2.Phonenumber = r.PatientMobileNumber;
                request2.Email = r.PatientEmail;
                request2.Status = 1;
                request2.Createddate = DateTime.Now;
                request2.Modifieddate = DateTime.Now;
                request2.Relationname = r.Relation;

                _context.Requests.Add(request2);
                _context.SaveChanges();

                Requestclient requestclient2 = new Requestclient
                {
                    Requestid = request2.Requestid,
                    Firstname = r.FirstName,
                    Lastname = r.LastName,
                    Phonenumber = r.Mnumber,
                    Email = r.Email,
                    Street = r.Street,
                    Address = r.Street + ", " + r.City + ", " + r.State,
                    Regionid = 1,
                    Notes = r.Symptoms,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.ZipCode,
                    Intyear = int.Parse(r.DOB.ToString("yyyy")),
                    Intdate = int.Parse(r.DOB.ToString("dd")),
                    Strmonth = r.DOB.ToString("MMM")
                };
                _context.Requestclients.Add(requestclient2);
                _context.SaveChanges();

                if (r.Upload != null)
                {
                    uploadFile(r.Upload, request2.Requestid);
                }

                return true;
            }

            else
            {
                return false;
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
    }
}