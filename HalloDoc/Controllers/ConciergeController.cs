using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HalloDoc.DataContext;
using HalloDoc.ViewModels;
using HalloDoc.DataModels;

namespace HalloDoc.Controllers
{
    public class ConciergeController : Controller
    {
        private readonly HelloDocDbContext _context;

        public ConciergeController(HelloDocDbContext context)
        {
            _context = context;
        }

        [HttpPost]

        public async Task<IActionResult> Insert(ConciergeRequest r)
        {
            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == r.PEmail);
            var user = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == r.PEmail);

            if (user != null) {
                Concierge concierge = new Concierge { 
                    Conciergename = r.FirstName+" "+r.LastName,
                    Address = r.Street+", "+r.City,
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
                    Firstname = r.PFirstName,
                    Lastname = r.PLastName,
                    Phonenumber = r.PMnumber,
                    Email = r.PEmail,
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

                return RedirectToAction("index", "Home");
            }
            return RedirectToAction("index", "Home");
        }
    }
}