using System.ComponentModel.DataAnnotations;

namespace Core.Data.Models.ManageUserRolesVM
{
    public class AddNewRoleViewModel
    {
        [Display(Name = "Role Name"), Required]
        public string RoleName { get; set; }
    }
}

