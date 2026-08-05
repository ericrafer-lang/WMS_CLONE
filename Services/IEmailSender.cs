namespace practice_for_wms.Services
{
    public interface IEmailSender
    {
        Task SendEmailVerificationAsync(string toEmail, string recipientName, string verificationLink);
    }
}
