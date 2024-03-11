using Services.ViewModels;

namespace Services.Contracts
{
    public interface ICaseActions
    {
        CaseActionsDetails AssignCase(int requestId);
        CaseActionsDetails BlockCase(int requestId);
        CaseActionsDetails CancelCase(int requestId);

        CaseActionsDetails Orders(int requestId);

        void SubmitAssign(int requestId, int physicianId, string assignNote);

        void SubmitBlock(int requestId, string blockNote);

        void SubmitCancel(int requestId, int caseId, string cancelNote);

        void SubmitNotes(int requestId, string notes, CaseActionDetails obj);

        void SubmitOrder(Orders obj);

        void SubmitTransfer(int requestId, int physicianId, string transferNote);
    }
}