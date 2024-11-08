﻿using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoachController : ControllerBase
{
    private readonly ICoachService _coachService;
    private readonly IEmailService _emailService;
    private readonly ICoachRegistrationValidator _coachRegistrationValidator;
    private readonly ILoginValidatorFactory _loginValidatorFactory;
    private readonly ISmsService _smsService;

    public CoachController(ICoachService coachService, IEmailService emailService, ICoachRegistrationValidator coachRegistrationValidator, ILoginValidatorFactory loginValidatorFactory, ISmsService smsService)
    {
        _coachService = coachService;
        _emailService = emailService;
        _coachRegistrationValidator = coachRegistrationValidator;
        _loginValidatorFactory = loginValidatorFactory;
        _smsService = smsService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CoachDTO>>> GetAllCoaches()
    {
        var coaches = await _coachService.GetAllCoachesAsync();
        return Ok(coaches);
    }
    [HttpPost("Register")]
    public async Task<IActionResult> CreateCoach([FromBody] SignUpCoachDTO model)
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
        string password = model.Password;
        var Id = await _coachService.CreateCoachAsync(model, password);
        if (Id != null)
        {
            var role = "Coach";
            await _coachService.AddToRoleAsync(Id, role);
            // Generate email confirmation token
            string code = await _coachService.GeneratePhoneNumberConfirmationTokenAsync(Id, model.PhoneNumber);
            var message = $"Your confirmation code is: {code}";
            _smsService.SendSms(model.PhoneNumber, message);
            var confirmationResult = await SendEmailConfirmationAsync(Id, model.Email);

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
        var token = await _coachService.GenerateEmailConfirmationTokenAsync(Id);
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
    [HttpPost("VerifyPhoneNumber")]
    public async Task<IActionResult> VerifyPhoneNumber(string name, string phoneNumber, string token)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            return BadRequest("User name, phone number and confirmation code are required.");
        }
        var result = await _coachService.ConfirmPhoneNumberAsync(name, phoneNumber, token);
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

    [HttpGet("{coachId}")]
    public async Task<IActionResult> GetCoach(string coachId)
    {
        var user = await _coachService.GetCoachByIdAsync(coachId);
        if (user == null)
        {
            return NotFound(); // User not found
        }
        // Return a DTO or a model with the user's information
        return Ok(new { user.Id, user.UserName, user.Email });
    }
    [HttpGet("{name}/GetCoachByName")]
    public async Task<IActionResult> GetCoachByName(string name)
    {
        var user = await _coachService.GetCoachByNameAsync(name);
        if (user == null)
        {
            return NotFound(); // User not found
        }
        // Return a DTO or a model with the user's information
        return Ok(new { user.Id, user.UserName, user.Email });
    }
    [HttpGet("{email}/GetCoachByEmail")]
    public async Task<IActionResult> GetCoachByEmail(string email)
    {
        var user = await _coachService.GetCoachByEmailAsync(email);
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
        var user = await _coachService.GetCoachByPhoneNumberAsync(phoneNumber);
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
        var role = "Coach";
        var validator = _loginValidatorFactory.CreateValidator(role);

        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var coach = model.PhoneNumber != null ? await _coachService.GetCoachByPhoneNumberAsync(model.PhoneNumber) : await _coachService.GetCoachByNameAsync(model.UserName);

        // At this point, we know the player exists and their email is confirmed,
        // and the password has been validated. Proceed with the login process.

        if (coach.TwoFactorEnabled)
        {
            // Two-factor authentication is enabled
            var otpToken = await _coachService.GenerateTwoFactorTokenAsync(coach.Id);
            var message = $"Your Tournament tracker verification code is: {otpToken}";
            _smsService.SendSms(coach.PhoneNumber, message);

            // Optionally, you may return a message to inform the user
            return Ok("Please check your email for the OTP code.");
        }

        // Two-factor authentication is not enabled, generate and return the authentication token
        var token = await _coachService.GenerateAuthTokenAsync(coach.Id);
        return Ok(new { Token = token });
    }
    [HttpPost("{email}/Enable2FA")]
    public async Task<IActionResult> EnableTwoFactorAuthentication(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }
        var coach = await _coachService.GetCoachByEmailAsync(email);

        if (coach == null)
        {
            return NotFound($"Coach not found.");
        }
        await _coachService.EnableTwoFactorAuthenticationAsync(coach.Id);

        return Ok("Two-factor authentication enabled successfully.");
    }
    [HttpPost("{email}/Disable2FA")]
    public async Task<IActionResult> DisableTwoFactorAuthentication(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }
        var coach = await _coachService.GetCoachByEmailAsync(email);

        if (coach == null)
        {
            return NotFound($"Player not found.");
        }

        await _coachService.DisableTwoFactorAuthenticationAsync(coach.Id);

        return Ok("Two-factor authentication disabled successfully.");
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _coachService.LogoutAsync();
        return Ok("Logout successful.");
    }
    [HttpDelete("{email}")]
    public async Task<IActionResult> DeleteCoachAccount(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }
        var coach = await _coachService.GetCoachByEmailAsync(email);
        if (coach == null)
        {
            return NotFound("Coach not found."); // Player not found
        }
        var success = await _coachService.DeleteCoachAsync(coach.Id);
        if (success)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok("Account deleted successfully");
        }
        return NotFound("Failed to delete coach account.");
    }
    [HttpPost("{email}/ChangePassWord")]
    public async Task<IActionResult> ChangePassword(string email, ChangePasswordDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var coach = await _coachService.GetCoachByEmailAsync(email);
        if (coach == null)
        {
            return NotFound($"Coach not found.");
        }
        var isCurrentPasswordValid = await _coachService.CheckCurrentPasswordAsync(coach.Id, model.CurrentPassword);
        if (!isCurrentPasswordValid)
        {
            return BadRequest("The current password is incorrect.");
        }
        var result = await _coachService.ChangePasswordAsync(coach.Id, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            await _coachService.SignInAsync(coach.Id, isPersistent: false);
            return Ok("Password changed successfully.");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return BadRequest(ModelState);
    }
}
