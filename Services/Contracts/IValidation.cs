using Data.Entity;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IValidation
    {
        PatientLogin Validate(Aspnetuser user);
    }
}