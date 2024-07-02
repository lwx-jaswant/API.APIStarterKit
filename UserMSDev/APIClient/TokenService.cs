using Core.Data.Pages;
using Core.Data.ViewModel.JWT;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RestSharp;

namespace UserMSDev.APIClient
{
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _iMemoryCache;
        private readonly RestClient _RestClient;
        private readonly IConfiguration _iConfiguration;
        public TokenService(IMemoryCache iMemoryCache, IConfiguration iConfiguration)
        {
            _iMemoryCache = iMemoryCache;
            _iConfiguration = iConfiguration;
            _RestClient = new RestClient(_iConfiguration["WebAppUrls:APIBaseURL"]);
        }
        public async Task<TokenViewModel> GetTokenByUserCred(string _Email, string _Password)
        {
            TokenViewModel _TokenViewModel = new();
            try
            {
                LoginModel _LoginModel = new()
                {
                    Username = _Email,
                    Password = _Password,
                };

                string _subURL = "AuthenticationAPI/Login";
                var _RestRequest = new RestRequest(_subURL, Method.Post);
                _RestRequest.AddJsonBody(_LoginModel);
                RestResponse _RestResponse = await _RestClient.ExecuteAsync(_RestRequest);
                _TokenViewModel = JsonConvert.DeserializeObject<TokenViewModel>(_RestResponse.Content);

                if (_TokenViewModel != null)
                {
                    _iMemoryCache.Set("TokenViewModel", _TokenViewModel);
                    _iMemoryCache.Set("MainMenuViewModel", _TokenViewModel.MainMenuViewModel);
                    _iMemoryCache.Set("Email", _Email);
                    _iMemoryCache.Set("APIBaseURL", _iConfiguration["WebAppUrls:APIBaseURL"]);
                    _iMemoryCache.Set("ClientBaseURL", _iConfiguration["WebAppUrls:ClientBaseURL"]);
                    return _TokenViewModel;
                }
                else
                {
                    _TokenViewModel.StatusMessage = "Token is Exparied or Token is Empty.";
                    return _TokenViewModel;
                }
            }
            catch (Exception ex)
            {
                _TokenViewModel.StatusMessage = "Token is Exparied or Token is Empty." + ex.Message;
                return _TokenViewModel;
            }
        }
        public string GetToken()
        {
            TokenViewModel _TokenViewModel = new();
            _iMemoryCache.TryGetValue("TokenViewModel", out _TokenViewModel);

            if (_TokenViewModel != null)
            {
                var IsTokenExpired = _TokenViewModel.TokenExpiryTime > DateTime.Now;
                if (IsTokenExpired == true)
                    return _TokenViewModel.AccessToken;
                else
                    return null;
            }
            else
            {
                return null;
            }

        }
        public TokenViewModel GetTokenDetails()
        {
            TokenViewModel _TokenViewModel = new();
            _iMemoryCache.TryGetValue("TokenViewModel", out _TokenViewModel);
            return _TokenViewModel;
        }
        public MainMenuViewModel GetMenuAccessList()
        {
            MainMenuViewModel _MainMenuViewModel = new();
            _iMemoryCache.TryGetValue("MainMenuViewModel", out _MainMenuViewModel);
            return _MainMenuViewModel;
        }
        public string GetLoginUserName()
        {
            string _Email = string.Empty;
            _iMemoryCache.TryGetValue("Email", out _Email);
            return _Email;
        }
        public async Task<bool> UserSignOut()
        {
            try
            {
                _iMemoryCache.Set("TokenViewModel", "");
                _iMemoryCache.Set("SharedUIDataViewModel", "");

                string _subURL = "AuthenticationAPI/UserSignOut";
                var _RestRequest = new RestRequest(_subURL, Method.Post);
                RestResponse _RestResponse = await _RestClient.ExecuteAsync(_RestRequest);
                var result = JsonConvert.DeserializeObject<bool>(_RestResponse.Content);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel vm)
        {
            try
            {
                string _subURL = "RefreshTokensAPI/RefreshToken";
                var _RestRequest = new RestRequest(_subURL, Method.Post);
                _RestRequest.AddJsonBody(vm);
                RestResponse _RestResponse = await _RestClient.ExecuteAsync(_RestRequest);
                var result = JsonConvert.DeserializeObject<TokenViewModel>(_RestResponse.Content);
                _iMemoryCache.Set("TokenViewModel", result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string[] GetAppInfo()
        {
            try
            {
                var _APIBaseURL = _iConfiguration["WebAppUrls:APIBaseURL"];
                var ClientBaseURL = _iConfiguration["WebAppUrls:ClientBaseURL"];
                string[] result = { _APIBaseURL, ClientBaseURL };
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
