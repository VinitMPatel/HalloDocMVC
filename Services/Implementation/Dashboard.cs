using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class Dashboard : IDashboard
    {
        private readonly HelloDocDbContext _context;
        public Dashboard(HelloDocDbContext context)
        {
            _context = context;
        }

        public patient_dashboard PatientDashboard(int id)
        {
            patient_dashboard dash = new patient_dashboard();
            var userdata = _context.Users.Where(u => u.Userid == id).FirstOrDefault();
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
                return dash;
        }
    }
}
