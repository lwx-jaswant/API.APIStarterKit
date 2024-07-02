using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.EmailConfigViewModel;
using Core.Data.Models.ManageUserRolesVM;
using Core.Data.Models.SubDepartmentViewModel;
using Core.Data.Models.UserProfileViewModel;

namespace API.APIStarterKit.Services
{
    public interface ICommonService
    {
        string UploadedFile(IFormFile ProfilePicture);
        string UploadedFile(IFormFile _IFormFile, string CreatedDirName);
        string GetContentPath(string DBFilePath);
        string GetServerFileDir();
        Task<SMTPEmailSetting> GetSMTPEmailSetting();
        Task<SendGridSetting> GetSendGridEmailSetting();
        Task<EmailConfig> GetEmailConfig();
        UserProfile GetByUserProfile(Int64 id);
        IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails();     
        Task<List<ManageUserRolesViewModel>> GetManageRoleDetailsList(Int64 id);
        

        IQueryable<ItemDropdownListViewModel> GetCommonddlData(string strTableName);
        IEnumerable<T> GetDropDownListData<T>() where T : class;
        IQueryable<ItemDropdownListViewModel> LoadddlUserProfile();

        
        IQueryable<ItemDropdownListViewModel> LoadddlDepartment();
        IQueryable<ItemDropdownListViewModel> LoadddlSubDepartment();
        IQueryable<ItemDropdownListViewModel> LoadddlEmployee();
        IQueryable<ItemDropdownListViewModel> LoadddlDesignation();
        Task<Int64> GetLoginEmployeeId(string _UserEmail);
        Task<List<ManageUserRolesDetails>> GetByManageUserRolesDetails(Int64 _RoleId);
        IQueryable<SubDepartmentCRUDViewModel> GetSubDepartmentGridItem();
        IQueryable<EmailConfigCRUDViewModel> GetEmailConfigGridItem();
    }
}
