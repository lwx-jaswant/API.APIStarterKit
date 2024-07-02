
using Core.Data.Pages;

namespace Core.Data.Models.CommonViewModel
{
    public class JsonResultViewModel
    {

        public string AlertMessage { get; set; }
        public string CurrentURL { get; set; }
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; }
        public DateTime TokenExpiryTime { get; set; }
    }
    public class JsonResultLoginViewModel
    {

        public string AlertMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; }
        public DateTime TokenExpiryTime { get; set; }
        public string APIBaseURL { get; set; }
        public string ClientBaseURL { get; set; }
    }
}
