using Client.ConsumeAPI.APIClient;
using Core.Data.Models.UserProfileViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class UserProfileController : Controller
    {
        private IAPIClientService<UserProfileCRUDViewModel> _iAPIClientService;
        private readonly IMemoryCache _iMemoryCache;
        private string SubURL = string.Empty;

        public UserProfileController(IAPIClientService<UserProfileCRUDViewModel> iAPIClientService, IMemoryCache iMemoryCache)
        {
            _iAPIClientService = iAPIClientService;
            _iMemoryCache = iMemoryCache;
            SubURL = "UserProfileAPI";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string _Email = string.Empty;
            _iMemoryCache.TryGetValue("Email", out _Email);
            SubURL = SubURL + "/GetGenUserProfile/" + _Email;
            var result = await _iAPIClientService.GetById(SubURL);
            return View(result);
        }

        [HttpGet]
        public IActionResult ResetPassword(string ApplicationUserId)
        {
            ResetPasswordVM vm = new();
            vm.ApplicationUserId = ApplicationUserId;
            return PartialView("_ResetPassword", vm);
        }
    }
}
