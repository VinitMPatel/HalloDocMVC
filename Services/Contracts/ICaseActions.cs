using Services.ViewModels;

namespace Services.Contracts
{
    public interface ICaseActions
    {
        AgreementDetails Agreement(int requestId);
        CaseActions AssignCase(int requestId);
        Task SubmitAssign(int requestId, int physicianId, string assignNote);
        CaseActions BlockCase(int requestId);
        CaseActions CancelCase(int requestId);

        CaseActions Orders(int requestId);

        void SubmitBlock(int requestId, string blockNote);

        void SubmitCancel(int requestId, int caseId, string cancelNote);
        void SubmitClearCase(int requestId);
        void SubmitNotes(int requestId, string notes, CaseActionDetails obj);

        void SubmitOrder(Orders obj);

        void SubmitTransfer(int requestId, int physicianId, string transferNote);

        public void SendingAgreement(int requestId, string email, string url);
        void AgreeAgreement(int requestId);
    
        CloseCase CloseCase(int requestId);
        void CloseCaseChanges(string email, int requestId, string phone);
        Task CancelAgreement(int requestId);
    }
}