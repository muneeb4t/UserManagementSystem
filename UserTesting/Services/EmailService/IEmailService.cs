namespace UserManagementSystem.Services.EmailService
{
    public interface IEmailService
    {
        Task SendAccountVerificationEmail(string ToEmail, string Subject, string Body, bool IsBodyHtml = false);
    }
}
