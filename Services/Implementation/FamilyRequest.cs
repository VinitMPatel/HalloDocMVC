using Data.DataContext;
using Data.Entity;
using HalloDoc.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.Implementation
{
    public class FamilyRequest : IFamilyRequest
    {
        private readonly HelloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public FamilyRequest(HelloDocDbContext context , IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public bool FamilyInsert(FamilyFriendRequest r)
        {
            var aspnetuser = _context.Aspnetusers.Where(m => m.Email == r.PatientEmail).FirstOrDefault();

            Data.Entity.Request request = new Data.Entity.Request();
            if (aspnetuser == null) { 
                request.Requesttypeid = 2;
                //Userid = user.Userid,
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
                    Street = r.Street,
                    Address = r.Street + ", " + r.City + ", " + r.State,
                    Regionid = 1,
                    Notes = r.Symptoms,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.ZipCode,
                };
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();
                
                if (r.Upload != null)
                {
                    uploadFile(r.Upload, request.Requestid);
                }
                return false;
            }
            else
            {
                var request2 = _context.Requests.FirstOrDefault(m => m.Email == r.PatientEmail);
                String username = r.PatientFirstName + r.PatientLastName;
                aspnetuser.Username = username;
                aspnetuser.Phonenumber = r.Mnumber;
                aspnetuser.Createddate = DateTime.Now;
                aspnetuser.Modifieddate = DateTime.Now;

                User user = new User();
                user.Aspnetuserid = aspnetuser.Id;
                user.Firstname = r.PatientFirstName;
                user.Lastname = r.PatientLastName;
                user.Email = r.PatientEmail;
                user.Mobile = r.PatientMobileNumber;
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
                _context.SaveChanges();
                _context.Aspnetusers.Update(aspnetuser);

                request2.Userid = user.Userid;
                request2.Phonenumber = r.PatientMobileNumber;
                _context.Requests.Update(request2);

                _context.SaveChanges();
                return true;
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