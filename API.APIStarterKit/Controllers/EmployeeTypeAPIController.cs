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
    [Authorize(Roles = MainMenu.IdentitySetting.RoleName)]
    public class EmployeeTypeAPIController : ControllerBase
    {
        private readonly IRepository<EmployeeType> _Repository;

        public EmployeeTypeAPIController(IRepository<EmployeeType> Repository)
        {
            _Repository = Repository;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<EmployeeType>>> GetAll()
        {
            var result = await _Repository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<EmployeeType>> GetById(Int64 id)
        {
            var result = await _Repository.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetByName/{EmployeeType}")]
        public async Task<ActionResult<SubDepartment>> GetByName(string SubDepartmentName)
        {
            return Ok(await _Repository.FindByConditionAsync(x => x.Name == SubDepartmentName));
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult> AddEdit([FromBody] EmployeeType model)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            var _UserName = HttpContext.User.Identity.Name;
            try
            {
                if (model.Id > 0)
                {
                    var currentEmployeeType = await _Repository.GetByIdAsync(model.Id);
                    model.CreatedDate = currentEmployeeType.CreatedDate;
                    model.CreatedBy = currentEmployeeType.CreatedBy;
                    model.ModifiedBy = _UserName;
                    _Repository.Update(model, currentEmployeeType);

                    _JsonResultViewModel.IsSuccess = await _Repository.SaveChangesAsync();
                    _JsonResultViewModel.AlertMessage = "Employee Type has been updated successfully. Employee Type Id: " + model.Id;
                    return Ok(_JsonResultViewModel);
                }
                else
                {
                    model.CreatedBy = _UserName;
                    model.ModifiedBy = _UserName;
                    _Repository.Add(model);

                    _JsonResultViewModel.IsSuccess = await _Repository.SaveChangesAsync();
                    _JsonResultViewModel.AlertMessage = "Employee Type has been created successfully. Employee Type Id: " + model.Id;
                    return Ok(_JsonResultViewModel);
                }
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return Ok(_JsonResultViewModel);
            }
        }        
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(Int64 id)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                await _Repository.Delete(id);
                _JsonResultViewModel.IsSuccess = await _Repository.SaveChangesAsync();
                _JsonResultViewModel.AlertMessage = "Employee Type has been deleted successfully. Employee Type Id: " + id;
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
