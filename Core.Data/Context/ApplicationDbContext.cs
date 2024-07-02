using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Context
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ItemDropdownListViewModel>().HasNoKey();
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<SMTPEmailSetting> SMTPEmailSetting { get; set; }
        public DbSet<SendGridSetting> SendGridSetting { get; set; }
        public DbSet<DefaultIdentityOptions> DefaultIdentityOptions { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<EmailConfig> EmailConfig { get; set; }


        public DbSet<Designation> Designation { get; set; }
        public DbSet<Department> Department { get; set; } 
        public DbSet<SubDepartment> SubDepartment { get; set; }
        public DbSet<UserInfoFromBrowser> UserInfoFromBrowser { get; set; }
        public DbSet<EmployeeType> EmployeeType { get; set; }
        public DbSet<ManageUserRoles> ManageUserRoles { get; set; }
        public DbSet<ManageUserRolesDetails> ManageUserRolesDetails { get; set; }

        public DbSet<ItemDropdownListViewModel> ItemDropdownListViewModel { get; set; }
    }
}
