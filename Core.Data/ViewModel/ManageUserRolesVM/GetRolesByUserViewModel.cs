using Microsoft.AspNetCore.Identity;

namespace Core.Data.Models.ManageUserRolesVM
{
    public class GetRolesByUserViewModel
    {
        public string ApplicationUserId { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        public List<IdentityRole> listIdentityRole { get; set; }
    }
}

