using Data.DataContext;
using Data.Entity;
using HalloDoc.ViewModels;
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

        public FamilyRequest(HelloDocDbContext context)
        {
            _context = context;
        }

        public void FamilyInsert(FamilyFriendRequest r)
        {
            var user = _context.Users.Where(m => m.Email == r.PatientEmail).FirstOrDefault();
            var aspnetuser = _context.Aspnetusers.Where(m => m.Email == r.PatientEmail).FirstOrDefault();

            if (user != null)
            {
                Request request = new Request
                {
                    Requesttypeid = 2,
                    Userid = user.Userid,
                    Firstname = r.PatientFirstName,
                    Lastname = r.PatientLastName,
                    Phonenumber = r.PatientMobileNumber,
                    Email = r.PatientEmail,
                    Status = 1,
                    Createddate = DateTime.Now,
                    Modifieddate = DateTime.Now,
                    Relationname = r.Relation
                };
                _context.Requests.Add(request);
                _context.SaveChanges();

                Requestclient requestclient = new Requestclient
                {
                    Requestid = request.Requestid,
                    Firstname = r.FirstName,
                    Lastname = r.LastName,
                    Phonenumber = r.Mnumber,
                    Address = r.Street + ", " + r.City + ", " + r.State,
                    Regionid = 1,
                    Notes = r.Symptoms,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.ZipCode,
                };
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();

            }

        }
    }
}
