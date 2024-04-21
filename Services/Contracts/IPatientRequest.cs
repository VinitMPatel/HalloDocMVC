using Data.Entity;
using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IPatientRequest
    {
        //IActionResult CheckEmail(string email);
        Task Insert(PatientInfo r);

        Task uploadFile(List<IFormFile> upload, int id);

        Task NewAccount(LoginPerson model);

        Task<User> CheckEmail(string Email);

    }
}