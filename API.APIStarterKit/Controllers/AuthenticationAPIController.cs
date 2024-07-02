using API.APIStarterKit.Extensions;
using API.APIStarterKit.Services;
using Core.Data.Context;
using Core.Data.Models;
using Core.Data.Models.AccountViewModels;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.UserProfileViewModel;
using Core.Data.ViewModel.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthenticationAPIController> _logger;

        public AuthenticationAPIController(ApplicationDbContext context, IAuthService authService, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<AuthenticationAPIController> logger)
        {
            _context = context;
            _authService = authService;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var result = await _authService.Login(model);
                if (result.StatusCode == 0)
                    return BadRequest(result.StatusMessage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var _ApplicationUser = new ApplicationUser
                {
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Email = model.Email,
                    EmailConfirmed = false
                };
                var result = await _userManager.CreateAsync(_ApplicationUser, model.Password);
                if (result.Succeeded)
                {
                    var _ManageRoleDetails = await _context.ManageUserRolesDetails.Where(x=>x.ManageRoleId == 2 && x.IsAllowed == true).ToListAsync();
                    foreach(var item in _ManageRoleDetails)
                    {
                        await _userManager.AddToRoleAsync(_ApplicationUser, item.RoleName);
                    }

                    //Insert: UserProfile
                    UserProfile _UserProfile = new()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        Address = model.Address,
                        Country = model.Country,
                        ApplicationUserId = _ApplicationUser.Id,

                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = HttpContext.User.Identity.Name,
                        ModifiedBy = HttpContext.User.Identity.Name
                    };
                    var result2 = await _context.UserProfile.AddAsync(_UserProfile);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User created a new account with password.");

                    var _DefaultIdentityOptions = await _context.DefaultIdentityOptions.FirstOrDefaultAsync(m => m.Id == 1);
                    if (_DefaultIdentityOptions.SignInRequireConfirmedEmail)
                    {
                        var _ConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(_ApplicationUser);
                        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(_ConfirmationToken));
                        var _CallbackLink = Extensions.UrlHelperExtensions.EmailConfirmationLink(_ApplicationUser.Id, code, model.ClientBaseURL);
                        
                        await _emailSender.SendEmailConfirmationAsync(_ApplicationUser.Email, _CallbackLink);
                    }

                    _logger.LogInformation("User created a new account with password.");
                    _JsonResultViewModel.AlertMessage = "User Created Successfully. User Name: " + _ApplicationUser.Email;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    string errorMessage = string.Empty;
                    foreach (var item in result.Errors)
                    {
                        errorMessage = errorMessage + item.Description;
                    }
                    _JsonResultViewModel.AlertMessage = "User Creation Failed." + errorMessage;
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }
        [HttpPost]
        [Route("RegConfirmEmail")]
        public async Task<IActionResult> RegConfirmEmail([FromBody] ConfirmEmailViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var _ApplicationUser = await _context.ApplicationUser.FindAsync(vm.ApplicationUserId);
                if (_ApplicationUser == null)
                {
                    _JsonResultViewModel.IsSuccess = false;
                    _JsonResultViewModel.AlertMessage = $"Unable to load user with ID '{vm.ApplicationUserId}'.";
                }

                var bytes = WebEncoders.Base64UrlDecode(vm.Code);
                var code = Encoding.UTF8.GetString(bytes);
                var result = await _userManager.ConfirmEmailAsync(_ApplicationUser, code);
                _JsonResultViewModel.IsSuccess = result.Succeeded;
                return Ok(_JsonResultViewModel);
            }
            catch (Exception)
            {
                _JsonResultViewModel.IsSuccess = false;
                return Ok(_JsonResultViewModel);
            }
        }

        [HttpPost]
        [Route("UserSignOut")]
        public async Task<IActionResult> UserSignOut()
        {
            try
            {
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                JsonResultViewModel _JsonResultViewModel = new();
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _JsonResultViewModel.AlertMessage = "User not found with this email: " + model.Email;
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
                else if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    _JsonResultViewModel.AlertMessage = "User email is not confirmed yet. Please confirm email first. Email: " + model.Email;
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
                var _PasswordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(_PasswordResetToken));
                var _CallbackLink = Extensions.UrlHelperExtensions.ResetPasswordCallbackLink(user.Id, code, model.ClientBaseURL);

                await _emailSender.SendEmailForgotPasswordAsync(model.Email, _CallbackLink);

                _JsonResultViewModel.AlertMessage = "Success, . Email: " + model.Email;
                _JsonResultViewModel.IsSuccess = true;
                return new JsonResult(_JsonResultViewModel);
            }
            return new JsonResult(model);
        }
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _JsonResultViewModel.AlertMessage = "Incorrect information.";
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }

                var bytes = WebEncoders.Base64UrlDecode(model.Code);
                var code = Encoding.UTF8.GetString(bytes);
                var result = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);
                if (result.Succeeded)
                {
                    _JsonResultViewModel.AlertMessage = "Success. Email: " + model.Email;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    _JsonResultViewModel.AlertMessage = "Reset Password Failed. Email: " + model.Email;
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(ex.Message);
                throw;
            }
        }
    }
}