using Core.Data.Models.CommonViewModel;
using Core.Data.ViewModel.APIClient;
using RestSharp;

namespace Client.ConsumeAPI.APIClient
{
    public interface IAPIClientService<T> where T : class
    {
        Task<List<T>> GetAll(string SubURL);
        Task<T> GetAllView(string SubURL);
        Task<T> GetById(string SubURL);
        Task<JsonResultViewModel> AddUpdate(T model, string subURL);
        Task<RestResponse> Delete(APIClientViewModel vm);
    }
}
