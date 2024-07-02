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
    [Authorize(Roles = MainMenu.EmailSetting.RoleName)]
    public class EmailSettingAPIController : ControllerBase
    {
        private readonly IRepository<SMTPEmailSetting> _RepositoryGmail;
        private readonly IRepository<SendGridSetting> _RepositorySendGrid;

        public EmailSettingAPIController(IRepository<SMTPEmailSetting> RepositoryGmail, IRepository<SendGridSetting> RepositorySendGrid)
        {
            _RepositoryGmail = RepositoryGmail;
            _RepositorySendGrid = RepositorySendGrid;
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<SMTPEmailSetting>> GetById(Int64 id)
        {
            var result = await _RepositoryGmail.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        [Route("SMTPEmailSettingAddEdit")]
        public async Task<IActionResult> SMTPEmailSettingAddEdit(SMTPEmailSettingViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                SMTPEmailSetting _SMTPEmailSetting = await _RepositoryGmail.GetByIdAsync(vm.Id);
                vm.CreatedDate = _SMTPEmailSetting.CreatedDate;
                vm.CreatedBy = _SMTPEmailSetting.CreatedBy;
                vm.ModifiedDate = DateTime.Now;
                vm.ModifiedBy = HttpContext.User.Identity.Name;

                _RepositoryGmail.Update(vm, _SMTPEmailSetting);
                await _RepositoryGmail.SaveChangesAsync();

                SendGridSetting _SendGridSetting = await _RepositorySendGrid.GetByIdAsync(vm.Id);
                var SGVM = _SendGridSetting;
                if (vm.IsDefault)
                    SGVM.IsDefault = false;
                else
                    SGVM.IsDefault = true;

                _SendGridSetting.ModifiedDate = DateTime.Now;
                _SendGridSetting.ModifiedBy = HttpContext.User.Identity.Name;
                _RepositorySendGrid.Update(SGVM, _SendGridSetting);
                await _RepositoryGmail.SaveChangesAsync();

                _JsonResultViewModel.AlertMessage = "SMTP Info Updated Successfully. User Name: " + _SMTPEmailSetting.UserName;
                _JsonResultViewModel.IsSuccess = true;
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
        [Route("GetByIdSendGrid/{id}")]
        public async Task<ActionResult<SendGridSetting>> GetByIdSendGrid(Int64 id)
        {
            var result = await _RepositorySendGrid.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        [Route("SendGridSettingAddEdit")]
        public async Task<IActionResult> SendGridSettingAddEdit(SendGridSettingViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                SendGridSetting _SendGridSetting = await _RepositorySendGrid.GetByIdAsync(vm.Id);
                vm.CreatedDate = _SendGridSetting.CreatedDate;
                vm.CreatedBy = _SendGridSetting.CreatedBy;
                vm.ModifiedDate = DateTime.Now;
                vm.ModifiedBy = HttpContext.User.Identity.Name;

                _RepositorySendGrid.Update(vm, _SendGridSetting);
                await _RepositoryGmail.SaveChangesAsync();

                SMTPEmailSetting _SMTPEmailSetting = await _RepositoryGmail.GetByIdAsync(vm.Id);
                var GmailVM = _SMTPEmailSetting;
                if (vm.IsDefault)
                    GmailVM.IsDefault = false;
                else
                    GmailVM.IsDefault = true;

                _SendGridSetting.ModifiedDate = DateTime.Now;
                _SendGridSetting.ModifiedBy = HttpContext.User.Identity.Name;
                _RepositoryGmail.Update(GmailVM, _SMTPEmailSetting);
                await _RepositoryGmail.SaveChangesAsync();

                _JsonResultViewModel.AlertMessage = "SendGrid Info Updated Successfully. User Name: " + _SendGridSetting.FromEmail;
                _JsonResultViewModel.IsSuccess = true;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(ex.Message);
                throw;
            }
        }
    }
}
