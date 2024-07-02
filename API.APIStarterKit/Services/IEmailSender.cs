using Core.Data.Models.EmailConfigViewModel;

namespace API.APIStarterKit.Services
{
    public interface IEmailSender
    {
        Task<Task> SendEmailAsync(string email, string subject, string message);
        Task<Task> SendEmailByGmailAsync(SendEmailViewModel vm);
    }
}
