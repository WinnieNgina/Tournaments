using API.Validators;

namespace API.DTO
{
    public interface ILoginValidator
    {
        Task<ValidationResult> ValidateAsync(LoginDTO model);
    }
}
