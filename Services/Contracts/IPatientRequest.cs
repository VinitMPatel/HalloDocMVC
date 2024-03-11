using Data.Entity;
using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IPatientRequest
    {
        //IActionResult CheckEmail(string email);
        void Insert(PatientInfo r);
        void uploadFile(List<IFormFile> upload, int id);
        void NewAccount(Aspnetuser model);
    }
}