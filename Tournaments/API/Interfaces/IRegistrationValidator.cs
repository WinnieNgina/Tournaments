using API.DTO;
using API.Validators;

namespace API.Interfaces
{
    public interface IRegistrationValidator
    {
        Task<ValidationResult> ValidateAsync(CreatePlayerDto model);
    }
}
