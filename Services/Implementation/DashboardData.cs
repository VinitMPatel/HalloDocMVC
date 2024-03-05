﻿using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections;
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
        private readonly IHostingEnvironment _env;

        public DashboardData(HelloDocDbContext context , IHostingEnvironment env)
        {
            _context = context;
            _env = env;
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
            List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Where(a => a.Request.Status == 2).ToList();
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

        public CaseDetails ViewCaseData(int requestId) {
                var request = _context.Requests.FirstOrDefault(m => m.Requestid == requestId);
                var requestclient = _context.Requestclients.FirstOrDefault(m => m.Requestid == requestId);
                var regiondata = _context.Regions.FirstOrDefault(m => m.Regionid == requestclient.Regionid);
                var regionList = _context.Regions.ToList();
                var data = new CaseDetails
                {
                    //ConfirmationNumber = request.Confirmationnumber,
                    requestId = requestId,
                    PatientNotes = requestclient.Notes,
                    FirstName = request.Firstname,
                    LastName = request.Lastname,
                    Email = request.Email,
                    DOB = new DateTime(Convert.ToInt32(requestclient.Intyear), DateTime.ParseExact(requestclient.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(requestclient.Intdate)),
                    PhoneNumber = request.Phonenumber,
                    Region = regiondata.Name,
                    regionList = regionList,
                    Address = requestclient.Address,
                    requestType = request.Requesttypeid
                };
                return data;
        }

        public List<Physician> PhysicianList(int regionid)
        {
            List<Physician> physicianList = _context.Physicians.Where(a => a.Regionid == regionid).ToList();
            return physicianList;
        }

        public CaseDetails ViewUploads(int requestId)
        {
            CaseDetails obj = new CaseDetails();
            //List<Requestwisefile> files = _context.Requestwisefiles(a => a.requestId == requestId).ToList();
            List<Requestwisefile> files = (from m in _context.Requestwisefiles where m.Requestid == requestId && m.Isdeleted != new BitArray(new[] { true }) select m ).ToList();
            var patientName = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault().Firstname;
            obj.FirstName = patientName;
            obj.requestId = requestId;
            obj.requestwisefile = files;
            return obj;
        }

        public void UplodingDocument(List<IFormFile> myfile, int requestId)
        {
            if (myfile.Count() > 0)
            {
                uploadFile(myfile, requestId);
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


        public void SingleDelete(int reqfileid)
        {
            var requestwisefile = _context.Requestwisefiles.FirstOrDefault(u => u.Requestwisefileid == reqfileid);
            int reqid = requestwisefile.Requestid;
            requestwisefile.Isdeleted = new BitArray(new[] { true });
            _context.Requestwisefiles.Update(requestwisefile);
            _context.SaveChanges();
        }
    }
}
