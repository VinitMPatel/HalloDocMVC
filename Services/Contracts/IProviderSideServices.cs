using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IProviderSideServices
    {
        Task AcceptCase(int requestId);

        Task<ProviderDashboard> AllData(string aspNetUserId);

        Task<ProviderDashboard> AllStateData(ProviderDashboard obj);

        Task ConcludeCareCase(int requestId , string note);

        Task<ConcludeCare> ConcludeCareView(int requestId);

        Task Consult(int requestId);
        Task<IActionResult> DownloadEncounter(int requestId);

        Task<EncounterFormViewModel> EncounterForm(int requestId);

        Task FinalizeEncounter(int requestId);

        Task HouseCall(int requestId);

        Task HouseCalling(int requestId);

        Task<InvoicingViewModel> InvocingData(string aspNetUserId , string startDate);

        Task<MonthWiseScheduling> Monthwise(DateTime currentDate, string aspNetUserId);

        Task<Scheduling> Scheduling();

        Task SubmitEncounterForm(EncounterFormViewModel model);

        Task SubmitTimeSheet(InvoicingViewModel obj , string aspNetUserId);

        Task SubmitTransferReqquest(int requestId, string note);

        Task uploadFiles(List<IFormFile> formFiles, int requestId);
    }
}
