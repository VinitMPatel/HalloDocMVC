using HalloDoc.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Services.Contracts
{
    public interface IPatientRequest
    {
        //IActionResult CheckEmail(string email);
        void Insert(PatientInfo r);
        void uploadFile(List<IFormFile> upload, int id);
    }
}