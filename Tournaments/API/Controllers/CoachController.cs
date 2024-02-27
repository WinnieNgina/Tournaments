using API.DTO;
using API.Interfaces;
using API.Services;
using API.Validators;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoachController : ControllerBase
{
    private readonly ICoachService _coachService;
    private readonly IEmailService _emailService;
    private readonly ICoachRegistrationValidator _coachRegistrationValidator;

    public CoachController(ICoachService coachService, IEmailService emailService, ICoachRegistrationValidator coachRegistrationValidator)
    {
        _coachService = coachService;
        _emailService = emailService;
        _coachRegistrationValidator = coachRegistrationValidator;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CoachDTO>>> GetAllCoaches()
    {
        var coaches = await _coachService.GetAllCoachesAsync();
        return Ok(coaches);
    }
    [HttpPost("Register")]
    public async Task<IActionResult> CreatePlayer([FromBody] SignUpCoachDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var validationResult = await _coachRegistrationValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        // Convert PlayerModelDto to PlayerModel
        var coach = new CoachModel
        {
           
            FirstName = model.FirstName,
            LastName = model.LastName,
            AreaOfResidence = model.AreaOfResidence,
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            SocialMediaUrl = model.SocialMediaUrl,
            CoachingSpecialization = model.CoachingSpecialization,
            Achievements = model.Achievements,
            YearsOfExperience = model.YearsOfExperience,

        };
        string password = model.Password;
        var Id = await _coachService.CreateCoachAsync(coach, password);
        if (Id != null)
        {
            var role = "Coach";
            await _coachService.AddToRoleAsync(Id, role);
            // Generate email confirmation token
            var token = await _coachService.GenerateEmailConfirmationTokenAsync(Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Coach", new
            {
                playerId = Uri.EscapeDataString(Id),
                token = token // Do not use Uri.EscapeDataString for token here
            }, "https", Request.Host.Value);
            // Send confirmation email
            var subject = "Confirm your email";

            var message = $@"Please confirm your email by clicking the following link:
                {callbackUrl}

                If you're unable to click the link, please copy and paste it into your web browser.";


            await _emailService.SendEmailAsync(coach.Email, subject, message);

            return Ok("User created successfully");
        }
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
        var result = await _coachService.ConfirmEmailAsync(decodedUserId, decodedToken);
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

}
