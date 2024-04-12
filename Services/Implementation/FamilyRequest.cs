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
                string region = _context.Regions.FirstOrDefault(a => a.Regionid == user.Regionid).Abbreviation;
                var requestcount = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date && a.Createddate.Month == DateTime.Now.Month && a.Createddate.Year == DateTime.Now.Year && a.Userid == user.Userid).ToList();
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

                    string newRegion = _context.Regions.FirstOrDefault(a => a.Regionid == newUser.Regionid).Abbreviation;
                    var newRequestCount = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date && a.Createddate.Month == DateTime.Now.Month && a.Createddate.Year == DateTime.Now.Year && a.Userid == newUser.Userid).ToList();

                    Data.Entity.Request request = new Data.Entity.Request();
                    request.Requesttypeid = 2;
                    request.Userid = newUser.Userid;
                    request.Firstname = r.FirstName;
                    request.Lastname = r.LastName;
                    request.Phonenumber = r.Mnumber;
                    request.Email = r.Email;
                    request.Status = 1;
                    request.Createddate = DateTime.Now;
                    request.Modifieddate = DateTime.Now;
                    request.Relationname = r.Relation;
                    request.Confirmationnumber = newRegion.Substring(0, 2) + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                                            DateTime.Now.Year.ToString().Substring(2) + r.PatientLastName.ToUpper().Substring(0, 2) + r.PatientFirstName.ToUpper().Substring(0, 2) +
                                            (newRequestCount.Count() + 1).ToString().PadLeft(4, '0');
                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    Requestclient requestclient = new Requestclient
                    {
                        Requestid = request.Requestid,
                        Firstname = r.PatientFirstName,
                        Lastname = r.PatientLastName,
                        Phonenumber = r.PatientMobileNumber,
                        Email = r.PatientEmail,
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
                request2.Firstname = r.FirstName;
                request2.Lastname = r.LastName;
                request2.Phonenumber = r.Mnumber;
                request2.Email = r.Email;
                request2.Status = 1;
                request2.Createddate = DateTime.Now;
                request2.Modifieddate = DateTime.Now;
                request2.Relationname = r.Relation;
                request2.Confirmationnumber = region.Substring(0, 2) + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                                            DateTime.Now.Year.ToString().Substring(2) + r.PatientLastName.ToUpper().Substring(0, 2) + r.PatientFirstName.ToUpper().Substring(0, 2) +
                                            (requestcount.Count() + 1).ToString().PadLeft(4, '0');

                _context.Requests.Add(request2);
                _context.SaveChanges();

                Requestclient requestclient2 = new Requestclient
                {
                    Requestid = request2.Requestid,
                    Firstname = r.PatientFirstName,
                    Lastname = r.PatientLastName,
                    Phonenumber = r.PatientMobileNumber,
                    Email = r.PatientEmail,
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