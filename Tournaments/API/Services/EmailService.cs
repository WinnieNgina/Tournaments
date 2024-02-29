using API.Interfaces;
using API.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;

namespace API.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly SmtpClient _smtpClient;

    public EmailService(IOptions<EmailOptions> emailOptions, SmtpClient smtpClient)
    {
        _emailOptions = emailOptions.Value;
        _smtpClient = smtpClient;
    }

    public async Task<IdentityResult> SendEmailAsync(string email, string subject, string message, bool isHtml = false)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.SenderEmail));
        emailMessage.To.Add(new MailboxAddress("Recipient Name", email));
        emailMessage.Subject = subject;

        if (isHtml)
        {
            emailMessage.Body = new TextPart("html")
            {
                Text = message
            };
        }
        else
        {
            emailMessage.Body = new TextPart("plain")
            {
                Text = message
            };
        }

        await _smtpClient.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.SmtpPort, SecureSocketOptions.StartTls);
        await _smtpClient.AuthenticateAsync(_emailOptions.SmtpUsername, _emailOptions.SmtpPassword);
        await _smtpClient.SendAsync(emailMessage);
        await _smtpClient.DisconnectAsync(true);
        return IdentityResult.Success;
    }

}
