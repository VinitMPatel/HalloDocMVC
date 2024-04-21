using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboard
    {
        Task<patient_dashboard> PatientDashboard(string aspNetUserId);

        Task<string> EditProfile(patient_dashboard r, string aspNetUserId);

        Task<patient_dashboard> ViewDocuments(string aspNetUserId, int userId);

        public void UplodingDocument(patient_dashboard obj, int reqId);

        public void uploadFile(List<IFormFile> upload, int id);
        Task<PatientInfo> RequestForSelfData(string aspNetUserId);
        Task<FamilyFriendRequest> RequestForElseData(string aspNetUserId);
        Task<(string , string)> FindUser(LoginPerson model);
        Task UpdatePassword(LoginPerson model);
    }
}