using API.APIStarterKit.Services;
using System.Text.Encodings.Web;

namespace API.APIStarterKit.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm Your Email: User Management System",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>"
                 + "<br /><br />Thanks<br />Admin<br />Email: userms.sys@gmail.com");
        }
        public static Task SendEmailForgotPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
        {
            return emailSender.SendEmailAsync(email, "Reset Password: User Management System",
                   $"Please reset your password by clicking here: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>"
                   + "<br /><br />Thanks<br />Admin<br />Email: userms.sys@gmail.com");
        }
    }
}
