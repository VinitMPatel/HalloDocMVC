using Data.DataContext;
using HalloDoc.ViewModels;
using HalloDoc.Views.Home;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace HalloDoc.Controllers
{
    public class RequestsController : Controller
    {
        private readonly HelloDocDbContext _context;
        private readonly IFamilyRequest familyRequest;
        private readonly IConciergeRequest conciergeRequest;
        private readonly IBusinessRequest businessRequest;

        public RequestsController(HelloDocDbContext context, IFamilyRequest familyRequest , IConciergeRequest conciergeRequest, IBusinessRequest businessRequest)
        {
            _context = context;
            this.familyRequest = familyRequest;
            this.conciergeRequest = conciergeRequest;
            this.businessRequest = businessRequest;
        }

        public IActionResult FamilyInsert(FamilyFriendRequest r)
        {
            familyRequest.FamilyInsert(r);
            return RedirectToAction("patient_login", "Home");
        }


        public IActionResult ConciergeInsert(ConciergeRequestData r)
        {
            conciergeRequest.CnciergeInsert(r);
            return RedirectToAction("patient_login", "Home");
        }

        public IActionResult BusinessInsert(BusinessRequestData r)
        {
            businessRequest.BusinessInsert(r);
            return RedirectToAction("patient_login", "Home");
        }
    }
}
