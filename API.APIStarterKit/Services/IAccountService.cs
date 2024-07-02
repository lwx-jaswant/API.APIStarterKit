using Microsoft.AspNetCore.Identity;
using Core.Data.Models;
using Core.Data.Models.AccountViewModels;
using Core.Data.Models.UserProfileViewModel;

namespace API.APIStarterKit.Services
{
    public interface IAccountService
    {
        Task<Tuple<ApplicationUser, IdentityResult>> CreateUserAccount(CreateUserAccountViewModel _CreateUserAccountViewModel);
        Task<Tuple<ApplicationUser, string>> CreateUserProfile(UserProfileCRUDViewModel vm, string LoginUser);       
    }
}
