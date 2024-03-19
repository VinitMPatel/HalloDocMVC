using Data.DataContext;
using Data.Entity;
using Services.ViewModels;
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
            // var aspnetuser =  _context.Aspnetusers.Where(m => m.Email == r.PatientEmail).FirstOrDefault();
            var user = _context.Users.Where(m => m.Email == r.PatientEmail).FirstOrDefault();

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

                string region = _context.Regions.FirstOrDefault(a => a.Regionid == user.Regionid).Abbreviation;
                var requestcount = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date && a.Createddate.Month == DateTime.Now.Month && a.Createddate.Year == DateTime.Now.Year && a.Userid == user.Userid).ToList();
                Request request = new Request
                {
                    Userid = user.Userid,
                    Requesttypeid = 3,
                    Firstname = r.FirstName,
                    Lastname = r.LastName,
                    Phonenumber = r.Mnumber,
                    Email = r.Email,
                    Status = 1,
                    Createddate = DateTime.Now,
                    Modifieddate = DateTime.Now,
                    Relationname = r.Relation,
                    Confirmationnumber = region.Substring(0, 2) + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                                            DateTime.Now.Year.ToString().Substring(2) + r.PatientLastName.ToUpper().Substring(0, 2) + r.PatientFirstName.ToUpper().Substring(0, 2) +
                                            (requestcount.Count() + 1).ToString().PadLeft(4, '0')
                };
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
