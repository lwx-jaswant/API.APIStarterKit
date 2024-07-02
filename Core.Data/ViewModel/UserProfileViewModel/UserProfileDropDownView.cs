namespace Core.Data.Models.UserProfileViewModel
{
    public class UserProfileDropDownView
    {
        public UserProfileCRUDViewModel UserProfileCRUDViewModel { get; set; } 
        public List<EmployeeType> listEmployeeType { get; set; }   
        public List<ManageUserRoles> listManageUserRoles { get; set; }   
        public List<Department> listDepartment { get; set; }   
        public List<SubDepartment> listSubDepartment { get; set; }   
        public List<Designation> listDesignation { get; set; }   
    }
}
