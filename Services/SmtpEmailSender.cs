using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace practice_for_wms.Services
{
    // Sends real email via SMTP using System.Net.Mail (no extra NuGet package
    // needed). Configure the "Email" section in appsettings.json / user-secrets
    // - see EmailSettings.cs for the fields it reads.
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IOptions<EmailSettings> settings, ILogger<SmtpEmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendEmailVerificationAsync(string toEmail, string recipientName, string verificationLink)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail, _settings.FromName),
                Subject = "Verify your WMS account",
                IsBodyHtml = true,
                Body = $@"
                    <p>Hi {WebUtility.HtmlEncode(recipientName)},</p>
                    <p>An account was created for you on the Warehouse Management System.
                    Click the link below to verify your email and activate your account:</p>
                    <p><a href=""{verificationLink}"">Verify my email</a></p>
                    <p>This link expires in 24 hours. If you didn't expect this email, you can ignore it.</p>"
            };
            message.To.Add(toEmail);

            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl,
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            };

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Don't let a mail-server hiccup block user creation - log it so
                // whoever's on call can see it and resend/investigate.
                _logger.LogError(ex, "Failed to send verification email to {Email}", toEmail);
                throw;
            }
        }
    }
}
