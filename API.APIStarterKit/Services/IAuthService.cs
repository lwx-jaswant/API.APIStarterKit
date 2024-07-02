using Core.Data.ViewModel.JWT;

namespace API.APIStarterKit.Services
{
    public interface IAuthService
    {
        Task<(int, string)> Registeration(RegistrationModel model, string role);
        Task<TokenViewModel> Login(LoginModel model);
        Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel model);
    }
}
