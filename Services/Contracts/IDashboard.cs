using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboard
    {
        patient_dashboard PatientDashboard(int id);

        public String editing(patient_dashboard r, int id);

        public patient_dashboard ViewDocuments(int reqId, int userId);

        public void UplodingDocument(patient_dashboard obj, int reqId);

        public void uploadFile(List<IFormFile> upload, int id);

    }
}