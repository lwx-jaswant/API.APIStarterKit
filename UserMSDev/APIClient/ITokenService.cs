using Core.Data.Pages;
using Core.Data.ViewModel.JWT;

namespace UserMSDev.APIClient
{
    public interface ITokenService
    {
        public Task<TokenViewModel> GetTokenByUserCred(string Email, string Password);
        public string GetToken();
        public TokenViewModel GetTokenDetails();
        MainMenuViewModel GetMenuAccessList();
        public string GetLoginUserName();
        Task<bool> UserSignOut();
        Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel vm);
        string[] GetAppInfo();
    }
}
