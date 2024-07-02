using API.APIStarterKit.GenericRepo;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.Admin.RoleName)]
    public class UserInfoFromBrowserAPIController : ControllerBase
    {
        private readonly IRepository<UserInfoFromBrowser> _Repository;

        public UserInfoFromBrowserAPIController(IRepository<UserInfoFromBrowser> Repository)
        {
            _Repository = Repository;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<UserInfoFromBrowser>>> GetAll()
        {
            var result = await _Repository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<UserInfoFromBrowser>> GetById(Int64 id)
        {
            var result = await _Repository.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("SaveUserInfoFromBrowser")]
        public async Task<IActionResult> SaveUserInfoFromBrowser(UserInfoFromBrowser model)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            var _UserName = HttpContext.User.Identity.Name;
            try
            {
                model.CreatedBy = _UserName;
                model.ModifiedBy = _UserName;
                _Repository.Add(model);

                _JsonResultViewModel.IsSuccess = await _Repository.SaveChangesAsync();
                _JsonResultViewModel.AlertMessage = "User Info From Browser has been created successfully. User Info From Browser Id: " + model.Id;
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
