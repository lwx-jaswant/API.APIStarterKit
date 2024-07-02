using Core.Data.Context;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.EmailConfigViewModel;
using Core.Data.Models.ManageUserRolesVM;
using Core.Data.Models.SubDepartmentViewModel;
using Core.Data.Models.UserProfileViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.APIStarterKit.Services
{
    public class CommonService : ICommonService
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        public CommonService(IWebHostEnvironment iHostingEnvironment,
            ApplicationDbContext context)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
        }
        public string UploadedFile(IFormFile _IFormFile)
        {
            try
            {
                string FileName = null;
                if (_IFormFile != null)
                {
                    string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");
                    if (_FileServerDir.Contains("\n"))
                    {
                        _FileServerDir.Replace("\n", "/");
                    }

                    if (_IFormFile.FileName == null)
                        FileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                    else
                        FileName = Guid.NewGuid().ToString() + "_" + _IFormFile.FileName;

                    string filePath = Path.Combine(_FileServerDir, FileName);
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    using (var fileStream = new FileStream(Path.Combine(_FileServerDir, FileName), FileMode.Create))
                    {
                        _IFormFile.CopyTo(fileStream);
                    }
                }
                return FileName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string UploadedFile(IFormFile _IFormFile, string CreatedDirName)
        {
            string FileName = null;
            if (_IFormFile != null)
            {
                string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/" + CreatedDirName);
                if (_IFormFile.FileName == null)
                    FileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                else
                    FileName = Guid.NewGuid().ToString() + "_" + _IFormFile.FileName;

                string filePath = Path.Combine(_FileServerDir, FileName);
                using (var _FileStream = new FileStream(filePath, FileMode.Create))
                {
                    _IFormFile.CopyTo(_FileStream);
                }
            }
            return FileName;
        }
        public string GetContentPath(string DBFilePath)
        {
            string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot");
            var _CombinePath = _FileServerDir + DBFilePath;
            return _CombinePath;
        }
        public string GetServerFileDir()
        {
            string _Path = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");
            return _Path;
        }
        public async Task<SMTPEmailSetting> GetSMTPEmailSetting()
        {
            return await _context.Set<SMTPEmailSetting>().Where(x => x.Id == 1).SingleOrDefaultAsync();
        }
        public async Task<SendGridSetting> GetSendGridEmailSetting()
        {
            return await _context.Set<SendGridSetting>().Where(x => x.Id == 1).SingleOrDefaultAsync();
        }
        public async Task<EmailConfig> GetEmailConfig()
        {
            return await _context.Set<EmailConfig>().Where(x => x.IsDefault == true).SingleOrDefaultAsync();
        }

        public UserProfile GetByUserProfile(Int64 id)
        {
            var _UserProfile = _context.UserProfile.Where(x => x.Id == id).SingleOrDefault();
            return _UserProfile;
        }
        public IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails()
        {
            var result = (from vm in _context.UserProfile
                          join _EmployeeType in _context.EmployeeType on vm.EmployeeTypeId equals _EmployeeType.Id
                          into _EmployeeType
                          from objEmployeeType in _EmployeeType.DefaultIfEmpty()

                          join _Department in _context.Department on vm.Department equals _Department.Id
                          into _Department
                          from objDepartment in _Department.DefaultIfEmpty()
                          join _SubDepartment in _context.SubDepartment on vm.SubDepartment equals _SubDepartment.Id
                          into _SubDepartment
                          from objSubDepartment in _SubDepartment.DefaultIfEmpty()
                          join _Designation in _context.Designation on vm.Designation equals _Designation.Id
                          into _Designation
                          from objDesignation in _Designation.DefaultIfEmpty()

                          join _ManageRole in _context.ManageUserRoles on vm.RoleId equals _ManageRole.Id
                          into _ManageRole
                          from objManageRole in _ManageRole.DefaultIfEmpty()
                          where vm.Cancelled == false
                          select new UserProfileCRUDViewModel
                          {
                              Id = vm.Id,
                              ApplicationUserId = vm.ApplicationUserId,
                              EmployeeId = vm.EmployeeId,
                              FirstName = vm.FirstName,
                              LastName = vm.LastName,
                              EmployeeTypeId = vm.EmployeeTypeId,
                              EmployeeTypeDisplay = objEmployeeType.Name,
                              DateOfBirth = vm.DateOfBirth,
                              Designation = vm.Designation,
                              DesignationDisplay = objDesignation.Name,
                              Department = vm.Department,
                              DepartmentDisplay = objDepartment.Name,
                              SubDepartment = vm.SubDepartment,
                              SubDepartmentDisplay = objSubDepartment.Name,
                              JoiningDate = vm.JoiningDate,
                              LeavingDate = vm.LeavingDate,
                              PhoneNumber = vm.PhoneNumber,
                              Email = vm.Email,
                              Address = vm.Address,
                              Country = vm.Country,
                              ProfilePicture = vm.ProfilePicture,
                              RoleId = vm.RoleId,
                              RoleIdDisplay = objManageRole.Name,
                              IsApprover = vm.IsApprover,
                              IsApproverDisplay = vm.IsApprover == 1 ? "No" : "Yes",
                              CreatedDate = vm.CreatedDate,
                              ModifiedDate = vm.ModifiedDate,
                              CreatedBy = vm.CreatedBy,
                              ModifiedBy = vm.ModifiedBy,
                              Cancelled = vm.Cancelled,
                          }).OrderByDescending(x => x.Id);
            return result;
        }
        public static async Task<string> GetPublicIP()
        {
            try
            {
                string url = "http://checkip.dyndns.org/";
                var _HttpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
                var _GetAsync = await _HttpClient.GetAsync(url);
                var _Stream = await _GetAsync.Content.ReadAsStreamAsync();
                StreamReader _StreamReader = new StreamReader(_Stream);
                string result = _StreamReader.ReadToEnd();

                string[] a = result.Split(':');
                string a2 = a[1].Substring(1);
                string[] a3 = a2.Split('<');
                string a4 = a3[0];
                return a4;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
        }
        public async Task<List<ManageUserRolesViewModel>> GetManageRoleDetailsList(Int64 id)
        {
            var result = await (from tblObj in _context.ManageUserRolesDetails.Where(x => x.ManageRoleId == id)
                                select new ManageUserRolesViewModel
                                {
                                    ManageRoleDetailsId = tblObj.Id,
                                    RoleId = tblObj.RoleId,
                                    RoleName = tblObj.RoleName,
                                    IsAllowed = tblObj.IsAllowed,
                                }).OrderBy(x => x.RoleName).ToListAsync();
            return result;
        }

        public IEnumerable<T> GetTableData<T>() where T : class
        {
            return _context.Set<T>();
        }
        public IQueryable<ItemDropdownListViewModel> GetCommonddlData(string strTableName)
        {
            var sql = "select Id, Name from " + strTableName + " where Cancelled = 0";
            var result = _context.ItemDropdownListViewModel.FromSqlRaw(sql);
            return result;
        }
        public IEnumerable<T> GetDropDownListData<T>() where T : class
        {
            return _context.Set<T>();
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlUserProfile()
        {
            return (from tblObj in _context.UserProfile.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.FirstName + " " + tblObj.LastName,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlDepartment()
        {
            return (from tblObj in _context.Department.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlSubDepartment()
        {
            return (from tblObj in _context.SubDepartment.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlEmployee()
        {
            var result = (from tblObj in _context.UserProfile.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                          select new ItemDropdownListViewModel
                          {
                              Id = tblObj.Id,
                              Name = tblObj.FirstName + " " + tblObj.LastName,
                          });
            return result;
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlDesignation()
        {
            var result = (from tblObj in _context.Designation.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                          select new ItemDropdownListViewModel
                          {
                              Id = tblObj.Id,
                              Name = tblObj.Name,
                          });
            return result;
        }
        public async Task<Int64> GetLoginEmployeeId(string _UserEmail)
        {
            Int64 _UserProfileId = 0;
            var _ApplicationUser = await _context.ApplicationUser.Where(x => x.Email == _UserEmail).SingleOrDefaultAsync();
            if (_ApplicationUser != null)
            {
                _UserProfileId = _context.UserProfile.Where(x => x.ApplicationUserId == _ApplicationUser.Id).SingleOrDefault().Id;
            }
            return _UserProfileId;
        }
        public async Task<List<ManageUserRolesDetails>> GetByManageUserRolesDetails(Int64 _RoleId)
        {
            var result = await _context.ManageUserRolesDetails.Where(x => x.ManageRoleId == _RoleId && x.IsAllowed == true).ToListAsync();
            return result;
        }
        public IQueryable<SubDepartmentCRUDViewModel> GetSubDepartmentGridItem()
        {
            try
            {
                return (from _SubDepartment in _context.SubDepartment
                        join _Department in _context.Department on _SubDepartment.DepartmentId equals _Department.Id
                        where _SubDepartment.Cancelled == false
                        select new SubDepartmentCRUDViewModel
                        {
                            Id = _SubDepartment.Id,
                            DepartmentId = _SubDepartment.DepartmentId,
                            DepartmentDisplay = _Department.Name,
                            Name = _SubDepartment.Name,
                            Description = _SubDepartment.Description,
                            CreatedDate = _SubDepartment.CreatedDate,
                            ModifiedDate = _SubDepartment.ModifiedDate,
                            CreatedBy = _SubDepartment.CreatedBy,
                            ModifiedBy = _SubDepartment.ModifiedBy,
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<EmailConfigCRUDViewModel> GetEmailConfigGridItem()
        {
            try
            {
                return (from _EmailConfig in _context.EmailConfig
                        where _EmailConfig.Cancelled == false
                        select new EmailConfigCRUDViewModel
                        {
                            Id = _EmailConfig.Id,
                            Email = _EmailConfig.Email,
                            Password = _EmailConfig.Password,
                            Hostname = _EmailConfig.Hostname,
                            Port = _EmailConfig.Port,
                            SSLEnabled = _EmailConfig.SSLEnabled,
                            SenderFullName = _EmailConfig.SenderFullName,
                            IsDefaultDisplay = _EmailConfig.IsDefault == true ? "Yes" : "No",
                            CreatedDate = _EmailConfig.CreatedDate,
                            ModifiedDate = _EmailConfig.ModifiedDate,
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception) { throw; }
        }
    }
}