using Client.ConsumeAPI.APIClient;
using Core.Data.Models;
using Core.Data.Models.CompanyInfoViewModel;
using Microsoft.AspNetCore.Mvc;
using UserMSDev.Services;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class CompanyInfoController : Controller
    {
        private IAPIClientService<CompanyInfoCRUDViewModel> _iAPIClientService;
        private string SubURL = string.Empty;
        private readonly ICommonService _iCommonService;
        public CompanyInfoController(IAPIClientService<CompanyInfoCRUDViewModel> iAPIClientService, ICommonService iCommonService)
        {
            _iAPIClientService = iAPIClientService;
            SubURL = "CommonDataAPI";
            _iCommonService = iCommonService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SubURL = SubURL + "/GetCompanyInfo/" + 1;
            CompanyInfoCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            SubURL = SubURL + "/GetCompanyInfo/" + 1;
            CompanyInfoCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyInfoCRUDViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (vm.CompanyLogo != null)
                    {
                        vm.Logo = "/upload/" + _iCommonService.UploadedFile(vm.CompanyLogo);
                        vm.CompanyLogo = null;
                    }
                    SubURL = SubURL + "/UpdateCompanyInfo";
                    var result = await _iAPIClientService.AddUpdate(vm, SubURL);
                    TempData["successAlert"] = result.AlertMessage;
                    return RedirectToAction(nameof(Index));
                }
                TempData["errorAlert"] = "Operation failed.";
                return View("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

