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

namespace Services.Implementation
{
    public class Dashboard : IDashboard
    {
        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;
        public Dashboard(HalloDocDbContext context , IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<patient_dashboard> PatientDashboard(string aspNetUserId)
        {
            patient_dashboard dash = new patient_dashboard();
            var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Aspnetuserid == aspNetUserId);

            dash.DOB = new DateTime(Convert.ToInt32(userdata.Intyear), 
                DateTime.ParseExact(userdata.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, 
                Convert.ToInt32(userdata.Intdate));

            var req = await _context.Requests.Where(m => m.Userid == userdata.Userid).ToListAsync();
            dash.user = userdata;
            dash.request = req;
            List<Requestwisefile> files = await _context.Requestwisefiles.ToListAsync();
                //(from m in _context.Requestwisefiles select m).ToListAsync();
            dash.requestwisefile = files;
            return dash;
        }

        public async Task<String> editing(patient_dashboard r , int id)
        {
            //int id = (int)HttpContext.Session.GetInt32("UserId");
            var userdata = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            if(userdata != null)
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

        public async Task<patient_dashboard> ViewDocuments(string aspNetUserId , int reqId)
        {
                patient_dashboard dash = new patient_dashboard();
                var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Aspnetuserid == aspNetUserId);
                dash.user = userdata;
                List<Requestwisefile> files = await _context.Requestwisefiles.Where(a=>a.Requestid == reqId).ToListAsync();

                dash.requestwisefile = files;
                dash.reqId = reqId;
                return dash;
        }

        public void UplodingDocument(patient_dashboard obj,int reqId)
        {
            if(obj.Upload != null)
            {
                uploadFile(obj.Upload,reqId);
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
                _context.Add(requestwisefile);
                _context.SaveChanges();
            }
        }
    }
}
