using API.DTO;
using API.Interfaces;

namespace API.Validators;

public class LoginValidator : ILoginValidator
{
    private readonly IPlayerService _playerService;

    public LoginValidator(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task<ValidationResult> ValidateAsync(LoginDTO model)
    {
        var result = new ValidationResult { IsValid = true };

        if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.UserName))
        {
            result.IsValid = false;
            result.Errors.Add("Either Email or UserName is required.");
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            result.IsValid = false;
            result.Errors.Add("Password is required.");
        }

        var player = await _playerService.GetPlayerByEmailAsync(model.Email) ??
                     await _playerService.GetPlayerByUserNameAsync(model.UserName);

        if (player == null)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid email or username.");
        }
        else if (!player.EmailConfirmed)
        {
            result.IsValid = false;
            result.Errors.Add("Please confirm your email before logging in.");
        }
        else
        {
            var passwordCheckResult = await _playerService.CheckPasswordAsync(player.Id, model.Password);
            if (!passwordCheckResult)
            {
                result.IsValid = false;
                result.Errors.Add("Invalid password.");
            }
        }

        return result;
    }
}
