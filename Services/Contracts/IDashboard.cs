using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboard
    {
        Task<patient_dashboard> PatientDashboard(string aspNetUserId);

        Task<String> editing(patient_dashboard r, int id);

        Task<patient_dashboard> ViewDocuments(string aspNetUserId, int userId);

        public void UplodingDocument(patient_dashboard obj, int reqId);

        public void uploadFile(List<IFormFile> upload, int id);

    }
}