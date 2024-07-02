using Core.Data.Context;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.ManageUserRolesVM;
using Core.Data.Pages;

namespace API.APIStarterKit.Services
{
    public interface IRolesService
    {
        Task GenerateRolesFromPageList();
        Task<string> CreateSingleRole(string _RoleName);
        Task AddToRoles(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> RolebaseMenuLoad(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> ManageUserRolesDetailsByUser(ApplicationUser _ApplicationUser, ApplicationDbContext _context);
        Task<List<ManageUserRolesViewModel>> GetRolesByUser(GetRolesByUserViewModel vm);
        Task<List<ManageUserRolesViewModel>> GetRoleList();
        Task<JsonResultViewModel> UpdateUserRoles(UpdateUserRolesViewModel vm);
    }
}
