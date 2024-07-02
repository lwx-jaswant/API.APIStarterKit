using API.APIStarterKit.GenericRepo;
using API.APIStarterKit.Services;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.ManageUserRolesVM;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.SystemRole.RoleName)]
    public class SystemRoleAPIController : ControllerBase
    {
        private readonly IRepository<ManageUserRoles> _Repository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolesService _iRolesService;

        public SystemRoleAPIController(IRepository<ManageUserRoles> Repository, RoleManager<IdentityRole> roleManager, IRolesService iRolesService)
        {
            _Repository = Repository;
            _roleManager = roleManager;
            _iRolesService = iRolesService;
        }
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<IEnumerable<ManageUserRolesViewModel>> GetAll()
        {
            List<ManageUserRolesViewModel> list = new List<ManageUserRolesViewModel>();
            try
            {
                var result = _roleManager.Roles.OrderBy(x => x.Name).ToList();
                int Count = 1;
                foreach (var role in result)
                {
                    var userRolesViewModel = new ManageUserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleId_SL = Count,
                        RoleName = role.Name
                    };
                    list.Add(userRolesViewModel);
                    Count++;
                }
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<ManageUserRoles>> GetById(Int64 id)
        {
            var result = await _Repository.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] ManageUserRolesViewModel model)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var _CreateSingleRole = await _iRolesService.CreateSingleRole(model.RoleName);
                _JsonResultViewModel.AlertMessage = _CreateSingleRole;

                if (_CreateSingleRole.Contains("Created"))
                    _JsonResultViewModel.IsSuccess = true;
                else
                    _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return Ok(_JsonResultViewModel);
            }
        }        
        [HttpDelete]
        [Route("Delete/{RoleId}")]
        public async Task<IActionResult> Delete(string RoleId)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var role = await _roleManager.FindByIdAsync(RoleId);
                var result = await _roleManager.DeleteAsync(role);
                _JsonResultViewModel.AlertMessage = "User Roles has been deleted successfully. Role Id: " + RoleId;
                _JsonResultViewModel.IsSuccess = true;
                return Ok(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return Ok(_JsonResultViewModel);
            }
        }
    }
}
