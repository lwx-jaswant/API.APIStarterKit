using API.APIStarterKit.GenericRepo;
using API.APIStarterKit.Services;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.EmailConfigViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.EmailConfig.RoleName)]
    public class EmailConfigAPIController : ControllerBase
    {
        private readonly IRepository<EmailConfig> _Repository;
        private readonly ICommonService _iCommonService;

        public EmailConfigAPIController(IRepository<EmailConfig> repository, ICommonService iCommonService)
        {
            _Repository = repository;
            _iCommonService = iCommonService;
        }
        [HttpGet]
        [Route("GetGridData")]
        public ActionResult<IEnumerable<EmailConfigCRUDViewModel>> GetGridData()
        {
            var result = _iCommonService.GetEmailConfigGridItem();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetByGridData/{id}")]
        public ActionResult<IEnumerable<EmailConfigCRUDViewModel>> GetByGridData(Int64 id)
        {
            var result = _iCommonService.GetEmailConfigGridItem().Where(x => x.Id == id).SingleOrDefault();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<EmailConfig>>> GetAll()
        {
            var result = await _Repository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<EmailConfig>> GetById(Int64 id)
        {
            return Ok(await _Repository.GetByIdAsync(id));
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult> AddEdit([FromBody] EmailConfigCRUDViewModel _EmailConfigCreateUpdateVM)
        {
            EmailConfig model = _EmailConfigCreateUpdateVM;
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
                    _JsonResultViewModel.AlertMessage = "Email Config has been updated successfully. Email Config Id: " + model.Id;
                    return Ok(_JsonResultViewModel);
                }
                else
                {
                    var result = await _Repository.FindByConditionAsync(x => x.Email == model.Email);
                    if (result != null)
                    {
                        _JsonResultViewModel.IsSuccess = false;
                        _JsonResultViewModel.AlertMessage = "Email Already Exist.";
                        return Ok(_JsonResultViewModel);
                    }

                    model.CreatedBy = _UserName;
                    model.ModifiedBy = _UserName;
                    _Repository.Add(model);

                    _JsonResultViewModel.IsSuccess = await _Repository.SaveChangesAsync();
                    _JsonResultViewModel.AlertMessage = "Email Config has been created successfully. Email Config Id: " + model.Id;
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
                _JsonResultViewModel.AlertMessage = "Email Config has been deleted successfully. Email Config Id: " + id;
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
