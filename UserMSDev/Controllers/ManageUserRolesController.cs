using Client.ConsumeAPI.APIClient;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.ManageUserRolesVM;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class ManageUserRolesController : Controller
    {
        private readonly IAPIClientService<ManageUserRolesCRUDViewModel> _iAPIClientService;
        private string SubURL = string.Empty;

        public ManageUserRolesController(IAPIClientService<ManageUserRolesCRUDViewModel> iAPIClientService)
        {
            _iAPIClientService = iAPIClientService;
            SubURL = "ManageUserRolesAPI";
        }

        [HttpGet]
        public IActionResult Index()
        {
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
                List<ManageUserRolesCRUDViewModel> listManageUserRolesCRUDViewModel = await _iAPIClientService.GetAll(SubURL);
                var _GetGridItem = listManageUserRolesCRUDViewModel.AsQueryable();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Int64 id)
        {
            SubURL = SubURL + "/GetByIdDetails/" + id;
            ManageUserRolesCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            return PartialView("_Info", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(Int64 id)
        {
            SubURL = SubURL + "/GetByIdDetails/" + id;
            var vm = await _iAPIClientService.GetById(SubURL);
            return PartialView("_AddEdit", vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(ManageUserRolesCRUDViewModel vm)
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
        [HttpGet]
        public async Task<IActionResult> UpdateUserRoleInUM(Int64 id)
        {
             SubURL = SubURL + "/UpdateUserRoleInUM/" + id;
            var vm = await _iAPIClientService.GetById(SubURL);
            return PartialView("_UpdateRoleInUM", vm);
        }
        [HttpPost]
        public async Task<IActionResult> SaveUpdateUserRoleInUM(ManageUserRolesCRUDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                SubURL = SubURL + "/SaveUpdateUserRoleInUM";
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
