using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboard
    {
        Task<patient_dashboard> PatientDashboard(int id);

        Task<String> editing(patient_dashboard r, int id);

        Task<patient_dashboard> ViewDocuments(int reqId, int userId);

        public void UplodingDocument(patient_dashboard obj, int reqId);

        public void uploadFile(List<IFormFile> upload, int id);

    }
}