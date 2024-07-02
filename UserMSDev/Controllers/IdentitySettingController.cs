using Client.ConsumeAPI.APIClient;
using Core.Data.Models;
using Core.Data.Models.CommonViewModel;
using Microsoft.AspNetCore.Mvc;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class IdentitySettingController : Controller
    {
        private IAPIClientService<DefaultIdentityOptions> _iAPIClientService;
        private string SubURL = string.Empty;
        public IdentitySettingController(IAPIClientService<DefaultIdentityOptions> iAPIClientService)
        {
            _iAPIClientService = iAPIClientService;
            SubURL = "IdentitySettingAPI";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SubURL = SubURL + "/GetById/" + 1;
            DefaultIdentityOptionsCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            SubURL = SubURL + "/GetById/" + 1;
            DefaultIdentityOptionsCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            return PartialView("_Edit", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DefaultIdentityOptions vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                SubURL = SubURL + "/AddEdit";
                _JsonResultViewModel = await _iAPIClientService.AddUpdate(vm, SubURL);
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

