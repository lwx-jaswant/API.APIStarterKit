using Client.ConsumeAPI.APIClient;
using Core.Data.Models.DashboardViewModel;
using Microsoft.AspNetCore.Mvc;
using UserMSDev.APIClient;

namespace UserMSDev.Controllers
{
    public class DashboardController : Controller
    {
        private IAPIClientService<UserMSDevSummaryViewModel> _iAPIClientService;
        private readonly ITokenService _iTokenService;
        private string SubURL = string.Empty;
        public DashboardController(IAPIClientService<UserMSDevSummaryViewModel> iAPIClientService, ITokenService iTokenService)
        {
            _iAPIClientService = iAPIClientService;
            _iTokenService = iTokenService;
            SubURL = "DashboardAPI";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string _GetToken = _iTokenService.GetToken();
            if (_GetToken != null)
            {
                var _MainMenuViewModel = _iTokenService.GetMenuAccessList();
                if(_MainMenuViewModel.Admin)
                {
                    SubURL = SubURL + "/GetDashboardData";
                    UserMSDevSummaryViewModel result = await _iAPIClientService.GetById(SubURL);
                    return View(result);
                }
                else
                {
                    return RedirectToAction("IndexGeneral", "Dashboard");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpGet]
        public IActionResult IndexGeneral()
        {
            return View();
        }
    }
}
