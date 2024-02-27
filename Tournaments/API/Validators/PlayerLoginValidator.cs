using API.DTO;
using API.Interfaces;
using static System.Net.WebRequestMethods;

namespace API.Validators;

public class PlayerLoginValidator : ILoginValidator
{
    private readonly IPlayerService _playerService;

    public PlayerLoginValidator(IPlayerService playerService)
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

        var player = model.Email != null ? await _playerService.GetPlayerByEmailAsync(model.Email) : await _playerService.GetPlayerByUserNameAsync(model.UserName);

        if (player == null)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid email or username.");
            return result;  // Early return
        }

        if (!player.EmailConfirmed)
        {
            result.IsValid = false;
            result.Errors.Add("Please confirm your email before logging in.");
            return result;  // Early return
        }

        var passwordCheckResult = await _playerService.CheckPasswordAsync(player.Id, model.Password);
        if (!passwordCheckResult)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid password.");
            return result;  // Early return
        }

        return result;  // Return result if all checks pass
    }
}
