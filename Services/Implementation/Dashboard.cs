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

        public patient_dashboard PatientDashboard(int id)
        {
            patient_dashboard dash = new patient_dashboard();
            var userdata = _context.Users.Where(u => u.Userid == id).FirstOrDefault();
            dash.DOB = new DateTime(Convert.ToInt32(userdata.Intyear), DateTime.ParseExact(userdata.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(userdata.Intdate));
            var req = _context.Requests.Where(m => m.Userid == id);
            dash.user = userdata;
            dash.request = req.ToList();
            List<Requestwisefile> files = (from m in _context.Requestwisefiles select m).ToList();
            dash.requestwisefile = files;
            return dash;
        }

        public String editing(patient_dashboard r , int id)
        {
            //int id = (int)HttpContext.Session.GetInt32("UserId");
            var userdata = _context.Users.FirstOrDefault(m => m.Userid == id);

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
            _context.SaveChanges();

            return userdata.Firstname;
        }

        public patient_dashboard ViewDocuments(int userId , int reqId)
        {
                patient_dashboard dash = new patient_dashboard();
                var userdata = _context.Users.Where(u => u.Userid == userId).FirstOrDefault();
                dash.user = userdata;
                List<Requestwisefile> files = (from m in _context.Requestwisefiles where m.Requestid == reqId select m).ToList();
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
