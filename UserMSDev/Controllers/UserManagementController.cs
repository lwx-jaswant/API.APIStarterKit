using Client.ConsumeAPI.APIClient;
using Core.Data.Helper;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.ManageUserRolesVM;
using Core.Data.Models.UserProfileViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Linq.Dynamic.Core;
using UserMSDev.Services;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class UserManagementController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly IAPIClientService<UserProfileCRUDViewModel> _iAPIClientService;
        private readonly IAPIClientService<UpdateUserRolesViewModel> _iAPIClientServiceUpdateRole;
        private IAPIClientService<DefaultIdentityOptions> _iAPIClientServiceIdentityOptions;
        private readonly IMemoryCache _iMemoryCache;
        private string SubURL = string.Empty;

        public UserManagementController(ICommonService iCommonService, IAPIClientService<UserProfileCRUDViewModel> iAPIClientService, IAPIClientService<UpdateUserRolesViewModel> iAPIClientServiceUpdateRole, IAPIClientService<DefaultIdentityOptions> iAPIClientServiceIdentityOptions, IMemoryCache iMemoryCache)
        {
            _iCommonService = iCommonService;
            _iAPIClientService = iAPIClientService;
            _iAPIClientServiceUpdateRole = iAPIClientServiceUpdateRole;
            _iAPIClientServiceIdentityOptions = iAPIClientServiceIdentityOptions;
            _iMemoryCache = iMemoryCache;
            SubURL = "UserManagementAPI";
        }

        [HttpGet]
        public IActionResult Index()
        {
            MainMenuViewModel _MainMenuViewModel = new();
            _iMemoryCache.TryGetValue("MainMenuViewModel", out _MainMenuViewModel);
            if (!_MainMenuViewModel.UserManagement)
                return RedirectToAction("AccessDenied", "Account");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                SubURL = SubURL + "/GetAll";
                List<UserProfileCRUDViewModel> listUserProfile = await _iAPIClientService.GetAll(SubURL);
                var _AccountUser = listUserProfile.AsQueryable();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _AccountUser = _AccountUser.Where(x => x.Cancelled == false).OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _AccountUser = _AccountUser.Where(obj => obj.FirstName.ToLower().Contains(searchValue)
                    || obj.LastName.ToLower().Contains(searchValue)
                    || obj.PhoneNumber.ToLower().Contains(searchValue)
                    || obj.Email.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _AccountUser.Count();

                var result = _AccountUser.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewUserDetails(Int64 id)
        {
            SubURL = SubURL + "/GetById/" + id;
            UserProfileCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            return PartialView("_ViewUserDetails", vm);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditUserAccount(Int64 id)
        {
            SubURL = SubURL + "/GetEditData/" + id;
            var vm = await _iAPIClientService.GetById(SubURL);

            ViewBag.ddlEmployeeType = new SelectList(vm.UserProfileDropDownView.listEmployeeType, "Id", "Name");
            ViewBag.ddlManageUserRoles = new SelectList(vm.UserProfileDropDownView.listManageUserRoles, "Id", "Name");
            ViewBag.ddlDepartment = new SelectList(vm.UserProfileDropDownView.listDesignation, "Id", "Name");
            ViewBag.ddlSubDepartment = new SelectList(vm.UserProfileDropDownView.listSubDepartment, "Id", "Name");
            ViewBag.ddlDesignation = new SelectList(vm.UserProfileDropDownView.listDesignation, "Id", "Name");

            UserProfileCRUDViewModel _UserProfileCRUDViewModel = new();
            if (id > 0)
            {
                _UserProfileCRUDViewModel = vm;
                return PartialView("_EditUserAccount", _UserProfileCRUDViewModel);
            }
            _UserProfileCRUDViewModel.EmployeeId = "EMP-" + StaticData.RandomDigits(6);
            return PartialView("_AddUserAccount", _UserProfileCRUDViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAddEditUserAccount(UserProfileCRUDViewModel _UserProfileViewModel)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var AddEditSubURL = SubURL + "/AddEdit";
                if (_UserProfileViewModel.Id > 0)
                {
                    if (_UserProfileViewModel.ProfilePictureDetails != null)
                    {
                        _UserProfileViewModel.ProfilePicture = "/upload/" + _iCommonService.UploadedFile(_UserProfileViewModel.ProfilePictureDetails);
                        _UserProfileViewModel.ProfilePictureDetails = null;
                    }
                    else
                    {
                        _UserProfileViewModel.IsProfilePictureDetailsNull = true;
                    }
                    _JsonResultViewModel = await _iAPIClientService.AddUpdate(_UserProfileViewModel, AddEditSubURL);
                }
                else
                {
                    if (_UserProfileViewModel.ProfilePictureDetails != null)
                    {
                        _UserProfileViewModel.ProfilePicture = "/upload/" + _iCommonService.UploadedFile(_UserProfileViewModel.ProfilePictureDetails);
                        _UserProfileViewModel.ProfilePictureDetails = null;
                    }
                    string _ClientBaseURL = string.Empty;
                    _iMemoryCache.TryGetValue("ClientBaseURL", out _ClientBaseURL);
                    _UserProfileViewModel.ClientBaseURL = _ClientBaseURL;
                    _JsonResultViewModel = await _iAPIClientService.AddUpdate(_UserProfileViewModel, AddEditSubURL);
                }
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordAdmin(Int64 id)
        {
            SubURL = SubURL + "/GetById/" + id;
            UserProfileCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            ResetPasswordAdminVM _ResetPasswordAdminVM = new()
            {
                ApplicationUserId = vm.ApplicationUserId
            };
            return PartialView("_ResetPasswordAdmin", _ResetPasswordAdminVM);
        }
    }
}
