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
        Task<EncounterFormViewModel> EncounterForm(int requestId);
        Task SubmitEncounterForm(EncounterFormViewModel model);
        Task SubmitTransferReqquest(int requestId, string note);
    }
}
