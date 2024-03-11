
using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IFamilyRequest
    {
        bool FamilyInsert(FamilyFriendRequest r);

        void uploadFile(List<IFormFile> upload, int id);
    }
}