using Data.DataContext;
using Data.Entity;
using Services.ViewModels;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Services.Implementation
{
    public class BusinessRequest : IBusinessRequest
    {
        private readonly HalloDocDbContext _context;

        public BusinessRequest(HalloDocDbContext context)
        {
            _context = context;
        }

        public async Task BusinessInsert(BusinessRequestData r)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(u => u.Email == r.PatientEmail);
                    User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == r.PatientEmail);
                    List<Request> requestcount = await _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date && a.Createddate.Month == DateTime.Now.Month && a.Createddate.Year == DateTime.Now.Year && a.Userid == user.Userid).ToListAsync();
                    Region? region = await _context.Regions.FirstOrDefaultAsync(a => a.Regionid == user.Regionid);

                    if (user.Isrequestwithemail[0] == true)
                    {
                        string username = r.PatientFirstName + r.PatientLastName;
                        aspnetuser.Username = username;
                        aspnetuser.Phonenumber = r.PatientMobileNumber;
                        aspnetuser.Modifieddate = DateTime.Now;
                        _context.Aspnetusers.Update(aspnetuser);

                        user.Firstname = r.PatientFirstName;
                        user.Lastname = r.PatientLastName;
                        user.Mobile = r.PatientMobileNumber;
                        user.Street = r.Street;
                        user.City = r.City;
                        user.State = r.State;
                        user.Zip = r.ZipCode;
                        user.Intyear = int.Parse(r.DOB.ToString("yyyy"));
                        user.Intdate = int.Parse(r.DOB.ToString("dd"));
                        user.Strmonth = r.DOB.ToString("MMM");
                        user.Modifieddate = DateTime.Now;
                        user.Status = 1;
                        user.Isrequestwithemail = new System.Collections.BitArray(new[] { false });
                        _context.Update(user);
                    }

                    Business business = new Business
                    {
                        Name = r.FirstName + " " + r.LastName,
                        Address1 = r.Street + ", " + r.City,
                        City = r.City,
                        Regionid = 1,
                        Phonenumber = r.Mnumber,
                        Createdby = aspnetuser.Id,
                        Createddate = DateTime.Now,
                        Isdeleted = new System.Collections.BitArray(new[] {false})
                    };
                    await _context.Businesses.AddAsync(business);
                    await _context.SaveChangesAsync();

                    Request request = new Request();
                    request.Requesttypeid = 3;
                    request.Userid = user.Userid;
                    request.Firstname = r.FirstName;
                    request.Lastname = r.LastName;
                    request.Phonenumber = r.Mnumber;
                    request.Email = r.Email;
                    request.Status = 1;
                    request.Createddate = DateTime.Now;
                    request.Modifieddate = DateTime.Now;
                    request.Confirmationnumber = region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                                            DateTime.Now.Year.ToString().Substring(2) + r.PatientLastName.ToUpper().Substring(0, 2) + r.PatientFirstName.ToUpper().Substring(0, 2) +
                                            (requestcount.Count() + 1).ToString().PadLeft(4, '0');
                    await _context.Requests.AddAsync(request);
                    await _context.SaveChangesAsync();

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

                    await _context.Requestclients.AddAsync(requestclient);
                    await _context.SaveChangesAsync();

                    Requestbusiness requestbusiness = new Requestbusiness
                    {
                        Requestid = request.Requestid,
                        Businessid = business.Businessid
                    };
                    await _context.Requestbusinesses.AddAsync(requestbusiness);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
