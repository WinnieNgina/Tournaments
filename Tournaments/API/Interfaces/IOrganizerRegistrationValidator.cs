using API.DTO;
using API.Validators;

namespace API.Interfaces
{
    public interface IOrganizerRegistrationValidator
    {
        Task<ValidationResult> ValidateAsync(SignUpOrganizerDTO model);
    }
}
