using Services.ViewModels;

namespace Services.Contracts
{
    public interface ICaseActions
    {
        AgreementDetails Agreement(int requestId);

        Task<CaseActions> AssignCase(int requestId);

        Task SubmitAssign(int requestId, int physicianId, string assignNote);

        Task<CaseActions> BlockCase(int requestId);

        Task<CaseActions> CancelCase(int requestId);

        CaseActions Orders(int requestId);

        Task SubmitBlock(int requestId, string blockNote);

        Task SubmitCancel(int requestId, int caseId, string cancelNote);

        Task SubmitClearCase(int requestId);

        Task SubmitNotes(int requestId, string notes, CaseActionDetails obj);

        Task SubmitOrder(Orders obj);

        Task SubmitTransfer(int requestId, int physicianId, string transferNote);

        Task SendingAgreement(int requestId, string url);

        Task AgreeAgreement(int requestId);
    
        Task<CloseCase> CloseCase(int requestId);

        Task CloseCaseChanges(string email, int requestId, string phone);

        Task CancelAgreement(int requestId);
    }
}