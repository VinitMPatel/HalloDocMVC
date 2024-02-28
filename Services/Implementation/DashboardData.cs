using Data.DataContext;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class DashboardData : IDashboardData
    {

        private readonly HelloDocDbContext _context;

        public DashboardData(HelloDocDbContext context)
        {
            _context = context;
        }


        public AdminDashboard AllData()
        {
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).ToList();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            return obj;
        }

        public AdminDashboard NewStateData(String status , String requesttype)
        {

            if (status == null && requesttype == null)
            {
                List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Status == 1).ToList();
                AdminDashboard obj = new AdminDashboard();
                obj.requestclients = reqc;
                return obj;
            }
            else
            {
                List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Status.ToString() == status && a.Request.Requesttypeid.ToString() == requesttype).ToList();
                AdminDashboard obj = new AdminDashboard();
                obj.requestclients = reqc;
                return obj;
            }
        }

        public AdminDashboard PendingStateData()
        {
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Status == 2).ToList();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            return obj;
        }

        public AdminDashboard ActiveStateData()
        {
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Status == 4 ||  a.Request.Status == 5).ToList();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            return obj;
        }

        public AdminDashboard ConcludeStateData()
        {
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Status == 6).ToList();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            return obj;
        }

        public AdminDashboard ToCloseStateData()
        {
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Status == 3 || a.Request.Status == 7 || a.Request.Status == 8).ToList();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            return obj;
        }

        public AdminDashboard UnpaidStateData()
        {
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Status == 9).ToList();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            return obj;
        }

        public ViewCase ViewCaseData(int requestId) {
                var requestclient = _context.Requestclients.FirstOrDefault(m => m.Requestid == requestId);
                var request = _context.Requests.FirstOrDefault(m => m.Requestid == requestId);
                var regiondata = _context.Regions.FirstOrDefault(m => m.Regionid == requestclient.Regionid);
                var data = new ViewCase
                {
                    //ConfirmationNumber = request.Confirmationnumber,
                    PatientNotes = requestclient.Notes,
                    FirstName = requestclient.Firstname,
                    LastName = requestclient.Lastname,
                    Email = requestclient.Email,
                    DOB = new DateTime(Convert.ToInt32(requestclient.Intyear), DateTime.ParseExact(requestclient.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(requestclient.Intdate)),
                    PhoneNumber = requestclient.Phonenumber,
                    Region = regiondata,
                    Address = requestclient.Address,

                };
                return data;
        }
    }
}
