using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage;

namespace Services.Implementation
{
    public class CaseActions : ICaseActions
    {
        private readonly HalloDocDbContext _context;

        public CaseActions(HalloDocDbContext context)
        {
            _context = context;
        }

        public ViewModels.CaseActions AssignCase(int requestId)
        {
            ViewModels.CaseActions obj = new ViewModels.CaseActions();
            obj.regionList = _context.Regions.ToList();
            obj.requestId = requestId;
            return obj;
        }

        public ViewModels.CaseActions CancelCase(int requestId)
        {
            ViewModels.CaseActions obj = new ViewModels.CaseActions();
            var name = _context.Requests.Where(a => a.Requestid == requestId).Select(a => a.Firstname).FirstOrDefault();
            obj.cancelList = _context.Casetags.ToList();
            obj.requestId = requestId;
            if (name != null)
            {
                obj.patietName = name;
            }
            return obj;
        }

        public ViewModels.CaseActions BlockCase(int requestId)
        {
            ViewModels.CaseActions obj = new ViewModels.CaseActions();
            var name = _context.Requests.Where(a => a.Requestid == requestId).Select(a => a.Firstname).FirstOrDefault();
            obj.requestId = requestId;
            if (name != null)
            {
                obj.patietName = name;
            }
            return obj;
        }

        public ViewModels.CaseActions Orders(int requestId)
        {
            ViewModels.CaseActions obj = new ViewModels.CaseActions();
            return obj;
        }

        public AgreementDetails Agreement(int requestId)
        {
            AgreementDetails obj = new AgreementDetails();
            Data.Entity.Request requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
            if (requestData != null)
            {
                obj.mobile = requestData.Phonenumber;
                obj.email = requestData.Email;
                obj.type = requestData.Requesttypeid;
            }
            return obj;
        }

        public async Task AgreeAgreement(int requestId)
        {
            Data.Entity.Request? requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
            if (requestData != null)
            {
                requestData.Modifieddate = DateTime.Now;
                requestData.Status = 4;
                _context.Requests.Update(requestData);
            }
            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = requestId,
                Status = 4,
                Createddate = DateTime.Now,
            };
            await _context.AddAsync(requeststatuslog);
            await _context.SaveChangesAsync();
        }

        public async Task CancelAgreement(int requestId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Data.Entity.Request? requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
                    if (requestData != null)
                    {
                        requestData.Modifieddate = DateTime.Now;
                        requestData.Status = 7;
                        _context.Requests.Update(requestData);
                    }

                    Requeststatuslog requeststatuslog = new Requeststatuslog
                    {
                        Requestid = requestId,
                        Status = 7,
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

        public async Task SubmitAssign(int requestId, int physicianId, string assignNote)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Data.Entity.Request? requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
                    if (requestData != null)
                    {
                        requestData.Modifieddate = DateTime.Now;
                        requestData.Status = 2;
                        requestData.Physicianid = physicianId;
                        _context.Requests.Update(requestData);
                    }

                    Requeststatuslog requeststatuslog = new Requeststatuslog
                    {
                        Requestid = requestId,
                        Status = 2,
                        Physicianid = physicianId,
                        Notes = assignNote,
                        Createddate = DateTime.Now,
                        Transtophysicianid = physicianId
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

        public async Task SubmitCancel(int requestId, int caseId, string cancelNote)
        {
            Data.Entity.Request? requestData = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            if(requestData != null)
            {
                requestData.Modifieddate = DateTime.Now;
                requestData.Casetag = _context.Casetags.FirstOrDefault(a => a.Casetagid == caseId).Name;
                requestData.Status = 3;
                _context.Requests.Update(requestData);
            }

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = requestId,
                Status = 3,
                Notes = cancelNote,
                Createddate = DateTime.Now
            };
            await _context.AddAsync(requeststatuslog);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitBlock(int requestId, string blockNote)
        {
            Data.Entity.Request? requestData = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            if( requestData != null )
            {
                requestData.Modifieddate = DateTime.Now;
                requestData.Status = 11;
                _context.Requests.Update(requestData);
            }

            Blockrequest blockrequest = new Blockrequest
            {
                Requestid = requestId,
                Phonenumber = requestData.Phonenumber,
                Email = requestData.Email,
                Reason = blockNote,
                Createddate = DateTime.Now,
            };
            await _context.AddAsync(blockrequest);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitNotes(int requestId, string notes, CaseActionDetails obj)
        {
            Data.Entity.Request? requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
            Requestnote requestnote = new Requestnote();
            var existRequestNote = _context.Requestnotes.FirstOrDefault(a => a.Requestid == requestId);
            requestnote.Requestid = requestId;
            requestnote.Createddate = DateTime.Now;
            requestnote.Adminnotes = notes;
            requestnote.Createdby = "1";
            await _context.Requestnotes.AddAsync(requestnote);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitOrder(Orders obj)
        {
            Orderdetail orderdetail = new Orderdetail
            {
                Vendorid = obj.vendorid,
                Requestid = obj.requestId,
                Faxnumber = obj.Fax,
                Email = obj.Email,
                Businesscontact = obj.Contact,
                Prescription = obj.prescription,
                Noofrefill = obj.refil,
                Createddate = DateTime.Now,
                Createdby = obj.createdby
            };
            await _context.Orderdetails.AddAsync(orderdetail);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitTransfer(int requestId, int physicianId, string transferNote)
        {
            Data.Entity.Request? requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
            if(requestData != null)
            {
                requestData.Modifieddate = DateTime.Now;
                requestData.Physicianid = physicianId;
                _context.Requests.Update(requestData);
            }

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = requestId,
                Status = 2,
                Physicianid = physicianId,
                Notes = transferNote,
                Createddate = DateTime.Now,
                Transtophysicianid = physicianId
            };
            await _context.Requeststatuslogs.AddAsync(requeststatuslog);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitClearCase(int requestId)
        {
            Data.Entity.Request? requestData = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
            if (requestData != null)
            {
                requestData.Modifieddate = DateTime.Now;
                requestData.Status = 10;
                _context.Requests.Update(requestData);
            }

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = requestId,
                Status = 10,
                Createddate = DateTime.Now,
            };
            await _context.Requeststatuslogs.AddAsync(requeststatuslog);
            await _context.SaveChangesAsync();
        }

        public void SendingAgreement(int requestId, string email, string url)
        {
            try
            {
                var mail = "tatva.dotnet.vinitpatel@outlook.com";
                var password = "016@ldce";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = "Agreement",
                    Body = "You can view agreement by using this link : " + url,
                    IsBodyHtml = true // Set to true if your message contains HTML
                };

                mailMessage.To.Add(email);
                client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public CloseCase CloseCase(int requestId)
        {
            var requestData = _context.Requests.Include(a => a.Requestclients).FirstOrDefault(a => a.Requestid == requestId);
            CloseCase obj = new CloseCase();
            obj.firstName = requestData.Firstname;
            obj.lastName = requestData.Lastname;
            obj.email = requestData.Email;
            obj.DOB = new DateTime(Convert.ToInt32(requestData.Requestclients.First().Intyear),
                DateTime.ParseExact(requestData.Requestclients.First().Strmonth, "MMM", CultureInfo.InvariantCulture).Month,
                Convert.ToInt32(requestData.Requestclients.First().Intdate));
            obj.mobileNumber = requestData.Phonenumber;
            obj.requestId = requestId;
            return obj;
        }

        public async Task CloseCaseChanges(string email, int requestId, string phone)
        {
            var requestData = _context.Requests.Include(a => a.Requestclients).FirstOrDefault(a => a.Requestid == requestId);
            if(requestData != null)
            {
                requestData.Email = email;
                requestData.Phonenumber = phone;
                _context.Update(requestData);
                await _context.SaveChangesAsync();
            }
        }

    }
}
