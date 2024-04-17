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

        public async Task<ViewModels.CaseActions> AssignCase(int requestId)
        {
            ViewModels.CaseActions obj = new ViewModels.CaseActions();
            obj.regionList = await _context.Regions.ToListAsync();
            obj.requestId = requestId;
            return obj;
        }

        public async Task<ViewModels.CaseActions> CancelCase(int requestId)
        {
            ViewModels.CaseActions obj = new ViewModels.CaseActions();
            var name = await _context.Requestclients.Where(a => a.Requestid == requestId).Select(a => a.Firstname).FirstOrDefaultAsync();
            obj.cancelList = await _context.Casetags.ToListAsync();
            obj.requestId = requestId;
            if (name != null)
            {
                obj.patietName = name;
            }
            return obj;
        }

        public async Task<ViewModels.CaseActions> BlockCase(int requestId)
        {
            ViewModels.CaseActions obj = new ViewModels.CaseActions();
            var name = await _context.Requestclients.Where(a => a.Requestid == requestId).Select(a => a.Firstname).FirstOrDefaultAsync();
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

        public async Task<AgreementDetails> Agreement(int requestId)
        {
            AgreementDetails obj = new AgreementDetails();
            Data.Entity.Request? requestData = await _context.Requests.Include(a=>a.Requestclients).FirstOrDefaultAsync(a => a.Requestid == requestId);
            if (requestData != null)
            {
                obj.mobile = requestData.Requestclients.ElementAt(0).Phonenumber;
                obj.email = requestData.Requestclients.ElementAt(0).Email;
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
                        requestData.Status = 1;
                        requestData.Physicianid = physicianId;
                        _context.Requests.Update(requestData);
                    }

                    Requeststatuslog requeststatuslog = new Requeststatuslog
                    {
                        Requestid = requestId,
                        Status = 1,
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
            Data.Entity.Request? requestData = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);
            Requestclient? requestclient = await _context.Requestclients.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if( requestData != null )
            {
                requestData.Modifieddate = DateTime.Now;
                requestData.Status = 11;
                _context.Requests.Update(requestData);
            }

            Blockrequest blockrequest = new Blockrequest
            {
                Requestid = requestId,
                Phonenumber = requestclient!.Phonenumber!,
                Email = requestclient.Email!,
                Reason = blockNote,
                Isactive = new System.Collections.BitArray(new[] {false}),
                Createddate = DateTime.Now,
            };
            await _context.AddAsync(blockrequest);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitNotes(int requestId, string notes, CaseActionDetails obj, string aspNetUserId, string role)
        {
            Data.Entity.Request? requestData = await _context.Requests.FirstOrDefaultAsync(a => a.Requestid == requestId);
            Requestnote? requestnote = await _context.Requestnotes.FirstOrDefaultAsync(a => a.Requestid == requestId);
            if(requestnote == null)
            {
                Requestnote newRequestNote = new Requestnote();
                newRequestNote.Requestid = requestId;
                newRequestNote.Createddate = DateTime.Now;
                if(role == "Admin")
                {
                    newRequestNote.Adminnotes = notes;
                }
                else
                {
                    newRequestNote.Physiciannotes = notes;
                }
                newRequestNote.Createdby = aspNetUserId;
                await _context.Requestnotes.AddAsync(newRequestNote);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (role == "Admin")
                {
                    requestnote.Adminnotes = notes;
                }
                else
                {
                    requestnote.Physiciannotes = notes;
                }
                requestnote.Modifiedby = aspNetUserId;
                requestnote.Modifieddate = DateTime.Now;
                _context.Update(requestnote);
                await _context.SaveChangesAsync();
            }
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

        public async Task SendingAgreement(int requestId, string url)
        {
            Data.Entity.Request requestData = await _context.Requests.Include(a => a.Requestclients).FirstOrDefaultAsync(a => a.Requestid == requestId);
            string email = requestData.Requestclients.ElementAt(0).Email;
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
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public async Task<CloseCase> CloseCase(int requestId)
        {
            var requestData = await _context.Requestclients.Include(a => a.Request).FirstOrDefaultAsync(a => a.Requestid == requestId);
            CloseCase obj = new CloseCase();
            if(requestData != null)
            {
                obj.firstName = requestData.Firstname;
                obj.lastName = requestData.Lastname!;
                obj.email = requestData.Email!;
                obj.DOB = new DateTime(Convert.ToInt32(requestData.Intyear),
                    DateTime.ParseExact(requestData.Strmonth!, "MMM", CultureInfo.InvariantCulture).Month,
                    Convert.ToInt32(requestData.Intdate));
                obj.mobileNumber = requestData.Phonenumber!;
                obj.requestId = requestId;
            }
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
