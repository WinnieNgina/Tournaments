using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IEmailService _emailService;
    public PlayerController(IPlayerService playerService, IEmailService emailService)
    {
        _playerService = playerService;
        _emailService = emailService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerModelDto>>> GetAllPlayers()
    {
        var players = await _playerService.GetAllPlayersAsync();
        return Ok(players);
    }
    [HttpPost("Register")]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerDto model)
    {
        // Convert PlayerModelDto to PlayerModel
        var player = new PlayerModel
        {
            DateOfBirth = model.DateOfBirth,
            FirstName = model.FirstName,
            LastName = model.LastName,
            AreaOfResidence = model.AreaOfResidence,
            UserName = model.UserName,
            Email = model.Email,
            Status = model.Status
        };
        string password = model.Password;
        var playerId = await _playerService.CreatePlayerAsync(player, password);
        if (playerId != null)
        {
            // Generate email confirmation token
            var token = await _playerService.GenerateEmailConfirmationTokenAsync(playerId);
            Console.WriteLine($"Player ID: {player.Id}");
            Console.WriteLine($"Token: {token}");
            var callbackUrl = Url.Action("ConfirmEmail", "Player", new
            {
                playerId = Uri.EscapeDataString(playerId),
                token = token // Do not use Uri.EscapeDataString for token here
            }, "https", Request.Host.Value);
            Console.WriteLine($"Callback URL: {callbackUrl}");

            // Send confirmation email
            var subject = "Confirm your email";

            var message = $@"Please confirm your email by clicking the following link:
                {callbackUrl}

                If you're unable to click the link, please copy and paste it into your web browser.";


            await _emailService.SendEmailAsync(player.Email, subject, message);

            return Ok("User created successfully");
        }

        // Include error details in the response
        return BadRequest(new { Message = "Failed to create user" });
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

        // Log the result of the email confirmation
        Console.WriteLine($"Email confirmation result: {result.Succeeded}");
        if (!result.Succeeded)
        {
            Console.WriteLine($"Email confirmation errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

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
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var player = await _playerService.GetPlayerByEmailAsync(model.Email);

        if (player == null)
        {
            // User not found
            return BadRequest("Invalid email");
        }

        if (!player.EmailConfirmed)
        {
            // User's email is not confirmed
            return BadRequest("Please confirm your email before logging in");
        }

        var result = await _playerService.CheckPasswordAsync(player.Id, model.Password);

        if (result)
        {
            // Password is correct
            if (player.TwoFactorEnabled)
            {
                // Two-factor authentication is enabled
                var otpToken = await _playerService.GenerateTwoFactorTokenAsync(player.Id);
                var emailSubject = "Your Login OTP Code";
                var emailMessage = $"Your OTP code is: {otpToken}";

                await _emailService.SendEmailAsync(player.Email, emailSubject, emailMessage);

                // Optionally, you may return a message to inform the user
                return Ok("Please check your email for the OTP code.");
            }

            // 2FA is not enabled, generate and return the authentication token
            var token = _playerService.GenerateAuthTokenAsync(player.Id);
            return Ok(new { Token = token });
        }

        // Invalid password
        return BadRequest("Invalid password");
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
            return NotFound(); // Player not found
        }
        var success = await _playerService.DeletePlayerAsync(player.Id);
        if (success)
        {
            return Ok("Account deleted successfully");
        }
        return NotFound("Player not found or failed to delete player");
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
