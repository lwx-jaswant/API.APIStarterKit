using API.APIStarterKit.GenericRepo;
using API.APIStarterKit.Services;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.SubDepartmentViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.SubDepartment.RoleName)]
    public class SubDepartmentAPIController : ControllerBase
    {
        private readonly IRepository<SubDepartment> _Repository;
        private readonly ICommonService _iCommonService;

        public SubDepartmentAPIController(IRepository<SubDepartment> repository, ICommonService iCommonService)
        {
            _Repository = repository;
            _iCommonService = iCommonService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<SubDepartment>>> GetAll()
        {
            var result = await _Repository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetGridData")]
        public ActionResult<IEnumerable<SubDepartmentCRUDViewModel>> GetGridData()
        {
            var result = _iCommonService.GetSubDepartmentGridItem();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetByGridData/{id}")]
        public ActionResult<IEnumerable<SubDepartmentCRUDViewModel>> GetByGridData(Int64 id)
        {
            var result = _iCommonService.GetSubDepartmentGridItem().Where(x => x.Id == id).SingleOrDefault();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<SubDepartment>> GetById(Int64 id)
        {
            return Ok(await _Repository.GetByIdAsync(id));
        }
        [HttpGet]
        [Route("GetByName/{SubDepartmentName}")]
        public async Task<ActionResult<SubDepartment>> GetByName(string SubDepartmentName)
        {
            return Ok(await _Repository.FindByConditionAsync(x => x.Name == SubDepartmentName));
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult> AddEdit([FromBody] SubDepartment model)
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
                    _JsonResultViewModel.AlertMessage = "Sub Department has been updated successfully. Sub Department Id: " + model.Id;
                    return Ok(_JsonResultViewModel);
                }
                else
                {
                    model.CreatedBy = _UserName;
                    model.ModifiedBy = _UserName;
                    _Repository.Add(model);

                    _JsonResultViewModel.IsSuccess = await _Repository.SaveChangesAsync();
                    _JsonResultViewModel.AlertMessage = "Sub Department has been created successfully. Sub Department Id: " + model.Id;
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
                _JsonResultViewModel.AlertMessage = "Sub Department has been deleted successfully. Sub Department Id: " + id;
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
        [HttpGet]
        [Route("GetDropDownData")]
        public ActionResult<List<Department>> GetDropDownData()
        {
            var result = _iCommonService.GetDropDownListData<Department>().ToList();
            return Ok(result);
        }
    }
}
