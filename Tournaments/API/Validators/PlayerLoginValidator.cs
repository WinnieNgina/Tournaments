using API.DTO;
using API.Interfaces;
using API.Services;
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
        var player = model.PhoneNumber != null ? await _playerService.GetPlayerByPhoneNumberAsync(model.PhoneNumber) : await _playerService.GetPlayerByUserNameAsync(model.UserName);

        if (player == null)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid Phone number or username.");
            return result;  // Early return
        }

        if (!player.PhoneNumberConfirmed)
        {
            result.IsValid = false;
            result.Errors.Add("Please confirm your phone number before logging in.");
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
