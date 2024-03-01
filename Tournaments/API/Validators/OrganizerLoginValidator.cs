using API.DTO;
using API.Interfaces;
using API.Services;

namespace API.Validators
{
    public class OrganizerLoginValidator : ILoginValidator
    {
        private readonly IOrganizerService _organizerService;
        public OrganizerLoginValidator(IOrganizerService organizerService)
        {
            _organizerService = organizerService;
        }
        public async Task<ValidationResult> ValidateAsync(LoginDTO model)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.UserName))
            {
                result.IsValid = false;
                result.Errors.Add("Either Email or UserName is required.");
                return result;  // Early return
            }

            if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.UserName))
            {
                result.IsValid = false;
                result.Errors.Add("Provide either Email or UserName, not both.");
                return result;  // Early return
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                result.IsValid = false;
                result.Errors.Add("Password is required.");
                return result;  // Early return
            }
            var organizer = model.Email != null ? await _organizerService.GetOrganizerByEmailAsync(model.Email) : await _organizerService.GetOrganizerByNameAsync(model.UserName);

            if (organizer == null)
            {
                result.IsValid = false;
                result.Errors.Add("Invalid email or username.");
                return result;  // Early return
            }

            if (!organizer.EmailConfirmed)
            {
                result.IsValid = false;
                result.Errors.Add("Please confirm your email before logging in.");
                return result;  // Early return
            }

            var passwordCheckResult = await _organizerService.CheckPasswordAsync(organizer.Id, model.Password);
            if (!passwordCheckResult)
            {
                result.IsValid = false;
                result.Errors.Add("Invalid password.");
                return result;  // Early return
            }

            return result;  // Return result if all checks pass
        }
    }
}
