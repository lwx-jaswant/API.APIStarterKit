using Core.Data.Models.CommonViewModel;
using Core.Data.ViewModel.APIClient;
using RestSharp;
using UserMSDev.APIClient;

namespace Client.ConsumeAPI.APIClient
{
    public class APIClientService<T> : IAPIClientService<T> where T : class
    {
        private readonly ITokenService _iTokenService;
        private readonly RestClient _RestClient;
        private readonly IConfiguration _iConfiguration;
        public APIClientService(ITokenService iTokenService, IConfiguration iConfiguration)
        {
            _iTokenService = iTokenService;
            _iConfiguration = iConfiguration;
            _RestClient = new RestClient(_iConfiguration["WebAppUrls:APIBaseURL"]);
        }
        public async Task<List<T>> GetAll(string SubURL)
        {
            string _GetToken = _iTokenService.GetToken();
            if (_GetToken != null || _GetToken != "")
            {
                var _RestRequest = new RestRequest(SubURL, Method.Get)
                { RequestFormat = DataFormat.Json };
                _RestRequest.AddHeader("Authorization", $"Bearer {_GetToken}");
                var response = await _RestClient.ExecuteAsync<List<T>>(_RestRequest);

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new Exception($"Not Accessable {response.Content}");
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Some Error Occured {response.Content}");
                }
                return response.Data;
            }
            else
            {
                return null;
            }
        }
        public async Task<T> GetAllView(string SubURL)
        {
            string _GetToken = _iTokenService.GetToken();
            var _RestRequest = new RestRequest(SubURL, Method.Get)
            { RequestFormat = DataFormat.Json };
            _RestRequest.AddHeader("Authorization", $"Bearer {_GetToken}");
            var response = await _RestClient.ExecuteAsync<T>(_RestRequest);
            return response.Data;
        }
        public async Task<T> GetById(string SubURL)
        {
            string _GetToken = _iTokenService.GetToken();
            var _RestRequest = new RestRequest(SubURL, Method.Get)
            { RequestFormat = DataFormat.Json };
            _RestRequest.AddHeader("Authorization", $"Bearer {_GetToken}");

            var response = await _RestClient.ExecuteAsync<T>(_RestRequest);
            return response.Data;
        }

        public async Task<JsonResultViewModel> AddUpdate(T model, string subURL)
        {
            string _GetToken = _iTokenService.GetToken();
            var _RestRequest = new RestRequest(subURL, Method.Post);
            _RestRequest.AddHeader("Authorization", $"Bearer {_GetToken}");
            _RestRequest.AddJsonBody(model);
            var _RestResponse = await _RestClient.ExecuteAsync<JsonResultViewModel>(_RestRequest);
            return _RestResponse.Data;
        }
        public async Task<RestResponse> Delete(APIClientViewModel vm)
        {
            string _GetToken = _iTokenService.GetToken();
            var _RestRequest = new RestRequest(vm.SubURL + vm.Id, Method.Delete);
            RestResponse _RestResponse = await _RestClient.ExecuteAsync(_RestRequest);
            return _RestResponse;
        }
    }
}
