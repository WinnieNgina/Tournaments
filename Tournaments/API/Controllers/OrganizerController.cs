using API.DTO;
using API.Interfaces;
using API.Services;
using API.Validators;
using Microsoft.AspNetCore.Authentication;
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
    private readonly ISmsService _smsService;
    private readonly ILoginValidatorFactory _loginValidatorFactory;
    public OrganizerController(IOrganizerService organizerService, IEmailService emailservice, IOrganizerRegistrationValidator organizerRegistrationValidator, ISmsService smsService, ILoginValidatorFactory loginValidatorFactory)
    {
        _organizerService = organizerService;
        _emailService = emailservice;
        _organizerRegistrationValidator = organizerRegistrationValidator;
        _smsService = smsService;
        _loginValidatorFactory = loginValidatorFactory;
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
            var role = "Organizer";
            await _organizerService.AddToRoleAsync(Id, role);
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
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetOrganizer(string Id)
    {
        var user = await _organizerService.GetOrganizerByIdAsync(Id);
        if (user == null)
        {
            return NotFound(); // User not found
        }
        // Return a DTO or a model with the user's information
        return Ok(new { user.Id, user.UserName, user.Email });
    }
    [HttpGet("{name}/GetOrganizerByName")]
    public async Task<IActionResult> GetOrganizerByName(string name)
    {
        var user = await _organizerService.GetOrganizerByNameAsync(name);
        if (user == null)
        {
            return NotFound(); // User not found
        }
        // Return a DTO or a model with the user's information
        return Ok(new { user.Id, user.UserName, user.Email });
    }
    [HttpGet("{email}/GetOrganizerByEmail")]
    public async Task<IActionResult> GetOrganizerByEmail(string email)
    {
        var user = await _organizerService.GetOrganizerByEmailAsync(email);
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
        var user = await _organizerService.GetOrganizerByPhoneNumberAsync(phoneNumber);
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
        var role = "Organizer";
        var validator = _loginValidatorFactory.CreateValidator(role);

        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var organizer = model.Email != null ? await _organizerService.GetOrganizerByEmailAsync(model.Email) : await _organizerService.GetOrganizerByNameAsync(model.UserName);

        // At this point, we know the player exists and their email is confirmed,
        // and the password has been validated. Proceed with the login process.

        if (organizer.TwoFactorEnabled)
        {
            // Two-factor authentication is enabled
            var otpToken = await _organizerService.GenerateTwoFactorTokenAsync(organizer.Id);
            var message = $"Your Tournament tracker verification code is: {otpToken}";

            _smsService.SendSms(organizer.PhoneNumber, message);

            // Optionally, you may return a message to inform the user
            return Ok("Please check your email for the OTP code.");
        }

        // Two-factor authentication is not enabled, generate and return the authentication token
        var token = await _organizerService.GenerateAuthTokenAsync(organizer.Id);
        return Ok(new { Token = token });
    }
    [HttpPost("{email}/Enable2FA")]
    public async Task<IActionResult> EnableTwoFactorAuthentication(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }
        var organizer = await _organizerService.GetOrganizerByEmailAsync(email);

        if (organizer == null)
        {
            return NotFound($"Coach not found.");
        }
        await _organizerService.EnableTwoFactorAuthenticationAsync(organizer.Id);

        return Ok("Two-factor authentication enabled successfully.");
    }
    [HttpPost("{email}/Disable2FA")]
    public async Task<IActionResult> DisableTwoFactorAuthentication(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }
        var organizer = await _organizerService.GetOrganizerByEmailAsync(email);

        if (organizer == null)
        {
            return NotFound($"Player not found.");
        }

        await _organizerService.DisableTwoFactorAuthenticationAsync(organizer.Id);

        return Ok("Two-factor authentication disabled successfully.");
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _organizerService.LogoutAsync();
        return Ok("Logout successful.");
    }
    [HttpDelete("{email}")]
    public async Task<IActionResult> DeleteCoachAccount(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }
        var organizer = await _organizerService.GetOrganizerByEmailAsync(email);
        if (organizer == null)
        {
            return NotFound("Organizer not found."); // Organizer not found
        }
        var success = await _organizerService.DeleteOrganizerAsync(organizer.Id);
        if (success)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok("Account deleted successfully");
        }
        return NotFound("Failed to delete organizer account.");
    }
    [HttpPost("{email}/ChangePassWord")]
    public async Task<IActionResult> ChangePassword(string email, ChangePasswordDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var organizer = await _organizerService.GetOrganizerByEmailAsync(email);
        if (organizer == null)
        {
            return NotFound($"Organizer not found.");
        }
        var isCurrentPasswordValid = await _organizerService.CheckCurrentPasswordAsync(organizer.Id, model.CurrentPassword);
        if (!isCurrentPasswordValid)
        {
            return BadRequest("The current password is incorrect.");
        }
        var result = await _organizerService.ChangePasswordAsync(organizer.Id, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            await _organizerService.SignInAsync(organizer.Id, isPersistent: false);
            return Ok("Password changed successfully.");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return BadRequest(ModelState);
    }
}
