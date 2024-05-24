using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;
using Data.DataContext;
using Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MathNet.Numerics.Optimization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Services.Contracts;
using Services.ViewModels;

namespace Services.Implementation
{
    public class ProviderSideServices : IProviderSideServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public ProviderSideServices(HalloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<ProviderDashboard> AllData(string aspNetUserId)
        {
            List<Requestclient> reqc = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Where(a => a.Request.Physician!.Aspnetuserid == aspNetUserId).ToListAsync();
            ProviderDashboard obj = new ProviderDashboard();
            obj.requestclients = reqc;
            return obj;
        }

        public async Task<ProviderDashboard> AllStateData(ProviderDashboard obj)
        {
            ProviderDashboard dataObj = new ProviderDashboard();

            List<Requestclient> requestclients = new List<Requestclient>();

            requestclients = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Include(a => a.Request.Encounters).Where(a => a.Request.Physician.Aspnetuserid == obj.aspNetUserId).ToListAsync();

            if (obj.requeststatus == 4)
            {
                requestclients = requestclients.Where(a => a.Request.Status == 4 || a.Request.Status == 5).ToList();
            }
            else
            {
                requestclients = requestclients.Where(a => a.Request.Status == obj.requeststatus).ToList();
            }

            requestclients = requestclients.Where(a =>
                                 (string.IsNullOrWhiteSpace(obj.searchKey) || a.Firstname.ToLower().Contains(obj.searchKey.ToLower()) || a.Lastname.ToLower().Contains(obj.searchKey.ToLower())) &&
                                 (obj.requestType == 0 || a.Request.Requesttypeid == obj.requestType)).ToList();

            dataObj.totalPages = (int)Math.Ceiling(requestclients.Count() / (double)obj.totalEntity);
            dataObj.currentpage = obj.requestedPage;
            requestclients = requestclients.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();
            dataObj.totalEntity = obj.totalEntity;
            dataObj.requestclients = requestclients;

            return dataObj;
        }

        public async Task AcceptCase(int requestId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Request? requestData = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);
                    if (requestData != null)
                    {
                        requestData.Status = 2;
                        requestData.Modifieddate = DateTime.Now;
                        requestData.Accepteddate = DateTime.Now;
                        _context.Update(requestData);
                    }
                    Requeststatuslog requeststatuslog = new Requeststatuslog
                    {
                        Requestid = requestId,
                        Status = 2,
                        //Physicianid = requestData.Physicianid,
                        Transtophysicianid = requestData!.Physicianid,
                        Createddate = DateTime.Now,

                    };
                    await _context.AddAsync(requeststatuslog);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task SubmitTransferReqquest(int requestId, string note)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    Request? request = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);
                    if (request != null)
                    {
                        request.Status = 1;
                        request.Modifieddate = DateTime.Now;
                        request.Physicianid = null;
                        Requeststatuslog requestStatusLog = new Requeststatuslog
                        {
                            Requestid = requestId,
                            Status = 1,
                            Physicianid = request.Physicianid,
                            Notes = note,
                            Createddate = DateTime.Now,
                            Transtoadmin = new System.Collections.BitArray(new[] { true })
                        };
                        _context.Update(request);
                        await _context.AddAsync(requestStatusLog);
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task Consult(int requestId)
        {
            Request? request = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if (request != null)
            {
                request.Status = 6;
                request.Modifieddate = DateTime.Now;
                _context.Update(request);
                Requeststatuslog requestStatusLog = new Requeststatuslog()
                {
                    Requestid = requestId,
                    Status = 6,
                    Createddate = DateTime.Now,
                };
                _context.Requeststatuslogs.Add(requestStatusLog);
                await _context.SaveChangesAsync();
            }
        }


        public async Task HouseCall(int requestId)
        {
            Request? request = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if (request != null)
            {
                request.Status = 5;
                request.Modifieddate = DateTime.Now;
                _context.Update(request);

                Requeststatuslog requestStatusLog = new Requeststatuslog()
                {
                    Requestid = requestId,
                    Status = 5,
                    Createddate = DateTime.Now,
                };

                _context.Requeststatuslogs.Add(requestStatusLog);
                await _context.SaveChangesAsync();
            }
        }


        public async Task HouseCalling(int requestId)
        {
            Request? request = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);

            if (request != null)
            {
                request.Status = 6;
                request.Modifieddate = DateTime.Now;
                _context.Update(request);

                Requeststatuslog requestStatusLog = new Requeststatuslog()
                {
                    Requestid = requestId,
                    Status = 6,
                    Createddate = DateTime.Now,
                };
                _context.Requeststatuslogs.Add(requestStatusLog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<EncounterFormViewModel> EncounterForm(int requestId)
        {
            if (requestId == 0)
            {
                return new EncounterFormViewModel();
            }
            EncounterFormViewModel model = new EncounterFormViewModel();
            Requestclient? requestclient = await _context.Requestclients.Include(u => u.Request).FirstOrDefaultAsync(u => u.Requestid == requestId);

            if (requestclient != null)
            {
                model.Firstname = requestclient.Firstname;
                model.Lastname = requestclient.Lastname!;
                if (requestclient?.Intyear != null && requestclient.Strmonth != null && requestclient.Intdate != null)
                {
                    model.DOB =
                        new DateTime(Convert.ToInt32(requestclient.Intyear),
                               DateTime.ParseExact(requestclient.Strmonth, "MMM", CultureInfo.InvariantCulture).Month,
                               Convert.ToInt32(requestclient.Intdate));
                }
                model.Dateofservice = requestclient!.Request.Accepteddate;
                model.Mobile = requestclient.Phonenumber!;
                model.Email = requestclient.Email!;
                model.RequestId = requestId;
                model.Location = requestclient.Address!;
            }
            Encounter? encounter = _context.Encounters.FirstOrDefault(u => u.RequestId == requestId);
            if (encounter != null)
            {
                model.Dateofservice = encounter.Date;
                model.HistoryOfIllness = encounter.HistoryIllness!;
                model.MedicalHistory = encounter.MedicalHistory!;
                model.Medication = encounter.Medications!;
                model.Allergies = encounter.Allergies!;
                model.Temp = encounter.Temp;
                model.HR = encounter.Hr;
                model.RR = encounter.Rr;
                model.BPs = encounter.BpS;
                model.BPd = encounter.BpD;
                model.O2 = encounter.O2;
                model.Pain = encounter.Pain!;
                model.Heent = encounter.Heent!;
                model.CV = encounter.Cv!;
                model.Chest = encounter.Chest!;
                model.ABD = encounter.Abd!;
                model.Extr = encounter.Extr!;
                model.Skin = encounter.Skin!;
                model.Neuro = encounter.Neuro!;
                model.Other = encounter.Other!;
                model.Diagnosis = encounter.Diagnosis!;
                model.TreatmentPlan = encounter.TreatmentPlan!;
                model.MedicationsDispended = encounter.MedicationDispensed!;
                model.Procedure = encounter.Procedures!;
                model.Followup = encounter.FollowUp!;
                if (encounter.IsFinalized[0] == true)
                {
                    model.isFinaled = true;
                }
                else
                {
                    model.isFinaled = false;
                }
            }
            return model;
        }

        public async Task SubmitEncounterForm(EncounterFormViewModel model)
        {
            Requestclient? requestclient = await _context.Requestclients.FirstOrDefaultAsync(u => u.Requestid == model.RequestId);
            if (requestclient != null)
            {
                requestclient.Firstname = model.Firstname;
                requestclient.Location = model.Location;
                requestclient.Intdate = model.DOB.Day;
                requestclient.Strmonth = model.DOB.ToString("MMM");
                requestclient.Intyear = model.DOB.Year;
                requestclient.Email = model.Email;

                _context.Update(requestclient);
            }
            await _context.SaveChangesAsync();

            var check = false;

            var encounter = await _context.Encounters.FirstOrDefaultAsync(u => u.RequestId == model.RequestId);
            if (encounter == null)
            {
                encounter = new Encounter();
                check = true;
            }
            encounter.RequestId = model.RequestId;
            encounter.Date = model.Dateofservice;
            encounter.HistoryIllness = model.HistoryOfIllness;
            encounter.MedicalHistory = model.MedicalHistory;
            encounter.Medications = model.Medication;
            encounter.Allergies = model.Allergies;
            encounter.Temp = model.Temp;
            encounter.Hr = model.HR;
            encounter.Rr = model.RR;
            encounter.BpS = model.BPs;
            encounter.BpD = model.BPd;
            encounter.O2 = model.O2;
            encounter.Pain = model.Pain;
            encounter.Heent = model.Heent;
            encounter.Cv = model.CV;
            encounter.Chest = model.Chest;
            encounter.Abd = model.ABD;
            encounter.Extr = model.Extr;
            encounter.Skin = model.Skin;
            encounter.Neuro = model.Neuro;
            encounter.Other = model.Other;
            encounter.Diagnosis = model.Diagnosis;
            encounter.TreatmentPlan = model.TreatmentPlan;
            encounter.MedicationDispensed = model.MedicationsDispended;
            encounter.Procedures = model.Procedure;
            encounter.FollowUp = model.Followup;
            encounter.IsFinalized = new BitArray(new[] { false });
            if (check == true)
            {
                await _context.Encounters.AddAsync(encounter);
            }
            else
            {
                _context.Encounters.Update(encounter);
            }
            await _context.SaveChangesAsync();
        }

        public async Task FinalizeEncounter(int requestId)
        {
            Encounter? encounter = await _context.Encounters.FirstOrDefaultAsync(a => a.RequestId == requestId);

            if (encounter != null)
            {
                encounter.IsFinalized = new BitArray(new[] { true });
                _context.Encounters.Update(encounter);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<ConcludeCare> ConcludeCareView(int requestId)
        {
            if (requestId == 0)
            {
                return new ConcludeCare();
            }
            ConcludeCare obj = new ConcludeCare();
            obj.requestId = requestId;
            Requestclient? requestclient = await _context.Requestclients.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if (requestclient != null)
            {
                obj.patientName = requestclient.Firstname + " " + requestclient.Lastname;
            }

            List<Requestwisefile> filesList = await _context.Requestwisefiles.Where(a => a.Requestid == requestId && a.Isdeleted == new BitArray(new[] { false })).ToListAsync();
            if (filesList.Count > 0)
            {
                obj.requestwisefiles = filesList;
            }

            return obj;
        }

        public async Task uploadFiles(List<IFormFile> formFiles, int requestId)
        {
            foreach (var item in formFiles)
            {
                string path = _env.WebRootPath + "/upload/" + item.FileName;
                FileStream stream = new FileStream(path, FileMode.Create);

                item.CopyTo(stream);
                Requestwisefile requestwisefile = new Requestwisefile
                {
                    Requestid = requestId,
                    Filename = item.FileName,
                    Createddate = DateTime.Now,
                    Isdeleted = new BitArray(new[] { false })
                };

                await _context.AddAsync(requestwisefile);
                await _context.SaveChangesAsync();
                stream.Close();
            }
        }

        public async Task<Scheduling> Scheduling()
        {
            Scheduling modal = new Scheduling();
            modal.regions = await _context.Regions.ToListAsync();
            return modal;
        }

        public async Task<MonthWiseScheduling> Monthwise(DateTime currentDate, string aspNetUserId)
        {
            MonthWiseScheduling obj = new MonthWiseScheduling
            {
                date = currentDate,
                shiftdetails = await _context.Shiftdetails.Include(a => a.Shift).ThenInclude(a => a.Physician).Where(a => a.Shift.Physician.Aspnetuserid == aspNetUserId).ToListAsync()
            };
            return obj;
        }

        public async Task ConcludeCareCase(int requestId, string note)
        {
            using (var transcation = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Request? requestData = await _context.Requests.FirstOrDefaultAsync(u => u.Requestid == requestId);
                    if (requestData != null)
                    {
                        requestData.Status = 8;
                        requestData.Modifieddate = DateTime.Now;
                        requestData.Completedbyphysician = new BitArray(new[] { true });

                        _context.Update(requestData);
                        await _context.SaveChangesAsync();
                    }

                    Requeststatuslog requeststatuslog = new Requeststatuslog
                    {
                        Requestid = requestId,
                        Status = 8,
                        Notes = note,
                        Createddate = DateTime.Now,
                    };
                    await _context.Requeststatuslogs.AddAsync(requeststatuslog);
                    await _context.SaveChangesAsync();
                    transcation.Commit();
                }
                catch
                {
                    transcation.Rollback();
                }
            }
        }

        public async Task<IActionResult> DownloadEncounter(int requestId)
        {
            Requestclient? requestData = await _context.Requestclients.Include(u => u.Request).FirstOrDefaultAsync(U => U.Requestid == requestId);
            Encounter? encounter = await _context.Encounters.FirstOrDefaultAsync(x => x.RequestId == requestId);
            EncounterFormViewModel model = new EncounterFormViewModel();
            if (requestData != null)
            {
                model.RequestId = requestId;
                model.Firstname = requestData.Firstname;
                model.Lastname = requestData.Lastname;
            }

            model.DOB = new DateTime(Convert.ToInt32(requestData.Intyear),
                    DateTime.ParseExact(requestData.Strmonth, "MMM", CultureInfo.InvariantCulture).Month,
                    Convert.ToInt32(requestData.Intdate));

            model.Mobile = requestData.Phonenumber;
            model.Email = requestData.Email;
            model.Location = requestData.Address;

            if (encounter != null)
            {
                model.HistoryOfIllness = encounter.HistoryIllness;
                model.MedicalHistory = encounter.MedicalHistory;
                model.Medication = encounter.Medications;
                model.Allergies = encounter.Allergies;
                model.Temp = encounter.Temp;
                model.HR = encounter.Hr;
                model.RR = encounter.Rr;
                model.BPs = encounter.BpS;
                model.BPd = encounter.BpD;
                model.O2 = encounter.O2;
                model.Pain = encounter.Pain;
                model.Heent = encounter.Heent;
                model.CV = encounter.Cv;
                model.Chest = encounter.Chest;
                model.ABD = encounter.Abd;
                model.Extr = encounter.Extr;
                model.Skin = encounter.Skin;
                model.Neuro = encounter.Neuro;
                model.Other = encounter.Other;
                model.Diagnosis = encounter.Diagnosis;
                model.TreatmentPlan = encounter.TreatmentPlan;
                model.MedicationsDispended = encounter.MedicationDispensed;
                model.Procedure = encounter.Procedures;
                model.Followup = encounter.FollowUp;
                model.isFinaled = encounter.IsFinalized![0];
            }
            var pdf = new iTextSharp.text.Document();
            using (var memoryStream = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(pdf, memoryStream);
                pdf.Open();

                // Add content to the PDF here. For example:
                pdf.Add(new Paragraph($"First Name: {model.Firstname}"));
                pdf.Add(new Paragraph($"Last Name: {model.Lastname}"));
                pdf.Add(new Paragraph($"DOB: {model.DOB}"));
                pdf.Add(new Paragraph($"Mobile: {model.Mobile}"));
                pdf.Add(new Paragraph($"Email: {model.Email}"));
                pdf.Add(new Paragraph($"Location: {model.Location}"));
                pdf.Add(new Paragraph($"History Of Illness: {model.HistoryOfIllness}"));
                pdf.Add(new Paragraph($"Medical History: {model.MedicalHistory}"));
                pdf.Add(new Paragraph($"Medication: {model.Medication}"));
                pdf.Add(new Paragraph($"Allergies: {model.Allergies}"));
                pdf.Add(new Paragraph($"Temp: {model.Temp}"));
                pdf.Add(new Paragraph($"HR: {model.HR}"));
                pdf.Add(new Paragraph($"RR: {model.RR}"));
                pdf.Add(new Paragraph($"BPs: {model.BPs}"));
                pdf.Add(new Paragraph($"BPd: {model.BPd}"));
                pdf.Add(new Paragraph($"O2: {model.O2}"));
                pdf.Add(new Paragraph($"Pain: {model.Pain}"));
                pdf.Add(new Paragraph($"Heent: {model.Heent}"));
                pdf.Add(new Paragraph($"CV: {model.CV}"));
                pdf.Add(new Paragraph($"Chest: {model.Chest}"));
                pdf.Add(new Paragraph($"ABD: {model.ABD}"));
                pdf.Add(new Paragraph($"Extr: {model.Extr}"));
                pdf.Add(new Paragraph($"Skin: {model.Skin}"));
                pdf.Add(new Paragraph($"Neuro: {model.Neuro}"));
                pdf.Add(new Paragraph($"Other: {model.Other}"));
                pdf.Add(new Paragraph($"Diagnosis: {model.Diagnosis}"));
                pdf.Add(new Paragraph($"Treatment Plan: {model.TreatmentPlan}"));
                pdf.Add(new Paragraph($"Medications Dispended: {model.MedicationsDispended}"));
                pdf.Add(new Paragraph($"Procedure: {model.Procedure}"));
                pdf.Add(new Paragraph($"Followup: {model.Followup}"));
                pdf.Add(new Paragraph($"Is Finaled: {model.isFinaled}"));

                pdf.Close();
                writer.Close();

                var bytes = memoryStream.ToArray();
                var result = new FileContentResult(bytes, "application/pdf");
                result.FileDownloadName = "Encounter_" + model.RequestId + ".pdf";
                return result;
            }
        }


        public async Task<InvoicingViewModel> InvocingData(string aspNetUserId, string startDate)
        {
            if (aspNetUserId == null)
            {
                return new InvoicingViewModel();
            }

            InvoicingViewModel obj = new InvoicingViewModel();
            obj.shiftdetails = await _context.Shiftdetails.Include(a => a.Shift).ThenInclude(a => a.Physician).Where(a => a.Shift.Physician.Aspnetuserid == aspNetUserId).ToListAsync();
            DateOnly date = DateOnly.Parse(startDate);
            List<TimeSheetData> data = new List<TimeSheetData>();
            if (date.Day == 1)
            {
                for (int i = 0; i < 15; i++)
                {
                    Timesheet? timesheet = await _context.Timesheets.Include(a => a.Physician).FirstOrDefaultAsync(a => a.Date == date && a.Physician.Aspnetuserid == aspNetUserId);
                    TimeSheetData ts = new TimeSheetData();
                    if (timesheet != null)
                    {
                        if (timesheet.Oncallhours != null)
                        {
                            ts.totalHour = (float)timesheet.Oncallhours;
                        }
                        if (timesheet.Housecall != null)
                        {
                            ts.houseCall = (int)timesheet.Housecall;
                        }
                        if (timesheet.Consult != null)
                        {
                            ts.consult = (int)timesheet.Consult;
                        }
                        if (timesheet.Isweekend != null)
                        {
                            ts.holidays = (bool)timesheet.Isweekend;
                        }
                    }
                    else
                    {
                        ts.totalHour = 0;
                        ts.houseCall = 0;
                        ts.consult = 0;
                        ts.holidays = false;
                    }
                    data.Add(ts);
                    date = date.AddDays(1);
                }

            }
            else
            {
                int i = 0;
                DateOnly currentDate = date;
                while (currentDate.Month == date.Month)
                {
                    Timesheet? timesheet = await _context.Timesheets.Include(a => a.Physician).FirstOrDefaultAsync(a => a.Date == currentDate && a.Physician.Aspnetuserid == aspNetUserId);
                    TimeSheetData ts = new TimeSheetData();
                    if (timesheet != null)
                    {
                        if (timesheet.Oncallhours != null)
                        {
                            ts.totalHour = (float)timesheet.Oncallhours;
                        }
                        if (timesheet.Housecall != null)
                        {
                            ts.houseCall = (int)timesheet.Housecall;
                        }
                        if (timesheet.Consult != null)
                        {
                            ts.consult = (int)timesheet.Consult;
                        }
                        if (timesheet.Isweekend != null)
                        {
                            ts.holidays = (bool)timesheet.Isweekend;
                        }
                    }
                    else
                    {
                        ts.totalHour = 0;
                        ts.houseCall = 0;
                        ts.consult = 0;
                        ts.holidays = false;
                    }
                    data.Add(ts);
                    currentDate = currentDate.AddDays(1);
                    i++;
                }
            }
            obj.timeSheetData = data;
            return obj;
        }

        public async Task SubmitTimeSheet(InvoicingViewModel obj, string aspNetUserId)
        {

            Timesheet? timeSheetData = await _context.Timesheets.Include(a => a.Physician).FirstOrDefaultAsync(a => a.Date == DateOnly.Parse(obj.startDate) && a.Physician.Aspnetuserid == aspNetUserId);
            Physician? physician = await _context.Physicians.FirstOrDefaultAsync(a => a.Aspnetuserid == aspNetUserId);
            if (timeSheetData == null)
            {
                foreach (var item in obj.timeSheetData)
                {
                    Timesheet timesheet = new Timesheet
                    {
                        Physicianid = physician.Physicianid,
                        Isweekend = item.holidays,
                        Oncallhours = (decimal)item.totalHour,
                        Housecall = item.houseCall,
                        Consult = item.consult,
                        Createdby = aspNetUserId,
                        Modifiedby = aspNetUserId,
                        Createddate = DateTime.Now,
                        Modifieddate = DateTime.Now,
                        Date = item.crrDate
                    };
                    await _context.Timesheets.AddAsync(timesheet);
                }
                
            }
            else
            {
                DateOnly currentDate = DateOnly.Parse(obj.startDate);
                while(currentDate <= obj.endDate)
                {
                    foreach (var item in obj.timeSheetData)
                    { 
                        Timesheet? timesheet = await _context.Timesheets.Include(a => a.Physician).FirstOrDefaultAsync(a => a.Date == currentDate && a.Physician.Aspnetuserid == aspNetUserId);
                        timesheet.Isweekend = item.holidays;
                        timesheet.Oncallhours = (decimal)item.totalHour;
                        timesheet.Consult = item.consult;
                        timesheet.Housecall = item.houseCall;
                        timesheet.Modifiedby = aspNetUserId;
                        timesheet.Modifieddate = DateTime.Now;

                        _context.Timesheets.Update(timesheet);
                        currentDate = currentDate.AddDays(1);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}
