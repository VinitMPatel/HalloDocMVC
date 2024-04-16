using Data.Entity;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IValidation
    {
        (PatientLogin, LoggedInPersonViewModel) AdminValidate(LoginPerson user);

        PatientLogin Validate(LoginPerson user);
    }
}