using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;

namespace Services.Implementation
{
    public class ProviderSideServices : IProviderSideServices
    {
        private readonly HalloDocDbContext _context;

        public ProviderSideServices(HalloDocDbContext context)
        {
            _context = context;
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

            requestclients = await _context.Requestclients.Include(a => a.Request).Include(a => a.Request.Physician).Where(a => a.Request.Physician.Aspnetuserid == obj.aspNetUserId).ToListAsync();

            requestclients = requestclients.Where(a =>
                                 (obj.requeststatus == 0 || a.Request.Status == obj.requeststatus) &&
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
                        Transtophysicianid = requestData.Physicianid,
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
       
    }
}
