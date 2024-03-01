using Services.ViewModels;

namespace Services.Contracts
{
    public interface ICaseActions
    {
        CaseActionsDetails AssignCase(int requestId);
        CaseActionsDetails BlockCase(int requestId);
        CaseActionsDetails CancelCase(int requestId);

        void SubmitAssign(CaseActionsDetails obj);
        void SubmitBlock(int requestId, string blockNote);
        void SubmitCancel(int requestId, int caseId, string cancelNote);
    }
}