
using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IFamilyRequest
    {
        Task FamilyInsert(FamilyFriendRequest r);

        Task uploadFile(List<IFormFile> upload, int id);
    }
}