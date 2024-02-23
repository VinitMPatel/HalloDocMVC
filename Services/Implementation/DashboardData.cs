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

        public AdminDashboard NewStateData()
        {
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Where(a => a.Request.Status == 1).ToList();
            AdminDashboard obj = new AdminDashboard();
            obj.requestclients = reqc;
            return obj;
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
    }
}
