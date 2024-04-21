using Services.ViewModels;

namespace Services.Contracts
{
    public interface IBusinessRequest
    {
        Task BusinessInsert(BusinessRequestData r);
    }
}