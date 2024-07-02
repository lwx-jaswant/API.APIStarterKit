using Client.ConsumeAPI.APIClient;
using Core.Data.Models;
using Core.Data.Models.AccountViewModels;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.UserProfileViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using UAParser;
using UserMSDev.APIClient;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITokenService _iTokenService;
        private readonly IAPIClientService<LoginHistory> _iAPIClientService;
        private readonly IAPIClientService<ApplicationUser> _iAPIClientServiceIdentityUser;
        private readonly IAPIClientService<ConfirmEmailViewModel> _iAPIClientServiceConfirmEmail;

        public AccountController(
            ILogger<AccountController> logger,
            ITokenService iTokenService,
            IAPIClientService<LoginHistory> iAPIClientService,
            IAPIClientService<ApplicationUser> iAPIClientServiceIdentityUser,
            IAPIClientService<ConfirmEmailViewModel> iAPIClientServiceConfirmEmail)
        {
            _logger = logger;
            _iTokenService = iTokenService;
            _iAPIClientService = iAPIClientService;
            _iAPIClientServiceIdentityUser = iAPIClientServiceIdentityUser;
            _iAPIClientServiceConfirmEmail = iAPIClientServiceConfirmEmail;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var _SignOutAsync = await _iTokenService.UserSignOut();
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            JsonResultLoginViewModel _JsonResultLoginViewModel = new();
            try
            {
                var _GetTokenByUserCred = await _iTokenService.GetTokenByUserCred(model.Email, model.Password);

                string _AlertMessage = "User logged in.";
                if (_GetTokenByUserCred.AccessToken != null && _GetTokenByUserCred.StatusCode == 1)
                {

                    _JsonResultLoginViewModel.AccessToken = _GetTokenByUserCred.AccessToken;
                    _JsonResultLoginViewModel.TokenExpiryTime = _GetTokenByUserCred.TokenExpiryTime;
                    _JsonResultLoginViewModel.AlertMessage = _AlertMessage;
                    _JsonResultLoginViewModel.IsSuccess = true;

                    var _GetAppInfo = _iTokenService.GetAppInfo();
                    _JsonResultLoginViewModel.APIBaseURL = _GetAppInfo[0];
                    _JsonResultLoginViewModel.ClientBaseURL = _GetAppInfo[1];
                    _logger.LogInformation(_AlertMessage);
                }
                else
                {
                    _AlertMessage = "Invalid Login Attempt. " + _GetTokenByUserCred.StatusMessage;
                    _logger.LogInformation(_AlertMessage);
                    _JsonResultLoginViewModel.AlertMessage = _AlertMessage;
                    _JsonResultLoginViewModel.IsSuccess = false;
                }
                await AddUserLoginHistory(true, _JsonResultLoginViewModel.IsSuccess, model.Email, model.PublicIPAddress);
                return new JsonResult(_JsonResultLoginViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultLoginViewModel.IsSuccess = false;
                _JsonResultLoginViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultLoginViewModel);
                throw;
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }

            ConfirmEmailViewModel _ConfirmEmailViewModel = new()
            {
                ApplicationUserId = userId,
                Code = code
            };
            var SubURLConfirmEmail = "AuthenticationAPI/RegConfirmEmail"; //AuthenticationAPI  UserManagementAPI
            var result = await _iAPIClientServiceConfirmEmail.AddUpdate(_ConfirmEmailViewModel, SubURLConfirmEmail);
            return View(result.IsSuccess ? "ConfirmEmail" : "Error");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetAppInitInfo()
        {
            var _GetAppInfo = _iTokenService.GetAppInfo();
            return new JsonResult(_GetAppInfo);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> UserSignOut(string PublicIPAddress)
        {
            var _UserName = _iTokenService.GetLoginUserName();
            await AddUserLoginHistory(false, true, _UserName, PublicIPAddress);
            var result = await _iTokenService.UserSignOut();
            return new JsonResult(result);
        }
        private async Task AddUserLoginHistory(bool IsLoginAction, bool _IsSuccess, string _UserName, string _PublicIP)
        {
            var _Headers = HttpContext.Request.Headers["User-Agent"];
            var _Parser = Parser.GetDefault();
            ClientInfo _ClientInfo = _Parser.Parse(_Headers);

            LoginHistory _LoginHistory = new LoginHistory
            {
                Browser = _ClientInfo.UA.Family,
                OperatingSystem = _ClientInfo.OS.Family,
                Device = _ClientInfo.Device.Family,
                ActionStatus = _IsSuccess == true ? "Success" : "Failed",
                PublicIP = _PublicIP
            };

            if (IsLoginAction)
            {
                _LoginHistory.UserName = _UserName;
                _LoginHistory.LoginTime = DateTime.Now;
                _LoginHistory.Duration = 0;
                _LoginHistory.Action = "Login";
                _LoginHistory.CreatedBy = _UserName;
                _LoginHistory.ModifiedBy = _UserName;
            }
            else
            {
                _LoginHistory.UserName = _UserName;
                _LoginHistory.LogoutTime = DateTime.Now;
                _LoginHistory.Action = "Logout";
                _LoginHistory.CreatedBy = _UserName;
                _LoginHistory.ModifiedBy = _UserName;
            }
            string SubURL = "CommonDataAPI/AddUserLoginHistory";
            var resut = await _iAPIClientService.AddUpdate(_LoginHistory, SubURL);
        }
    }
}
