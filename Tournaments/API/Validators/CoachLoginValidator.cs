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

        var coach= model.Email != null ? await _coachService.GetCoachByEmailAsync(model.Email) : await _coachService.GetCoachByNameAsync(model.UserName);

        if (coach == null)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid email or username.");
            return result;  // Early return
        }

        if (!coach.EmailConfirmed)
        {
            result.IsValid = false;
            result.Errors.Add("Please confirm your email before logging in.");
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

