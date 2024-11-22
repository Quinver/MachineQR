using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
        {
            Port = int.TryParse(_configuration["Smtp:Port"], out var port) ? port : throw new InvalidOperationException("Invalid SMTP port configuration"),
            Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
            EnableSsl = bool.TryParse(_configuration["Smtp:EnableSsl"], out var enableSsl) ? enableSsl : throw new InvalidOperationException("Invalid SMTP EnableSsl configuration"),
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:From"] ?? throw new InvalidOperationException("SMTP From address is not configured")),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}