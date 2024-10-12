using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebProject.Services.MailService
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Task.Run(() => { });
            return Task.CompletedTask;
        }
    }
}
