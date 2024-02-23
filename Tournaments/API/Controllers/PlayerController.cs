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
    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerModelDto>>> GetAllPlayers()
    {
        var players = await _playerService.GetAllPlayersAsync();
        return Ok(players);
    }
    [HttpPost]
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
        var result = await _playerService.CreatePlayerAsync(player, password);
        if (result)
        {
            var token = await _playerService.GenerateEmailConfirmationTokenAsync(player);
            var callbackUrl = Url.Action("ConfirmEmail", "User", new
            {
                userId = Uri.EscapeDataString(player.Id),
                token = token // Do not use Uri.EscapeDataString for token here
            }, "https", Request.Host.Value);
            // Send confirmation email
            var subject = "Confirm your email";

            var message = $@"Please confirm your email by clicking the following link:
                {callbackUrl}

                If you're unable to click the link, please copy and paste it into your web browser.";
            // this part awaits implementation of email service

            // await _emailService.SendEmailAsync(player.Email, subject, message);

            return Ok("User created successfully and account confirmation email sent to the account");
        }
        // Include error details in the response
        return BadRequest(new { Message = "Failed to create user"});
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
