using Data.DataContext;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication.PgOutput.Messages;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDashboardData dashboardData;
        private readonly ICaseActions caseActions;
        private readonly HelloDocDbContext _context;
        public AdminController(IDashboardData dashboardData , HelloDocDbContext context , ICaseActions caseActions) {
            this.dashboardData = dashboardData;
            _context = context;
            this.caseActions = caseActions;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard() 
        {
            AdminDashboard obj = dashboardData.AllData();
            return View(obj);
        }

        public IActionResult ActiveState()
        {
            AdminDashboard data = dashboardData.ActiveStateData();
            return View(data);
        }

        public IActionResult ConcludeState()
        {
            AdminDashboard data = dashboardData.ConcludeStateData();
            return View(data);
        }

        public IActionResult NewState(String status , String requesttype)
        {
                AdminDashboard data = dashboardData.NewStateData(status,requesttype);
                return View(data);
        }

        public IActionResult PendingState()
        {
            AdminDashboard data = dashboardData.PendingStateData();
            return View(data);
        }

        public IActionResult ToCloseState()
        {
            AdminDashboard data = dashboardData.ToCloseStateData();
            return View(data);
        }

        public IActionResult UnpaidState()
        {
            AdminDashboard data = dashboardData.UnpaidStateData();
            return View(data);
        }

        public IActionResult ViewCase(int requestId)
        {
            CaseDetails obj = dashboardData.ViewCaseData(requestId);
            return PartialView("_ViewCase" , obj);
        }

        public IActionResult ViewNotes(int requestId)
        {
            var requestData = _context.Requests.FirstOrDefault(a=>a.Requestid == requestId);
            var requestnote = _context.Requestnotes.FirstOrDefault(a=> a.Requestid == requestId);
            CaseDetails obj = new CaseDetails();
            if (requestnote != null)
            {
                obj.adminNote = requestnote.Adminnotes;
            }

            var admin = "Vinit";
            obj.requestId = requestId;
            var transferNote = _context.Requeststatuslogs.FirstOrDefault(a => a.Requestid == obj.requestId && a.Status == 2);
            if (transferNote != null)
            {
                var physicianName = _context.Physicians.FirstOrDefault(a=>a.Physicianid == transferNote.Physicianid).Firstname;
                obj.adminName = admin;
                obj.physicianName = physicianName;
                obj.assignTime = transferNote.Createddate;
            }

            //bol jaldi bol ungh aave he

            //CaseDetails obj = dashboardData.ViewCaseData(requestId);
            return PartialView("_ViewNotes",obj);
        }

        public List<Physician> FilterData(int regionid)
        {
            List<Physician> physicianList= dashboardData.PhysicianList(regionid);
            return physicianList;
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult AssignCase(int requestId)
        {

            CaseActionsDetails obj =  caseActions.AssignCase(requestId);
            return PartialView("_AssignCase",obj);
        }

        public IActionResult CancelCase(int requestId)
        {
            CaseActionsDetails obj = caseActions.CancelCase(requestId);
            return PartialView("_CancelCase", obj);
        }

        public IActionResult SubmitAssign(CaseActionsDetails obj)
        {
            caseActions.SubmitAssign(obj);
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult SubmitCancel(int requestId , int caseId , string cancelNote)
        {
            
            caseActions.SubmitCancel(requestId , caseId , cancelNote);
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult BlockCase(int requestId)
        {
            CaseActionsDetails obj = caseActions.BlockCase(requestId);
            return PartialView("_BlockCase", obj);
        }

        public IActionResult SubmitBlock(int requestId, string blockNote)
        {

            caseActions.SubmitBlock(requestId, blockNote);
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult SubmitNotes(int requestId, string notes , CaseDetails obj)
        {
            caseActions.SubmitNotes(requestId, notes , obj);
            return RedirectToAction("AdminDashboard");
        }

    }
}
