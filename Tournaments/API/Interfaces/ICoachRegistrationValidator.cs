using API.DTO;
using API.Validators;

namespace API.Interfaces;

public interface ICoachRegistrationValidator
{
    Task<ValidationResult> ValidateAsync(SignUpCoachDTO model);
}
