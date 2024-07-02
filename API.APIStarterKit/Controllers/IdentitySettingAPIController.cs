using API.APIStarterKit.GenericRepo;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.IdentitySetting.RoleName)]
    public class IdentitySettingAPIController : ControllerBase
    {
        private readonly IRepository<DefaultIdentityOptions> _Repository;
        private readonly IdentityOptions _identityOptions;

        public IdentitySettingAPIController(IRepository<DefaultIdentityOptions> Repository, IOptions<IdentityOptions> iOptions)
        {
            _Repository = Repository;
            _identityOptions = iOptions.Value;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<DefaultIdentityOptions>>> GetAll()
        {
            var result = await _Repository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<DefaultIdentityOptions>> GetById(Int64 id)
        {
            var result = await _Repository.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult> AddEdit(DefaultIdentityOptions vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                DefaultIdentityOptions _DefaultIdentityOptions = await _Repository.GetByIdAsync(vm.Id);

                vm.ModifiedDate = DateTime.Now;
                vm.ModifiedBy = HttpContext.User.Identity.Name;
                _Repository.Update(vm, _DefaultIdentityOptions);
                await _Repository.SaveChangesAsync();

                IdentityOptionUpdate(_DefaultIdentityOptions);
                _JsonResultViewModel.AlertMessage = "Default Identity Options Updated Successfully. Id: " + _DefaultIdentityOptions.Id;
                _JsonResultViewModel.IsSuccess = true;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(ex.Message);
                throw;
            }
        }
        private void IdentityOptionUpdate(DefaultIdentityOptions _DefaultIdentityOptions)
        {
            _identityOptions.Password.RequireDigit = _DefaultIdentityOptions.PasswordRequireDigit;
            _identityOptions.Password.RequiredLength = _DefaultIdentityOptions.PasswordRequiredLength;
            _identityOptions.Password.RequireNonAlphanumeric = _DefaultIdentityOptions.PasswordRequireNonAlphanumeric;
            _identityOptions.Password.RequireUppercase = _DefaultIdentityOptions.PasswordRequireUppercase;
            _identityOptions.Password.RequireLowercase = _DefaultIdentityOptions.PasswordRequireLowercase;
            _identityOptions.Password.RequiredUniqueChars = _DefaultIdentityOptions.PasswordRequiredUniqueChars;

            _identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(_DefaultIdentityOptions.LockoutDefaultLockoutTimeSpanInMinutes);
            _identityOptions.Lockout.MaxFailedAccessAttempts = _DefaultIdentityOptions.LockoutMaxFailedAccessAttempts;
            _identityOptions.Lockout.AllowedForNewUsers = _DefaultIdentityOptions.LockoutAllowedForNewUsers;

            _identityOptions.User.RequireUniqueEmail = _DefaultIdentityOptions.UserRequireUniqueEmail;
            _identityOptions.SignIn.RequireConfirmedEmail = _DefaultIdentityOptions.SignInRequireConfirmedEmail;

            //_IdentityOptions.Cookie.HttpOnly = _DefaultIdentityOptions.CookieHttpOnly;
            //_IdentityOptions.Cookie.Expiration = TimeSpan.FromDays(_DefaultIdentityOptions.CookieExpiration);
            //_IdentityOptions.ExpireTimeSpan = TimeSpan.FromMinutes(_DefaultIdentityOptions.CookieExpireTimeSpan);
        }
    }
}
