using Microsoft.AspNetCore.Http;
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

        Task<ConcludeCare> ConcludeCareView(int requestId);

        Task Consult(int requestId);

        Task<EncounterFormViewModel> EncounterForm(int requestId);

        Task FinalizeEncounter(int requestId);

        Task HouseCall(int requestId);

        Task HouseCalling(int requestId);
        Task<MonthWiseScheduling> Monthwise(DateTime currentDate, string aspNetUserId);
        Task<Scheduling> Scheduling();

        Task SubmitEncounterForm(EncounterFormViewModel model);

        Task SubmitTransferReqquest(int requestId, string note);

        Task uploadFiles(List<IFormFile> formFiles, int requestId);
    }
}
