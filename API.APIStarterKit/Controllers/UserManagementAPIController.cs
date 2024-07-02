using API.APIStarterKit.Extensions;
using API.APIStarterKit.GenericRepo;
using API.APIStarterKit.Services;
using Core.Data.Context;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.ManageUserRolesVM;
using Core.Data.Models.UserProfileViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.UserManagement.RoleName)]
    public class UserManagementAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<UserProfile> _Repository;
        private readonly ICommonService _iCommonService;
        private readonly IAccountService _iAccountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolesService _iRolesService;
        private readonly IEmailSender _emailSender;

        public UserManagementAPIController(ApplicationDbContext context,
        IRepository<UserProfile> repository,
        ICommonService iCommonService,
        IAccountService iAccountService,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IRolesService iRolesService,
        IEmailSender emailSender)
        {
            _context = context;
            _Repository = repository;
            _iCommonService = iCommonService;
            _iAccountService = iAccountService;
            _userManager = userManager;
            _roleManager = roleManager;
            _iRolesService = iRolesService;
            _emailSender = emailSender;
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
        [Route("GetByIdIdentityUser/{id}")]
        public async Task<ActionResult<ApplicationUser>> GetByIdIdentityUser(string id)
        {
            var _ApplicationUser = await _context.ApplicationUser.FindAsync(id);
            return Ok(_ApplicationUser);
        }
        [HttpGet]
        [Route("GetEditData/{id}")]
        public async Task<ActionResult<UserProfileCRUDViewModel>> GetEditData(Int64 id)
        {
            try
            {
                UserProfileDropDownView _UserProfileDropDownView = new()
                {
                    listEmployeeType = _iCommonService.GetDropDownListData<EmployeeType>().ToList(),
                    listManageUserRoles = _iCommonService.GetDropDownListData<ManageUserRoles>().ToList(),
                    listDepartment = _iCommonService.GetDropDownListData<Department>().ToList(),
                    listSubDepartment = _iCommonService.GetDropDownListData<SubDepartment>().ToList(),
                    listDesignation = _iCommonService.GetDropDownListData<Designation>().ToList()
                };
                UserProfileCRUDViewModel vm = new();
                if (id > 0)
                {
                    vm = await _iCommonService.GetUserProfileDetails().Where(x => x.Id == id).SingleOrDefaultAsync();
                }
                vm.UserProfileDropDownView = _UserProfileDropDownView;
                return Ok(vm);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult> AddEdit([FromBody] UserProfileCRUDViewModel _UserProfileViewModel)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                if (_UserProfileViewModel.Id > 0)
                {
                    UserProfile _UserProfile = _iCommonService.GetByUserProfile(_UserProfileViewModel.Id);
                    if (_UserProfileViewModel.IsProfilePictureDetailsNull == true)
                        _UserProfileViewModel.ProfilePicture = _UserProfile.ProfilePicture;

                    Int64 _CurrentRoleId = _UserProfile.RoleId;
                    _UserProfileViewModel.ModifiedDate = DateTime.Now;
                    _UserProfileViewModel.ModifiedBy = HttpContext.User.Identity.Name;
                    _UserProfileViewModel.CreatedDate = _UserProfile.CreatedDate;
                    _UserProfileViewModel.CreatedBy = _UserProfile.CreatedBy;
                    _Repository.Update(_UserProfileViewModel, _UserProfile);
                    await _Repository.SaveChangesAsync();

                    if (_CurrentRoleId != _UserProfileViewModel.RoleId)
                        await UpdateUserRoleInUserEdit(_UserProfileViewModel);

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
                        var _DefaultIdentityOptions = await _context.DefaultIdentityOptions.FirstOrDefaultAsync(m => m.Id == 1);
                        if (_DefaultIdentityOptions.SignInRequireConfirmedEmail)
                        {
                            var _ConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(_ApplicationUser.Item1);
                            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(_ConfirmationToken));
                            var _CallbackLink = Extensions.UrlHelperExtensions.EmailConfirmationLink(_ApplicationUser.Item1.Id, code, _UserProfileViewModel.ClientBaseURL);
                            await _emailSender.SendEmailConfirmationAsync(_UserProfileViewModel.Email, _CallbackLink);
                        }

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
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var result = await _Repository.Delete(id);
                _JsonResultViewModel.IsSuccess = await _Repository.SaveChangesAsync();
                var _ApplicationUser = await _context.ApplicationUser.FindAsync(result.ApplicationUserId);
                _context.Remove(_ApplicationUser);
                await _context.SaveChangesAsync();

                _JsonResultViewModel.AlertMessage = "User has been deleted successfully. User Id: " + id;
                _JsonResultViewModel.IsSuccess = true;
                return Ok(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return Ok(_JsonResultViewModel);
            }
        }
        [HttpPost]
        [Route("ResetPasswordAdmin")]
        public async Task<IActionResult> ResetPasswordAdmin(ResetPasswordAdminVM vm)
        {
            try
            {
                string AlertMessage = string.Empty;
                var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
                if (vm.NewPassword.Equals(vm.ConfirmPassword))
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(_ApplicationUser);
                    var _ResetPasswordAsync = await _userManager.ResetPasswordAsync(_ApplicationUser, code, vm.NewPassword);
                    if (_ResetPasswordAsync.Succeeded)
                        AlertMessage = "Reset Password Succeeded. User name: " + _ApplicationUser.Email;
                    else
                    {
                        string errorMessage = string.Empty;
                        foreach (var item in _ResetPasswordAsync.Errors)
                        {
                            errorMessage = errorMessage + " " + item.Description;
                        }
                        AlertMessage = "error Reset password failed." + errorMessage;
                    }
                }
                return new JsonResult(AlertMessage);
            }
            catch (Exception ex)
            {
                return new JsonResult("error" + ex.Message);
                throw;
            }
        }
        private async Task<bool> UpdateUserRoleInUserEdit(UserProfileCRUDViewModel vm)
        {
            var _ManageRoleDetails = await _context.ManageUserRolesDetails.Where(x => x.ManageRoleId == vm.RoleId && x.IsAllowed == true).ToListAsync();
            var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
            var roles = await _userManager.GetRolesAsync(_ApplicationUser);
            var result = await _userManager.RemoveFromRolesAsync(_ApplicationUser, roles);
            foreach (var item in _ManageRoleDetails)
            {
                await _userManager.AddToRoleAsync(_ApplicationUser, item.RoleName);
            }
            return true;
        }
    }
}
