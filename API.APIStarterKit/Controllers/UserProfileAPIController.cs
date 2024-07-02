using API.APIStarterKit.GenericRepo;
using API.APIStarterKit.Services;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.DashboardViewModel;
using Core.Data.Models.UserProfileViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.UserProfile.RoleName)]
    public class UserProfileAPIController : ControllerBase
    {
        private readonly IRepository<UserProfile> _Repository;
        private readonly ICommonService _iCommonService;
        private readonly IAccountService _iAccountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRolesService _iRolesService;
        private readonly ApplicationInfo _applicationInfo;

        public UserProfileAPIController(IRepository<UserProfile> Repository, ICommonService iCommonService, IAccountService iAccountService, UserManager<ApplicationUser> userManager, IRolesService iRolesService, IOptions<ApplicationInfo> applicationInfo)
        {
            _Repository = Repository;
            _iCommonService = iCommonService;
            _iAccountService = iAccountService;
            _userManager = userManager;
            _iRolesService = iRolesService;
          _applicationInfo = applicationInfo.Value;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetAll()
        {
            var result = await _Repository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<UserProfileCRUDViewModel>> GetById(Int64 id)
        {
            var result = await _iCommonService.GetUserProfileDetails().Where(x => x.Id == id).SingleOrDefaultAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetGenUserProfile/{email}")]
        public async Task<ActionResult<UserProfileCRUDViewModel>> GetGenUserProfile(string email)
        {
            var _UserProfile = await _Repository.FindByConditionAsync(x => x.Email == email);
            var result = await _iCommonService.GetUserProfileDetails().Where(x => x.Id == _UserProfile.Id).SingleOrDefaultAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetApplicationUser/{applicationUserId}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string applicationUserId)
        {
            var _ApplicationUser = await _userManager.FindByIdAsync(applicationUserId);
            return Ok(_ApplicationUser);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<JsonResult> AddEdit([FromBody] UserProfileCRUDViewModel _UserProfileViewModel)
        {
            var _UserName = HttpContext.User.Identity.Name;
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                if (_UserProfileViewModel.Id > 0)
                {
                    UserProfile _UserProfile = _iCommonService.GetByUserProfile(_UserProfileViewModel.Id);
                    if (_UserProfileViewModel.ProfilePictureDetails != null)
                        _UserProfileViewModel.ProfilePicture = "/upload/" + _iCommonService.UploadedFile(_UserProfileViewModel.ProfilePictureDetails);
                    else
                        _UserProfileViewModel.ProfilePicture = _UserProfile.ProfilePicture;

                    Int64 _CurrentRoleId = _UserProfile.RoleId;
                    _UserProfileViewModel.ModifiedDate = DateTime.Now;
                    _UserProfileViewModel.ModifiedBy = HttpContext.User.Identity.Name;
                    _UserProfileViewModel.CreatedDate = _UserProfile.CreatedDate;
                    _UserProfileViewModel.CreatedBy = _UserProfile.CreatedBy;
                    _Repository.Update(_UserProfileViewModel, _UserProfile);
                    await _Repository.SaveChangesAsync();

                    if (_CurrentRoleId != _UserProfileViewModel.RoleId)
                        await UpdateUserRole(_UserProfileViewModel);

                    _JsonResultViewModel.AlertMessage = "User info Updated Successfully. User Name: " + _UserProfile.Email;
                    _JsonResultViewModel.CurrentURL = _UserProfileViewModel.CurrentURL;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    var _ApplicationUser = await _iAccountService.CreateUserProfile(_UserProfileViewModel, HttpContext.User.Identity.Name);
                    if (_ApplicationUser.Item2 == "Success")
                    {
                        //var _DefaultIdentityOptions = await _context.DefaultIdentityOptions.FirstOrDefaultAsync(m => m.Id == 1);
                        //if (_DefaultIdentityOptions.SignInRequireConfirmedEmail)
                        //{
                        //var _ConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(_ApplicationUser.Item1);
                        //var callbackUrl = Url.EmailConfirmationLink(_ApplicationUser.Item1.Id, _ConfirmationToken, Request.Scheme);
                        //await _emailSender.SendEmailConfirmationAsync(_ApplicationUser.Item1.Email, callbackUrl);
                        //}

                        _JsonResultViewModel.AlertMessage = "User Created Successfully. User Name: " + _ApplicationUser.Item1.Email;
                        _JsonResultViewModel.CurrentURL = _UserProfileViewModel.CurrentURL;
                        _JsonResultViewModel.IsSuccess = true;
                        return new JsonResult(_JsonResultViewModel);
                    }
                    else
                    {
                        _JsonResultViewModel.AlertMessage = "User Creation Failed." + _ApplicationUser.Item2;
                        _JsonResultViewModel.IsSuccess = false;
                        return new JsonResult(_JsonResultViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(ex.Message);
                throw;
            }
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(Int64 id)
        {
            var result = await _Repository.Delete(id);
            await _Repository.SaveChangesAsync();
            return Ok(result);
        }
        [HttpPost]
        [Route("SaveResetPassword")]
        public async Task<IActionResult> SaveResetPassword([FromBody] ResetPasswordVM vm)
        {
            try
            {
                string AlertMessage = string.Empty;
                var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
                if (vm.NewPassword.Equals(vm.ConfirmPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(_ApplicationUser, vm.OldPassword, vm.NewPassword);
                    if (result.Succeeded)
                        AlertMessage = "Change Password Succeeded. User name: " + _ApplicationUser.Email;
                    else
                    {
                        string errorMessage = string.Empty;
                        foreach (var item in result.Errors)
                        {
                            errorMessage = errorMessage + " " + item.Description;
                        }
                        AlertMessage = "error" + errorMessage;
                    }
                }
                return new JsonResult(AlertMessage);
                //return Ok(AlertMessage);
            }
            catch (Exception ex)
            {
                return new JsonResult("error" + ex.Message);
                throw;
            }
        }
        private async Task<bool> UpdateUserRole(UserProfileCRUDViewModel vm)
        {
            var _ManageRoleDetails = await _iCommonService.GetByManageUserRolesDetails(vm.RoleId);
            var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
            var roles = await _userManager.GetRolesAsync(_ApplicationUser);
            var result = await _userManager.RemoveFromRolesAsync(_ApplicationUser, roles);
            foreach (var item in _ManageRoleDetails)
            {
                await _userManager.AddToRoleAsync(_ApplicationUser, item.RoleName);
            }
            return true;
        }
        [HttpGet]
        [Route("GetRequiredAppData/{_UserName}")]
        public async Task<SharedUIDataViewModel> GetRequiredAppData(string _UserName)
        {
            try
            {
                SharedUIDataViewModel _SharedUIDataViewModel = new();
                var _ApplicationUser = await _userManager.FindByEmailAsync(_UserName);
                var _UserProfile = await _Repository.FindByConditionAsync(x => x.Email == _ApplicationUser.Email);
                _SharedUIDataViewModel.UserProfile = _UserProfile;
                _SharedUIDataViewModel.MainMenuViewModel = await _iRolesService.RolebaseMenuLoad(_ApplicationUser);
                _SharedUIDataViewModel.ApplicationInfo = _applicationInfo;
                return _SharedUIDataViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
