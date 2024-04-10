using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using Services.ViewModels;
using Common.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Services.Implementation
{
    public class PatientRequest : IPatientRequest
    {

        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public PatientRequest(HalloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task Insert(PatientInfo r)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    User? user = await _context.Users.FirstOrDefaultAsync(m => m.Email == r.Email);
                    
                    if (user == null)
                    {

                        Aspnetuser aspnetuser1 = new Aspnetuser();
                        aspnetuser1.Id = Guid.NewGuid().ToString();
                        string encryptPassword = EncryptDecryptHelper.Encrypt(r.Password);
                        aspnetuser1.Passwordhash = encryptPassword;
                        aspnetuser1.Email = r.Email;
                        String username = r.FirstName + r.LastName;
                        aspnetuser1.Username = username;
                        aspnetuser1.Phonenumber = r.PhoneNumber;
                        aspnetuser1.Createddate = DateTime.Now;
                        aspnetuser1.Modifieddate = DateTime.Now;
                        await _context.Aspnetusers.AddAsync(aspnetuser1);
                        
                        aspnetuser1 = aspnetuser1;
                         
                        User newUser = new User
                        {
                            Aspnetuserid = aspnetuser1.Id,
                            Firstname = r.FirstName,
                            Lastname = r.LastName,
                            Email = r.Email,
                            Mobile = r.PhoneNumber,
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
                            Regionid = 1,
                        };
                        user = newUser;
                        await _context.Users.AddAsync(user);
                    }
                    string region = _context.Regions.FirstOrDefault(a => a.Regionid == user.Regionid).Abbreviation;
                    var requestcount = await _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date && a.Createddate.Month == DateTime.Now.Month && a.Createddate.Year == DateTime.Now.Year && a.Userid == user.Userid).ToListAsync();
                    Request request = new Request
                    {
                        Requesttypeid = 1,
                        Firstname = r.FirstName,
                        Lastname = r.LastName,
                        Phonenumber = r.PhoneNumber,
                        Email = r.Email,
                        Status = 1,
                        Createddate = DateTime.Now,
                        Modifieddate = DateTime.Now,
                        Confirmationnumber = region.Substring(0, 2) + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + 
                                            DateTime.Now.Year.ToString().Substring(2) + r.LastName.ToUpper().Substring(0, 2) + r.FirstName.ToUpper().Substring(0, 2) +
                                            (requestcount.Count() + 1).ToString().PadLeft(4, '0'),
                        User = user,
                    };
                    await _context.Requests.AddAsync(request);

                    Requestclient requestclient = new Requestclient
                    {
                        Request = request,
                        Firstname = r.FirstName,
                        Lastname = r.LastName,
                        Phonenumber = r.PhoneNumber,
                        Notes = r.Symptoms,
                        Email = r.Email,
                        Street = r.Street,
                        City = r.City,
                        State = r.State,
                        Zipcode = r.ZipCode,
                        Address = r.Street + ", " + r.City + ", " + r.State,
                        Regionid = 1,
                        Intyear = int.Parse(r.DOB.ToString("yyyy")),
                        Intdate = int.Parse(r.DOB.ToString("dd")),
                        Strmonth = r.DOB.ToString("MMM")
                    };
                    await _context.Requestclients.AddAsync(requestclient);

                    await _context.SaveChangesAsync();

                    if (r.Upload != null)
                    {
                        await uploadFile(r.Upload, request.Requestid);
                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task uploadFile(List<IFormFile> upload, int id)
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
                await _context.AddAsync(requestwisefile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task NewAccount(Aspnetuser model)
        {
            Aspnetuser aspnetuser1 = new Aspnetuser();
            aspnetuser1.Id = Guid.NewGuid().ToString();
            aspnetuser1.Passwordhash = model.Passwordhash;
            aspnetuser1.Email = model.Email;
            aspnetuser1.Username = "Temp";

            await _context.Aspnetusers.AddAsync(aspnetuser1);
            await _context.SaveChangesAsync();

        }
    }
}
