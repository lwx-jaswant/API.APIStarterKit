using Microsoft.AspNetCore.Mvc;

namespace API.APIStarterKit.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(string IdentityUserId, string code, string ClientBaseURL)
        {
            var result = ClientBaseURL + "Account/ConfirmEmail?userId=" + IdentityUserId + "&code=" + code;
            return result;
        }
        public static string ResetPasswordCallbackLink(string IdentityUserId, string code, string ClientBaseURL)
        {
            var result = ClientBaseURL + "Account/ResetPassword?userId=" + IdentityUserId + "&code=" + code;
            return result;
        }
        public static string EmailConfirmationLinkOLD(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: "ConfirmEmail",
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
        public static string ResetPasswordCallbackLinkOLD(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            var result = urlHelper.Action(
               action: "ResetPassword",
               controller: "Account",
               values: new { userId, code },
               protocol: scheme);
            return result;
        }
    }
}
