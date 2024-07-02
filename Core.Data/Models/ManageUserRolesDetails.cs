using System;

namespace Core.Data.Models
{
    public class ManageUserRolesDetails : EntityBase
    {
        
        public Int64 ManageRoleId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAllowed { get; set; }      
    }
}
