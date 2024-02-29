using Microsoft.AspNetCore.Identity;

namespace API.Interfaces;

public interface IEmailService
{
    Task<IdentityResult> SendEmailAsync(string email, string subject, string message, bool isHtml = false);
}
