using Core.Data.Models;
using Core.Data.Models.DashboardViewModel;
using System.Security.Claims;

namespace API.APIStarterKit.Services
{
    public interface IFunctional
    {
        Task CreateDefaultSuperAdmin();
        Task CreateDefaultOtherUser();

        Task SendEmailBySendGridAsync(string apiKey,
            string fromEmail,
            string fromFullName,
            string subject,
            string message,
            string email);

        Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL);

        Task InitAppData();
        Task GenerateUserUserRole();
        Task CreateDefaultEmailSettings();
        Task<SharedUIDataViewModel> GetSharedUIData(ClaimsPrincipal _ClaimsPrincipal);
        Task CreateDefaultIdentitySettings();
        Task<DefaultIdentityOptions> GetDefaultIdentitySettings();
        Task<string> UploadFile(List<IFormFile> files, IWebHostEnvironment env, string uploadFolder);
    }
}
