using Client.ConsumeAPI.APIClient;
using Core.Data.Models.DashboardViewModel;
using Core.Data.ViewModel.JWT;
using Microsoft.Extensions.Caching.Memory;
using UserMSDev.APIClient;

namespace UserMSDev.Services
{
    public class DynamicMenuService : IDynamicMenuService
    {
        private IAPIClientService<SharedUIDataViewModel> _iAPIClientService;
        private readonly ITokenService _iTokenService;
        private readonly IMemoryCache _iMemoryCache;
        public DynamicMenuService(IAPIClientService<SharedUIDataViewModel> iAPIClientService, ITokenService iTokenService, IMemoryCache iMemoryCache)
        {
            _iAPIClientService = iAPIClientService;
            _iTokenService = iTokenService;
            _iMemoryCache = iMemoryCache;
        }
        public async Task<SharedUIDataViewModel> GetRequiredAppData()
        {
            try
            {
                TokenViewModel _TokenViewModel = new();
                _iMemoryCache.TryGetValue("TokenViewModel", out _TokenViewModel);
                var IsTokenExpired = _TokenViewModel.TokenExpiryTime > DateTime.Now;
                if (IsTokenExpired == true)
                {
                    SharedUIDataViewModel _SharedUIDataViewModel = new();
                    _iMemoryCache.TryGetValue("SharedUIDataViewModel", out _SharedUIDataViewModel);
                    if (_SharedUIDataViewModel != null)
                    {
                        return _SharedUIDataViewModel;
                    }
                    else
                    {
                        var _GetMenuData = await GetMenuData();
                        return _GetMenuData;
                    }
                }
                else
                {
                    GetRefreshTokenViewModel _GetRefreshTokenViewModel = new();
                    _GetRefreshTokenViewModel.AccessToken = _TokenViewModel.AccessToken;
                    _GetRefreshTokenViewModel.RefreshToken = _TokenViewModel.RefreshToken;
                    var result = await _iTokenService.GetRefreshToken(_GetRefreshTokenViewModel);
                    var _GetMenuData = await GetMenuData();
                    return _GetMenuData;
                }
            }
            catch
            {
                throw;
            }
        }
        private async Task<SharedUIDataViewModel> GetMenuData()
        {
            string _UserName = string.Empty;
            _iMemoryCache.TryGetValue("Email", out _UserName);
            string SubURL = "UserProfileAPI/GetRequiredAppData/" + _UserName;
            var result = await _iAPIClientService.GetById(SubURL);
            _iMemoryCache.Set("SharedUIDataViewModel", result);
            return result;
        }
    }
}
