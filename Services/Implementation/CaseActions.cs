using Data.DataContext;
using Data.Entity;
using Microsoft.CodeAnalysis.Operations;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class CaseActions : ICaseActions
    {
        private readonly HalloDocDbContext _context;

        public CaseActions(HalloDocDbContext context)
        {
            _context = context;
        }

        public CaseActionsDetails AssignCase(int requestId)
        {
            CaseActionsDetails obj = new CaseActionsDetails();
            obj.regionList = _context.Regions.ToList();
            obj.requestId = requestId;
            return obj;
        }

        public CaseActionsDetails CancelCase(int requestId)
        {
            CaseActionsDetails obj = new CaseActionsDetails();
            var name = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault().Firstname;
            obj.cancelList = _context.Casetags.ToList();
            obj.requestId = requestId;
            obj.patietName = name;
            return obj;
        }

        public CaseActionsDetails BlockCase(int requestId)
        {
            CaseActionsDetails obj = new CaseActionsDetails();
            var name = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault().Firstname;
            obj.requestId = requestId;
            obj.patietName = name;
            return obj;
        }

        public CaseActionsDetails Orders(int requestId)
        {
            CaseActionsDetails obj = new CaseActionsDetails();
           
            return obj;
        }


        public void SubmitAssign(int requestId, int physicianId, string assignNote)
        {
            var requestData = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            requestData.Modifieddate = DateTime.Now;
            requestData.Status = 2;
            requestData.Physicianid = physicianId;
            _context.Requests.Update(requestData);

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = requestId,
                Status = 2,
                Physicianid = physicianId,
                Notes = assignNote,
                Createddate = DateTime.Now,
                Transtophysicianid = physicianId
            };
            _context.Add(requeststatuslog);
            _context.SaveChanges();
        }

        public void SubmitCancel(int requestId, int caseId, string cancelNote)
        {
            var requestData = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            requestData.Modifieddate = DateTime.Now;
            requestData.Casetag = _context.Casetags.FirstOrDefault(a=> a.Casetagid == caseId).Name;
            requestData.Status = 3;
            _context.Requests.Update(requestData);

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = requestId,
                Status = 3,
                Notes = cancelNote,
                Createddate = DateTime.Now
            };
            _context.Add(requeststatuslog);
            _context.SaveChanges();
        }

        public void SubmitBlock(int requestId, string blockNote)
        {
            var requestData = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            requestData.Modifieddate = DateTime.Now;
            requestData.Status = 11;
            _context.Requests.Update(requestData);

            Blockrequest blockrequest = new Blockrequest
            {
                Requestid = requestId,
                Phonenumber = requestData.Phonenumber,
                Email = requestData.Email,
                Reason = blockNote,
                Createddate= DateTime.Now,
            };
            _context.Add(blockrequest);
            _context.SaveChanges();
        }

        public void SubmitNotes(int requestId, string notes, CaseActionDetails obj)
        {
            var requestData = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            Requestnote requestnote = new Requestnote();
            var existRequestNote = _context.Requestnotes.FirstOrDefault(a => a.Requestid == requestId);
            requestnote.Requestid = requestId;
            requestnote.Createddate = DateTime.Now;
            requestnote.Adminnotes = notes;
            requestnote.Createdby = "1";
            _context.Requestnotes.Add(requestnote);
            _context.SaveChanges();
        }

        public void SubmitOrder(Orders obj)
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
            _context.Orderdetails.Add(orderdetail);
            _context.SaveChanges();
        }

        public void SubmitTransfer(int requestId, int physicianId, string transferNote)
        {
            var requestData = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            requestData.Modifieddate = DateTime.Now;
            requestData.Physicianid = physicianId;
            _context.Requests.Update(requestData);

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = requestId,
                Status = 2,
                Physicianid = physicianId,
                Notes = transferNote,
                Createddate = DateTime.Now,
                Transtophysicianid = physicianId
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();
        }
    }
}
