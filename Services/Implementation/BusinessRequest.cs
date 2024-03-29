﻿using Data.DataContext;
using Data.Entity;
using Services.ViewModels;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class BusinessRequest : IBusinessRequest
    {
        private readonly HalloDocDbContext _context;

        public BusinessRequest(HalloDocDbContext context)
        {
            _context = context;
        }

        public void BusinessInsert(BusinessRequestData r)
        {

            var aspnetuser = _context.Aspnetusers.Where(m => m.Email == r.PatientEmail).FirstOrDefault();
            var user = _context.Users.Where(m => m.Email == r.PatientEmail).FirstOrDefault();

            if (user != null)
            {
                Business business = new Business
                {
                    Name = r.Business,
                    Address1 = r.Street + ", " + r.City,
                    City = r.City,
                    Regionid = 1,
                    Zipcode = r.ZipCode,
                    Phonenumber = r.PatientMobileNumber,
                    Createddate = DateTime.Now,
                    Status = 1,
                    Createdby = aspnetuser.Id,
                    Modifiedby = aspnetuser.Id

                };
                _context.Businesses.Add(business);
                _context.SaveChanges();

                string region = _context.Regions.FirstOrDefault(a => a.Regionid == user.Regionid).Abbreviation;
                var requestcount = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date && a.Createddate.Month == DateTime.Now.Month && a.Createddate.Year == DateTime.Now.Year && a.Userid == user.Userid).ToList();
                Request request = new Request
                {
                    Userid = user.Userid,
                    Requesttypeid = 4,
                    Firstname = r.FirstName,
                    Lastname = r.LastName,
                    Phonenumber = r.Mnumber,
                    Email = r.Email,
                    Status = 1,
                    Createddate = DateTime.Now,
                    Modifieddate = DateTime.Now,
                    Relationname = r.Business,
                    Confirmationnumber = region.Substring(0, 2) + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                                            DateTime.Now.Year.ToString().Substring(2) + r.PatientLastName.ToUpper().Substring(0, 2) + r.PatientFirstName.ToUpper().Substring(0, 2) +
                                            (requestcount.Count() + 1).ToString().PadLeft(4, '0')
                };
                _context.Requests.Add(request);
                _context.SaveChanges();

                Requestclient requestclient = new Requestclient
                {
                    Requestid = request.Requestid,
                    Firstname = r.PatientFirstName,
                    Lastname = r.PatientLastName,
                    Street = r.Street,
                    Email = r.PatientEmail,
                    Phonenumber = r.PatientMobileNumber,
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
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();

                Requestbusiness requestbusiness = new Requestbusiness
                {
                    Requestid = request.Requestid,
                    Businessid = business.Businessid
                };
                _context.Requestbusinesses.Add(requestbusiness);
                _context.SaveChanges();
            }
        }
    }
}
