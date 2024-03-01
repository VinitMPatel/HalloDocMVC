using Data.DataContext;
using Data.Entity;
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
        private readonly HelloDocDbContext _context;

        public CaseActions(HelloDocDbContext context)
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

        public void SubmitAssign(CaseActionsDetails obj)
        {
            var requestData = _context.Requests.Where(a => a.Requestid == obj.requestId).FirstOrDefault();
            requestData.Modifieddate = DateTime.Now;
            requestData.Status = 2;
            _context.Requests.Update(requestData);

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = obj.requestId,
                Status = 2,
                Physicianid = obj.physicianId,
                Notes = obj.assignNotes,
                Createddate = DateTime.Now,
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
    }
}
