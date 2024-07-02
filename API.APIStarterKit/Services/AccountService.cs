using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Data.Context;
using Core.Data.Models;
using Core.Data.Models.AccountViewModels;
using Core.Data.Models.UserProfileViewModel;

namespace API.APIStarterKit.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommonService _iCommonService;
        public AccountService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ICommonService iCommonService)
        {
            _context = context;
            _userManager = userManager;
            _iCommonService = iCommonService;
        }
        public async Task<Tuple<ApplicationUser, IdentityResult>> CreateUserAccount(CreateUserAccountViewModel _CreateUserAccountViewModel)
        {
            IdentityResult _IdentityResult = null;
            ApplicationUser _ApplicationUser = _CreateUserAccountViewModel;

            try
            {
                if (_CreateUserAccountViewModel.PasswordHash.Equals(_CreateUserAccountViewModel.ConfirmPassword))
                {
                    _IdentityResult = await _userManager.CreateAsync(_ApplicationUser, _CreateUserAccountViewModel.PasswordHash);
                }
                return Tuple.Create(_ApplicationUser, _IdentityResult);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<ApplicationUser, string>> CreateUserProfile(UserProfileCRUDViewModel vm, string LoginUser)
        {
            UserProfile _UserProfile = new();
            string errorMessage = string.Empty;
            try
            {
                IdentityResult _IdentityResult = null;
                ApplicationUser _ApplicationUser = new ApplicationUser()
                {
                    UserName = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Email = vm.Email,
                    EmailConfirmed = false
                };
                if (vm.PasswordHash.Equals(vm.ConfirmPassword))
                {
                    _IdentityResult = await _userManager.CreateAsync(_ApplicationUser, vm.PasswordHash);
                }

                if (_IdentityResult.Succeeded)
                {
                    vm.ApplicationUserId = _ApplicationUser.Id;
                    vm.PasswordHash = _ApplicationUser.PasswordHash;
                    vm.ConfirmPassword = _ApplicationUser.PasswordHash;
                    vm.CreatedDate = DateTime.Now;
                    vm.ModifiedDate = DateTime.Now;
                    vm.CreatedBy = LoginUser;
                    vm.ModifiedBy = LoginUser;

                    _UserProfile = vm;
                    await _context.UserProfile.AddAsync(_UserProfile);
                    var result = await _context.SaveChangesAsync();

                    var _ManageRoleDetails = await _context.ManageUserRolesDetails.Where(x=>x.ManageRoleId == vm.RoleId && x.IsAllowed == true).ToListAsync();
                    foreach(var item in _ManageRoleDetails)
                    {
                        await _userManager.AddToRoleAsync(_ApplicationUser, item.RoleName);
                    }
                }
                else
                {
                    foreach (var item in _IdentityResult.Errors)
                    {
                        errorMessage = errorMessage + " " + item.Description;
                    }
                    return Tuple.Create(_ApplicationUser, errorMessage);
                }
                return Tuple.Create(_ApplicationUser, "Success");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
