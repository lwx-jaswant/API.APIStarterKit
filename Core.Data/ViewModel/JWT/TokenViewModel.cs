using Core.Data.Pages;

namespace Core.Data.ViewModel.JWT
{
    public class TokenViewModel
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string AccessToken { get; set; }
        public DateTime TokenExpiryTime { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public MainMenuViewModel MainMenuViewModel { get; set; }
    }
}
