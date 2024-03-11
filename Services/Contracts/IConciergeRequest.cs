using Services.ViewModels;

namespace Services.Contracts
{
    public interface IConciergeRequest
    {
        void CnciergeInsert(ConciergeRequestData r);
    }
}