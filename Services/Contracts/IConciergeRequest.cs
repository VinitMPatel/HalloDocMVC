using Services.ViewModels;

namespace Services.Contracts
{
    public interface IConciergeRequest
    {
        Task CnciergeInsert(ConciergeRequestData r);
    }
}