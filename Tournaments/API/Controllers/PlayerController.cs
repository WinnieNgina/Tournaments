using API.DTO;
using API.Interfaces;
using API.Services;
using API.Validators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IEmailService _emailService;
    private readonly IRegistrationValidator _registrationValidator;
    private readonly ILoginValidatorFactory _loginValidatorFactory;
    private readonly ISmsService _smsService;
    public PlayerController(IPlayerService playerService, IEmailService emailService, IRegistrationValidator registrationValidator, ILoginValidatorFactory loginValidatorFactory, ISmsService smsService)
    {
        _playerService = playerService;
        _emailService = emailService;
        _registrationValidator = registrationValidator;
        _loginValidatorFactory = loginValidatorFactory;
        _smsService = smsService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetAllPlayers()
    {
        var players = await _playerService.GetAllPlayersAsync();
        return Ok(players);
    }
    [HttpPost("Register")]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var validationResult = await _registrationValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        string password = model.Password;
        var playerId = await _playerService.CreatePlayerAsync(model, password);
        if (playerId != null)
        {
            var role = "Player";
            await _playerService.AddToRoleAsync(playerId, role);
            // Generate email confirmation token
            string code = await _playerService.GeneratePhoneNumberConfirmationTokenAsync(playerId, model.PhoneNumber);
            var message = $"Your confirmation code is: {code}";
            _smsService.SendSms(model.PhoneNumber, message);
            var confirmationResult = await SendEmailConfirmationAsync(playerId, model.Email);

            if (confirmationResult.Succeeded)
            {
                return Ok("User created successfully");
            }
            else
            {
                return BadRequest($"Failed to send confirmation email: {string.Join(", ", confirmationResult.Errors)}");
            }
        }
        return BadRequest(new { Message = "Failed to create user" });
    }
    private async Task<IdentityResult> SendEmailConfirmationAsync(string Id, string email)
    {
        var token = await _playerService.GenerateEmailConfirmationTokenAsync(Id);
        var callbackUrl = Url.Action("ConfirmEmail", "Player", new
        {
            playerId = Uri.EscapeDataString(Id),
            token = token // Do not use Uri.EscapeDataString for token here
        }, "https", Request.Host.Value);
        // Send confirmation email
        var subject = "Confirm your email";

        var message = $@"Please confirm your email by clicking the following link:
                {callbackUrl}

                If you're unable to click the link, please copy and paste it into your web browser.";
        var isHtml = false;

        return await _emailService.SendEmailAsync(email, subject, message, isHtml);
    }
    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string playerId, string token)
    {
        if (string.IsNullOrWhiteSpace(playerId) || string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("User ID and confirmation code are required.");
        }
        var decodedUserId = Uri.UnescapeDataString(playerId);
        var decodedToken = Uri.UnescapeDataString(token);
        // Find the user by their ID and
        // Verify the email confirmation token
        var result = await _playerService.ConfirmEmailAsync(decodedUserId, decodedToken);
        if (result.Succeeded)
        {
            // Email confirmed successfully
            return Ok("Email confirmed successfully");
        }
        else
        {
            // Email confirmation failed
            // Include specific error messages in the response
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest($"Failed to confirm email: {errorMessages}");
        }
    }
    [HttpPost("VerifyPhoneNumber")]
    public async Task<IActionResult> VerifyPhoneNumber(string name, string phoneNumber, string token)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            return BadRequest("User name, phone number and confirmation code are required.");
        }
        var result = await _playerService.ConfirmPhoneNumberAsync(name, phoneNumber, token);
        if (result.Succeeded)
        {
            return Ok("Phone number confirmed successfully");
        }
        else
        {
            // Phone number confirmation failed
            // Include specific error messages in the response
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest($"Failed to confirm email: {errorMessages}");
        }
    }
    [HttpGet("{playerId}")]
    public async Task<IActionResult> GetPlayer(string playerId)
    {
        var user = await _playerService.GetPlayerByIdAsync(playerId);
        if (user == null)
        {
            return NotFound(); // User not found
        }
        // Return a DTO or a model with the user's information
        return Ok(new { user.Id, user.UserName, user.Email });
    }
    [HttpGet("{userName}/GetUserByName")]
    public async Task<IActionResult> GetPlayerByName(string userName)
    {
        var user = await _playerService.GetPlayerByUserNameAsync(userName);
        if (user == null)
        {
            return NotFound(); // User not found
        }
        // Return a DTO or a model with the user's information
        return Ok(new { user.Id, user.UserName, user.Email });
    }
    [HttpGet("{email}/GetUserByEmail")]
    public async Task<IActionResult> GetPlayerByEmail(string email)
    {
        var user = await _playerService.GetPlayerByEmailAsync(email);
        if (user == null)
        {
            return NotFound(); // User not found
        }
        // Return a DTO or a model with the user's information
        return Ok(new { user.Id, user.UserName, user.Email });
    }
    [HttpGet("{phoneNumber}/GetOrganizerByPhoneNumber")]
    public async Task<IActionResult> GetOrganizerByPhoneNumber(string phoneNumber)
    {
        var user = await _playerService.GetPlayerByPhoneNumberAsync(phoneNumber);
        if (user == null)
        {
            return NotFound(); // User not found
        }
        // Return a DTO or a model with the user's information
        return Ok(new { user.Id, user.UserName, user.Email });
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var role = "Player";
        var validator = _loginValidatorFactory.CreateValidator(role);
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // Assuming _playerService.GetPlayerByEmailAsync and _playerService.GetPlayerByUserNameAsync
        // return the same type of player object, we can simplify the retrieval process.
        var player = model.PhoneNumber != null ? await _playerService.GetPlayerByPhoneNumberAsync(model.PhoneNumber) : await _playerService.GetPlayerByUserNameAsync(model.UserName);

        // At this point, we know the player exists and their email is confirmed,
        // and the password has been validated. Proceed with the login process.

        if (player.TwoFactorEnabled)
        {
            // Two-factor authentication is enabled
            var otpToken = await _playerService.GenerateTwoFactorTokenAsync(player.Id);
            var message = $"Your Tournament tracker verification code is: {otpToken}";
            _smsService.SendSms(player.PhoneNumber, message);

            // Optionally, you may return a message to inform the user
            return Ok("Please check your messages for the OTP code.");
        }

        // Two-factor authentication is not enabled, generate and return the authentication token
        var token = await _playerService.GenerateAuthTokenAsync(player.Id);
        return Ok(new { Token = token });
    }
    [HttpPost("{email}/Enable2FA")]
    public async Task<IActionResult> EnableTwoFactorAuthentication(string email)
    {
        var player = await _playerService.GetPlayerByEmailAsync(email);

        if (player == null)
        {
            return NotFound($"Player not found.");
        }

        await _playerService.EnableTwoFactorAuthenticationAsync(player.Id);

        return Ok("Two-factor authentication enabled successfully.");
    }
    [HttpPost("{email}/Disable2FA")]
    public async Task<IActionResult> DisableTwoFactorAuthentication(string email)
    {
        var player = await _playerService.GetPlayerByEmailAsync(email);

        if (player == null)
        {
            return NotFound($"Player not found.");
        }

        await _playerService.DisableTwoFactorAuthenticationAsync(player.Id);

        return Ok("Two-factor authentication disabled successfully.");
    }
    [HttpDelete("{email}")]
    public async Task<IActionResult> DeletePlayerAccount(string email)
    {
        var player = await _playerService.GetPlayerByEmailAsync(email);
        if (player == null)
        {
            return NotFound("Player not found."); // Player not found
        }
        var success = await _playerService.DeletePlayerAsync(player.Id);
        if (success)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok("Account deleted successfully");
        }
        return NotFound("Failed to delete player account");
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _playerService.LogoutAsync();
        return Ok("Logout successful.");
    }
    [HttpPost("{email}/ChangePassWord")]
    public async Task<IActionResult> ChangePassword(string email, ChangePasswordDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var player = await _playerService.GetPlayerByEmailAsync(email);
        if (player == null)
        {
            return NotFound($"User not found.");
        }
        var isCurrentPasswordValid = await _playerService.CheckCurrentPasswordAsync(player.Id, model.CurrentPassword);
        if (!isCurrentPasswordValid)
        {
            return BadRequest("The current password is incorrect.");
        }
        var result = await _playerService.ChangePasswordAsync(player.Id, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            await _playerService.SignInAsync(player.Id, isPersistent: false);
            return Ok("Password changed successfully.");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return BadRequest(ModelState);
    }
}
