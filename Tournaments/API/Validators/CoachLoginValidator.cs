using API.DTO;
using API.Interfaces;

namespace API.Validators;

public class CoachLoginValidator : ILoginValidator
{
    private readonly ICoachService _coachService;

    public CoachLoginValidator(ICoachService coachService)
    {
        _coachService = coachService;
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

        var coach= model.PhoneNumber != null ? await _coachService.GetCoachByPhoneNumberAsync(model.PhoneNumber) : await _coachService.GetCoachByNameAsync(model.UserName);

        if (coach == null)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid Phone number or username.");
            return result;  // Early return
        }

        if (!coach.PhoneNumberConfirmed)
        {
            result.IsValid = false;
            result.Errors.Add("Please confirm your phone number before logging in.");
            return result;  // Early return
        }

        var passwordCheckResult = await _coachService.CheckPasswordAsync(coach.Id, model.Password);
        if (!passwordCheckResult)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid password.");
            return result;  // Early return
        }

        return result;  // Return result if all checks pass
    }
}

