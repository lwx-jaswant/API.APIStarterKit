using Core.Data.Models.CommonViewModel;
using Microsoft.AspNetCore.Mvc;
using Client.ConsumeAPI.APIClient;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class EmailSettingController : Controller
    {
        private IAPIClientService<SMTPEmailSettingViewModel> _iAPIClientServiceGmail;
        private IAPIClientService<SendGridSettingViewModel> _iAPIClientServiceSendGrid;
        private string SubURL = string.Empty;
        public EmailSettingController(IAPIClientService<SMTPEmailSettingViewModel> iAPIClientServiceGmail, IAPIClientService<SendGridSettingViewModel> iAPIClientServiceSendGrid)
        {
            _iAPIClientServiceGmail = iAPIClientServiceGmail;
            _iAPIClientServiceSendGrid = iAPIClientServiceSendGrid;
            SubURL = "EmailSettingAPI";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            EmailSettingViewModel vm = new();
            var SubURLGmail = SubURL + "/GetById/" + 1;
            SMTPEmailSettingViewModel _SMTPEmailSettingViewModel = await _iAPIClientServiceGmail.GetById(SubURLGmail);
            vm.SMTPEmailSettingViewModel = _SMTPEmailSettingViewModel;
            
            var SubURLSendGrid = SubURL + "/GetByIdSendGrid/" + 1;
            SendGridSettingViewModel _SendGridSettingViewModel = await _iAPIClientServiceSendGrid.GetById(SubURLSendGrid);
            vm.SendGridSettingViewModel = _SendGridSettingViewModel;
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> SMTPEmailSettingDetails(Int64 id)
        {
            SubURL = SubURL + "/GetById/" + id;
            SMTPEmailSettingViewModel vm = await _iAPIClientServiceGmail.GetById(SubURL);
            return PartialView("_SMTPEmailSettingDetails", vm);
        }
        [HttpGet]
        public async Task<IActionResult> SMTPEmailSettingAddEdit(Int64 id)
        {
            SubURL = SubURL + "/GetById/" + id;
            SMTPEmailSettingViewModel vm = await _iAPIClientServiceGmail.GetById(SubURL);
            return PartialView("_SMTPEmailSettingAddEdit", vm);
        }

        [HttpPost]
        public async Task<JsonResult> SMTPEmailSettingAddEdit(SMTPEmailSettingViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                SubURL = SubURL + "/SMTPEmailSettingAddEdit";
                _JsonResultViewModel = await _iAPIClientServiceGmail.AddUpdate(vm, SubURL);
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
        public async Task<IActionResult> SendGridSettingDetails(Int64 id)
        {
            SubURL = SubURL + "/GetByIdSendGrid/" + id;
            SendGridSettingViewModel vm = await _iAPIClientServiceSendGrid.GetById(SubURL);
            return PartialView("_SendGridSettingDetails", vm);
        }
        [HttpGet]
        public async Task<IActionResult> SendGridSettingAddEdit(Int64 id)
        {
            SubURL = SubURL + "/GetByIdSendGrid/" + id;
            SendGridSettingViewModel vm = await _iAPIClientServiceSendGrid.GetById(SubURL);
            return PartialView("_SendGridSettingAddEdit", vm);
        }

        [HttpPost]
        public async Task<JsonResult> SendGridSettingAddEdit(SendGridSettingViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                SubURL = SubURL + "/SendGridSettingAddEdit";
                _JsonResultViewModel = await _iAPIClientServiceSendGrid.AddUpdate(vm, SubURL);
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