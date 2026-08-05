namespace practice_for_wms.Services
{
    // No SMTP is configured for this project yet, so this just logs the
    // verification link instead of actually sending an email. That's enough
    // to test the verification flow locally: create a user, then copy the
    // link from the console/output window instead of checking an inbox.
    //
    // To send real emails later, implement IEmailSender against an SMTP
    // provider (System.Net.Mail.SmtpClient, SendGrid, etc.) and swap the
    // registration in Program.cs.
    public class ConsoleEmailSender : IEmailSender
    {
        private readonly ILogger<ConsoleEmailSender> _logger;

        public ConsoleEmailSender(ILogger<ConsoleEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailVerificationAsync(string toEmail, string recipientName, string verificationLink)
        {
            _logger.LogInformation(
                "==== EMAIL VERIFICATION (no SMTP configured - dev mode) ====\n" +
                "To: {Email}\n" +
                "Hi {Name}, verify your WMS account using this link:\n{Link}\n" +
                "==============================================================",
                toEmail, recipientName, verificationLink);

            return Task.CompletedTask;
        }
    }
}
