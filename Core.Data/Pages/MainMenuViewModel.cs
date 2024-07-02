namespace Core.Data.Pages
{
    public class MainMenuViewModel
    {
        public bool Admin { get; set; }
        public bool SuperAdmin { get; set; }
        public bool Settings { get; set; }

        public bool Dashboard { get; set; }
        public bool DashboardGeneral { get; set; }
        public bool UserManagement { get; set; }
        public bool UserInfoFromBrowser { get; set; }
        public bool AuditLogs { get; set; }
        public bool UserProfile { get; set; }
        public bool SystemRole { get; set; }
        public bool EmailSetting { get; set; }
        public bool IdentitySetting { get; set; }
        public bool LoginHistory { get; set; }
        public bool CompanyInfo { get; set; }
        public bool EmailConfig { get; set; }


        //UMS
        public bool EmployeeType { get; set; }
        public bool Department { get; set; }
        public bool SubDepartment { get; set; }
        public bool Designation { get; set; }
        public bool ManageUserRoles { get; set; }
    }
}