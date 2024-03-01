using API.DTO;
using API.Interfaces;
namespace API.Validators;

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

        if (string.IsNullOrEmpty(model.PhoneNumber) && string.IsNullOrEmpty(model.UserName))
        {
            result.IsValid = false;
            result.Errors.Add("Either Phone number or UserName is required.");
            return result;  // Early return
        }

        if (!string.IsNullOrEmpty(model.PhoneNumber) && !string.IsNullOrEmpty(model.UserName))
        {
            result.IsValid = false;
            result.Errors.Add("Provide either Phone number or UserName, not both.");
            return result;  // Early return
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            result.IsValid = false;
            result.Errors.Add("Password is required.");
            return result;  // Early return
        }

        var organizer = model.PhoneNumber != null ? await _organizerService.GetOrganizerByPhoneNumberAsync(model.PhoneNumber) : await _organizerService.GetOrganizerByNameAsync(model.UserName);

        if (organizer == null)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid Phone number or username.");
            return result;  // Early return
        }

        if (!organizer.PhoneNumberConfirmed)
        {
            result.IsValid = false;
            result.Errors.Add("Please confirm your phone number before logging in.");
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
