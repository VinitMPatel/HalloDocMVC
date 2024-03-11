using Services.ViewModels;

namespace Services.Contracts
{
    public interface IBusinessRequest
    {
        void BusinessInsert(BusinessRequestData r);
    }
}