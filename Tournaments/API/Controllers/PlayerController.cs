using API.DTO;
using API.Interfaces;
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
}
