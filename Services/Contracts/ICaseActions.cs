using Services.ViewModels;

namespace Services.Contracts
{
    public interface ICaseActions
    {
        CaseActionsDetails AssignCase(int requestId);

        CaseActionsDetails CancelCase(int requestId);

        void SubmitAssign(CaseActionsDetails obj);

        void SubmitCancel(int requestId, int caseId, string cancelNote);
    }
}