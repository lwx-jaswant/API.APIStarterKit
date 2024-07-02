namespace Core.Data.Models.DashboardViewModel
{
    public class UserMSDevSummaryViewModel
    {
        public int TotalEmployee { get; set; }
        public int TotalUserRole  { get; set; }
        public int TotalDepartment { get; set; }
        public int TotalDesignation { get; set; }

        public int TotalEmployeeType { get; set; }
        public int TotalLeaveApplication { get; set; }
        public List<UserProfile> listUserProfile { get; set; }
    }
}
