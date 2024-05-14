
using System.Net;
using System.Net.Mail;

namespace UserManagementSystem.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendAccountVerificationEmail(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
        {

                string MailServer = _configuration["EmailSettings:MailServer"]!;
                string FromEmail = _configuration["EmailSettings:FromEmail"]!;
                string Password = _configuration["EmailSettings:Password"]!;
                int Port = int.Parse(_configuration["EmailSettings:MailPort"]!);
                var client = new SmtpClient(MailServer, Port)
                {
                    Credentials = new NetworkCredential(FromEmail, Password),
                    EnableSsl = true,
                };
                MailMessage mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Body)
                {
                    IsBodyHtml = IsBodyHtml
                };
                return client.SendMailAsync(mailMessage);
        }
    }
}
