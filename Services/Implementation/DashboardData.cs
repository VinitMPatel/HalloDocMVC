﻿using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class DashboardData : IDashboardData
    {

        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public DashboardData(HalloDocDbContext context , IHostingEnvironment env)
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

        public AdminDashboard AllStateData(String status , String requesttype  , int currnetPage , string searchKey = "")
        {

            if (requesttype == "0" || requesttype == null)
            {
                List<Requestclient> reqc = new List<Requestclient>();
                AdminDashboard obj = new AdminDashboard();

                if (status == "4")
                {
                    reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status.ToString() == "4" || a.Request.Status.ToString() == "5").ToList();
                }
                else if(status == "3")
                {
                    reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.User.Region).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status.ToString() == "3" || a.Request.Status.ToString() == "7" || a.Request.Status.ToString() == "8").ToList();
                }
                else
                {
                    reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status.ToString() == status).ToList();
                }

                if (!string.IsNullOrWhiteSpace(searchKey))
                {
                    reqc = reqc.Where(a => a.Firstname.ToLower().Contains(searchKey.ToLower()) || a.Lastname.ToLower().Contains(searchKey.ToLower())).ToList();
                }

                if (currnetPage != 0)
                {
                obj.totalPages = (int)Math.Ceiling(reqc.Count() / 2.00);
                obj.currentpage = currnetPage;
                reqc = reqc.Skip((currnetPage - 1) * 2).Take(2).ToList();
                }
                obj.requestclients = reqc;
                return obj;
            }
            else
            {
                List<Requestclient> reqc = _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(r=>r.Request.User.Region).Include(a => a.Request.Requeststatuslogs).Where(a => a.Request.Status.ToString() == status && a.Request.Requesttypeid.ToString() == requesttype).ToList();
                AdminDashboard obj = new AdminDashboard();

                if (!string.IsNullOrWhiteSpace(searchKey))
                {
                    reqc = reqc.Where(a => a.Request.Firstname.ToLower().Contains(searchKey.ToLower()) || a.Request.Lastname.ToLower().Contains(searchKey.ToLower())).ToList();
                }
                if (currnetPage != 0)
                {
                    obj.totalPages = (int)Math.Ceiling(reqc.Count() / 2.00);
                    obj.currentpage = currnetPage;
                    reqc = reqc.Skip((currnetPage - 1) * 2).Take(2).ToList();
                }
                    obj.requestclients = reqc;
                return obj;
            }
        }

        public CaseActionDetails ViewCaseData(int requestId) {
                var request = _context.Requests.FirstOrDefault(m => m.Requestid == requestId);
                var requestclient = _context.Requestclients.FirstOrDefault(m => m.Requestid == requestId);
                var regiondata = _context.Regions.FirstOrDefault(m => m.Regionid == requestclient.Regionid);
                var regionList = _context.Regions.ToList();
                var data = new CaseActionDetails
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
                    requestType = request.Requesttypeid,
                    ConfirmationNumber = request.Confirmationnumber
                };
                return data;
        }

        public List<Physician> PhysicianList(int regionid)
        {
            List<Physician> physicianList = _context.Physicians.Where(a => a.Regionid == regionid).ToList();
            return physicianList;
        }

        public CaseActionDetails ViewUploads(int requestId)
        {
            CaseActionDetails obj = new CaseActionDetails();
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

        public byte[] DownloadExcle(AdminDashboard model)
        {
            using (var workbook = new XSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet("FilteredRecord");
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Sr No.");
                headerRow.CreateCell(1).SetCellValue("Request Id");
                headerRow.CreateCell(2).SetCellValue("Patient Name");
                headerRow.CreateCell(3).SetCellValue("Patient DOB");
                headerRow.CreateCell(4).SetCellValue("RequestorName");
                headerRow.CreateCell(5).SetCellValue("RequestedDate");
                headerRow.CreateCell(6).SetCellValue("PatientPhone");
                headerRow.CreateCell(7).SetCellValue("TransferNotes");
                headerRow.CreateCell(8).SetCellValue("RequestorPhone");
                headerRow.CreateCell(9).SetCellValue("RequestorEmail");
                headerRow.CreateCell(10).SetCellValue("Address");
                headerRow.CreateCell(11).SetCellValue("Notes");
                headerRow.CreateCell(12).SetCellValue("ProviderEmail");
                headerRow.CreateCell(13).SetCellValue("PatientEmail");
                headerRow.CreateCell(14).SetCellValue("RequestType");
                //headerRow.CreateCell(15).SetCellValue("Region");
                headerRow.CreateCell(16).SetCellValue("PhysicainName");
                headerRow.CreateCell(17).SetCellValue("Status");

                for (int i = 0; i < model.requestclients.Count; i++)
                {
                    var reqclient = model.requestclients.ElementAt(i);
                    var type = "";
                    if (reqclient.Request.Requesttypeid == 1)
                    {
                        type = "Patient";
                    }
                    else if (reqclient.Request.Requesttypeid == 2)
                    {
                        type = "Family";
                    }
                    else if (reqclient.Request.Requesttypeid == 4)
                    {
                        type = "Business";
                    }
                    else if (reqclient.Request.Requesttypeid == 3)
                    {
                        type = "Concierge";
                    }
                    IRow row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(i + 1);
                    row.CreateCell(1).SetCellValue(reqclient.Request.Requestid);
                    row.CreateCell(2).SetCellValue(reqclient.Firstname);
                    row.CreateCell(3).SetCellValue(reqclient.Intdate + "/" + reqclient.Strmonth + "/" + reqclient.Intyear);
                    row.CreateCell(4).SetCellValue(reqclient.Request.Firstname);
                    row.CreateCell(5).SetCellValue(reqclient.Request.Createddate);
                    row.CreateCell(6).SetCellValue(reqclient.Phonenumber);
                    if (reqclient.Request.Requeststatuslogs.Count() == 0)
                    {
                        row.CreateCell(7).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(7).SetCellValue(reqclient.Request.Requeststatuslogs.ElementAt(0).Notes);
                    }
                    row.CreateCell(8).SetCellValue(reqclient.Request.Phonenumber);
                    row.CreateCell(9).SetCellValue(reqclient.Request.Email);
                    row.CreateCell(10).SetCellValue(reqclient.Request.Requestclients.ElementAt(0).Address);
                    row.CreateCell(11).SetCellValue(reqclient.Notes);
                    if (reqclient.Request.Physician == null)
                    {
                        row.CreateCell(12).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue(reqclient.Request.Physician.Email);
                    }
                    row.CreateCell(13).SetCellValue(reqclient.Email);
                    row.CreateCell(14).SetCellValue(type);
                    //row.CreateCell(15).SetCellValue(reqclient.Region.Name);
                    if (reqclient.Request.Physician == null)
                    {
                        row.CreateCell(16).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(16).SetCellValue(reqclient.Request.Physician.Firstname);
                    }
                    row.CreateCell(17).SetCellValue(reqclient.Request.Status);
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }
    }
}
