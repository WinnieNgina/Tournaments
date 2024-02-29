using API.DTO;
using API.Interfaces;
using API.Services;
using API.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;
using Org.BouncyCastle.Crypto.Macs;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrganizerController : ControllerBase
{
    private readonly IOrganizerService _organizerService;
    private readonly IEmailService _emailService;
    private readonly IOrganizerRegistrationValidator _organizerRegistrationValidator;
    private readonly IEmailService emailService;
    private readonly ISmsService _smsService;
    public OrganizerController(IOrganizerService organizerService, IEmailService emailservice, IOrganizerRegistrationValidator organizerRegistrationValidator, IEmailService emailServive, ISmsService smsService)
    {
        _organizerService = organizerService;
        _emailService = emailservice;
        _organizerRegistrationValidator = organizerRegistrationValidator;
        _emailService = emailServive;
        _smsService = smsService;


    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrganizerDTO>>> GetAllOrganizer()
    {
        var organizers = await _organizerService.GetAllOrganizersAsync();
        return Ok(organizers);
    }
    [HttpPost("Register")]
    public async Task<IActionResult> CreateOrganizer([FromBody] SignUpOrganizerDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var validationResult = await _organizerRegistrationValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        // Convert PlayerModelDto to PlayerModel
        var organizer = new TournamentOrganizerModel
        {

            FirstName = model.FirstName,
            LastName = model.LastName,
            AreaOfResidence = model.AreaOfResidence,
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            OrganizationName = model.OrganizationName,
        };
        string password = model.Password;
        var Id = await _organizerService.CreateOrganizerAsync(organizer, password);
        if (Id != null)
        {
            string code = await _organizerService.GeneratePhoneNumberConfirmationTokenAsync(Id, model.PhoneNumber);
            var message = $"Your confirmation code is: {code}";
            _smsService.SendSms(organizer.PhoneNumber, message);
            var confirmationResult = await SendEmailConfirmationAsync(Id, organizer.Email);

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
        var token = await _organizerService.GenerateEmailConfirmationTokenAsync(Id);
        var callbackUrl = Url.Action("ConfirmEmail", "Organizer", new
        {
            playerId = Uri.EscapeDataString(Id),
            token = token
        }, "https", Request.Host.Value);

        var subject = "Confirm your email";
        var message = $@"Please confirm your email by clicking the following link:
                <a href=""{callbackUrl}"">Confirm Email</a>
                <br><br>
                If you're unable to click the link, please copy and paste it into your web browser.";
        var isHtml = true;

        return await _emailService.SendEmailAsync(email, subject, message, isHtml);
    }
    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string Id, string token)
    {
        if (string.IsNullOrWhiteSpace(Id) || string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("User ID and confirmation code are required.");
        }
        var decodedUserId = Uri.UnescapeDataString(Id);
        var decodedToken = Uri.UnescapeDataString(token);
        // Find the user by their ID and
        // Verify the email confirmation token
        var result = await _organizerService.ConfirmEmailAsync(decodedUserId, decodedToken);
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
        var result = await _organizerService.ConfirmPhoneNumberAsync(name, phoneNumber, token);
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

}
