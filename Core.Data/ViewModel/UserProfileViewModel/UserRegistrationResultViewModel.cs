
namespace Core.Data.Models.UserProfileViewModel
{
    public class SendNewRegistrationEmail
    {

        public string Email { get; set; }
        public string CallBackUrl { get; set; }
    }
    public class ConfirmEmailViewModel
    {
        public string ApplicationUserId { get; set; }
        public string Code { get; set; }
    }
}
