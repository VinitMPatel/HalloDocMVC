using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HalloDoc.ViewModels;
using HalloDoc.DataModels;

namespace HalloDoc.Controllers
{
    public class BusinessController : Controller
    {
        private readonly HelloDocDbContext _context;

        public BusinessController(HelloDocDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Insert(BusinessRequest r)
        {

            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == r.PEmail);
            var user = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == r.PEmail);

            if (user != null)
            {
                Business business = new Business { 
                    Name = r.Business,
                    Address1 = r.Street + ", " +r.City,
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

                return RedirectToAction("index", "Home");
            }

            return RedirectToAction("index", "Home");
        }
    }
}
