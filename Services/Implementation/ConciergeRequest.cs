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
    public class ConciergeRequest : IConciergeRequest
    {
        private readonly HalloDocDbContext _context;

        public ConciergeRequest(HalloDocDbContext context)
        {
            _context = context;
        }


        public void CnciergeInsert(ConciergeRequestData r)
        {
            var aspnetuser =  _context.Aspnetusers.Where(m => m.Email == r.PatientEmail).FirstOrDefault();
            var user =  _context.Aspnetusers.Where(m => m.Email == r.PatientEmail).FirstOrDefault();

            if (user != null)
            {
                Concierge concierge = new Concierge
                {
                    Conciergename = r.FirstName + " " + r.LastName,
                    Address = r.Street + ", " + r.City,
                    Street = r.Street,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.ZipCode,
                    Createddate = DateTime.Now,
                    Regionid = 1,
                };
                _context.Concierges.Add(concierge);
                _context.SaveChanges();

                Request request = new Request
                {
                    Requesttypeid = 3,
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

                Requestconcierge requestconcierge = new Requestconcierge
                {
                    Requestid = request.Requestid,
                    Conciergeid = concierge.Conciergeid,
                };
                _context.Requestconcierges.Add(requestconcierge);
                _context.SaveChanges();
            }
        }
    }
}
