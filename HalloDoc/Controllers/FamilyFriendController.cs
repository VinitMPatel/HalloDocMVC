using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.DataContext;
using Data.Entity;
using HalloDoc.ViewModels;


namespace HalloDoc.Controllers
{
    public class FamilyFriendController : Controller
    {

        private readonly HelloDocDbContext _context;

        public FamilyFriendController(HelloDocDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Insert(FamilyFriendRequest r)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == r.PEmail);
            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == r.PEmail);

            if (user != null)
            {
                Request request = new Request
                {
                    Requesttypeid = 2,
                    Userid = user.Userid,
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
                    Address = r.Street+", "+r.City+ ", "+r.State,
                    Regionid = 1,
                    Notes = r.Symptoms,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.ZipCode,
                };
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();

                return RedirectToAction("index", "Home");
            }
            return RedirectToAction("index", "Home");
        }
    }
}
