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
    public class BusinessRequest : IBusinessRequest
    {
        private readonly HelloDocDbContext _context;

        public BusinessRequest(HelloDocDbContext context)
        {
            _context = context;
        }

        public void BusinessInsert(BusinessRequestData r)
        {

            var aspnetuser = _context.Aspnetusers.Where(m => m.Email == r.PEmail).FirstOrDefault();
            var user = _context.Aspnetusers.Where(m => m.Email == r.PEmail).FirstOrDefault();

            if (user != null)
            {
                Business business = new Business
                {
                    Name = r.Business,
                    Address1 = r.Street + ", " + r.City,
                    City = r.City,
                    Regionid = 1,
                    Zipcode = r.ZipCode,
                    Phonenumber = r.PMnumber,
                    Createddate = DateTime.Now,
                    Status = 1,
                    Createdby = aspnetuser.Id,
                    Modifiedby = aspnetuser.Id

                };
                _context.Businesses.Add(business);
                _context.SaveChanges();

                Request request = new Request
                {
                    Requesttypeid = 4,
                    Firstname = r.PFirstName,
                    Lastname = r.PLastName,
                    Phonenumber = r.PMnumber,
                    Email = r.PEmail,
                    Status = 1,
                    Createddate = DateTime.Now,
                    Modifieddate = DateTime.Now,
                    Relationname = r.Business
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

                Requestbusiness requestbusiness = new Requestbusiness
                {
                    Requestid = request.Requestid,
                    Businessid = business.Businessid
                };
                _context.Requestbusinesses.Add(requestbusiness);
                _context.SaveChanges();
            }
        }
    }
}
