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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            else{
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

        public async Task SubmitTransferReqquest(int requestId , string note)
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
            if(requestId == 0)
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
                model.Skin = encounter.Skin! ;
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
            if(requestId == 0)
            {
                return new ConcludeCare();
            }
            ConcludeCare obj = new ConcludeCare();
            obj.requestId = requestId;
            Requestclient? requestclient = await _context.Requestclients.FirstOrDefaultAsync(a=>a.Requestid == requestId);
            if(requestclient != null)
            {
                obj.patientName = requestclient.Firstname + " " + requestclient.Lastname;
            }

            List<Requestwisefile> filesList = await _context.Requestwisefiles.Where(a => a.Requestid == requestId && a.Isdeleted == new BitArray(new[] {false})).ToListAsync();
            if(filesList.Count > 0)
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
            }
        }

        public async Task<Scheduling> Scheduling()
        {
            Scheduling modal = new Scheduling();
            modal.regions = await _context.Regions.ToListAsync();
            return modal;
        }

        public async Task<MonthWiseScheduling> Monthwise(DateTime currentDate , string aspNetUserId)
        {
            MonthWiseScheduling obj = new MonthWiseScheduling
            {
                date = currentDate,
                shiftdetails = await _context.Shiftdetails.Include(a => a.Shift).ThenInclude(a => a.Physician).Where(a => a.Shift.Physician.Aspnetuserid == aspNetUserId).ToListAsync()
            };
            return obj;
        }

    }
}
