using HalloDoc.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Services.Contracts
{
    public interface IFamilyRequest
    {
        bool FamilyInsert(FamilyFriendRequest r);

        void uploadFile(List<IFormFile> upload, int id);
    }
}