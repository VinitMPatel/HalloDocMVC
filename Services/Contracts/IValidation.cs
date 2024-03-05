using Data.Entity;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IValidation
    {
        PatientLogin AdminValidate(Aspnetuser user);
        PatientLogin Validate(Aspnetuser user);
    }
}