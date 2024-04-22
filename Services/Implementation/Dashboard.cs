using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Common.Helper;

namespace Services.Implementation
{
    public class Dashboard : IDashboard
    {
        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;
        public Dashboard(HalloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<patient_dashboard> PatientDashboard(string aspNetUserId)
        {
            if(aspNetUserId == "")
            {
                return new patient_dashboard();
            }
            patient_dashboard dash = new patient_dashboard();
            var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Aspnetuserid == aspNetUserId);

            if (userdata.Intyear != null && userdata.Strmonth != null && userdata.Intdate != null)
            {
                dash.DOB = new DateTime(Convert.ToInt32(userdata.Intyear),
                    DateTime.ParseExact(userdata.Strmonth, "MMM", CultureInfo.InvariantCulture).Month,
                    Convert.ToInt32(userdata.Intdate));
            }

            var req = await _context.Requests.Where(m => m.Userid == userdata.Userid).ToListAsync();
            dash.user = userdata;
            dash.request = req;
            List<Requestwisefile> files = await _context.Requestwisefiles.ToListAsync();

            dash.requestwisefile = files;
            return dash;
        }

        public async Task<string> EditProfile(patient_dashboard r, string aspNetUserId)
        {

            //int id = (int)HttpContext.Session.GetInt32("UserId");
            var userdata = await _context.Users.FirstOrDefaultAsync(m => m.Aspnetuserid == aspNetUserId);
            if (userdata != null)
            {
                userdata.Firstname = r.user.Firstname;
                userdata.Lastname = r.user.Lastname;
                userdata.Street = r.user.Street;
                userdata.City = r.user.City;
                userdata.State = r.user.State;
                userdata.Zip = r.user.Zip;
                userdata.Modifieddate = DateTime.Now;
                userdata.Intyear = int.Parse(r.DOB.ToString("yyyy"));
                userdata.Intdate = int.Parse(r.DOB.ToString("dd"));
                userdata.Strmonth = r.DOB.ToString("MMM");
                userdata.Mobile = r.user.Mobile;
                _context.Users.Update(userdata);
            }

            await _context.SaveChangesAsync();

            return userdata.Firstname;
        }

        public async Task<patient_dashboard> ViewDocuments(string aspNetUserId, int reqId)
        {
            patient_dashboard dash = new patient_dashboard();
            var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Aspnetuserid == aspNetUserId);
            dash.user = userdata;
            List<Requestwisefile> files = await _context.Requestwisefiles.Where(a => a.Requestid == reqId).ToListAsync();

            dash.requestwisefile = files;
            dash.reqId = reqId;
            return dash;
        }

        public void UplodingDocument(patient_dashboard obj, int reqId)
        {
            if (obj.Upload != null)
            {
                uploadFile(obj.Upload, reqId);
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
                stream.Close();
                _context.Add(requestwisefile);
                _context.SaveChanges();
            }
        }

        public async Task<PatientInfo> RequestForSelfData(string aspNetUserId)
        {
            if (aspNetUserId == null)
            {
                return new PatientInfo();
            }
            PatientInfo model = new PatientInfo();
            User? user = await _context.Users.FirstOrDefaultAsync(a => a.Aspnetuserid == aspNetUserId);
            if (user != null)
            {
                model.FirstName = user.Firstname;
                model.LastName = user.Lastname;
                model.Email = user.Email;
                if (user.Intyear != null && user.Strmonth != null && user.Intdate != null)
                {
                    model.DOB = new DateOnly(Convert.ToInt32(user.Intyear),
                        DateTime.ParseExact(user.Strmonth, "MMM", CultureInfo.InvariantCulture).Month,
                        Convert.ToInt32(user.Intdate));
                }
                model.PhoneNumber = user.Mobile;
            }
            return model;
        }

        public async Task<FamilyFriendRequest> RequestForElseData(string aspNetUserId)
        {
            if (aspNetUserId == null)
            {
                return new FamilyFriendRequest();
            }
            FamilyFriendRequest familyFriendRequest = new FamilyFriendRequest();
            User? user = await _context.Users.FirstOrDefaultAsync(a => a.Aspnetuserid == aspNetUserId);
            if (user != null)
            {
                familyFriendRequest.FirstName = user.Firstname;
                familyFriendRequest.LastName = user.Lastname;
                familyFriendRequest.Email = user.Email;
                familyFriendRequest.Mnumber = user.Mobile;
            }
            return familyFriendRequest;
        }

        public async Task<(string, string)> FindUser(LoginPerson model)
        {
            if (model == null)
            {
                return ("", "");
            }

            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(u => u.Email == model.email);
            return (aspnetuser!.Id, aspnetuser.Email)!;

        }

        public async Task UpdatePassword(LoginPerson model)
        {
            if(model == null)
            {
                return;
            }
            Aspnetuser? aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(a => a.Id == model.aspNetUserId);
            string encryptedPassword = EncryptDecryptHelper.Encrypt(model.password);
            aspnetuser.Passwordhash = encryptedPassword;
            _context.Update(aspnetuser);
            await _context.SaveChangesAsync();
        }
    }
}
