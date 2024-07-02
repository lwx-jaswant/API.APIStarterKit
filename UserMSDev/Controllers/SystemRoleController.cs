using Client.ConsumeAPI.APIClient;
using Core.Data.Models.CommonViewModel;
using Core.Data.Models.ManageUserRolesVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class SystemRoleController : Controller
    {
        private readonly IAPIClientService<ManageUserRolesViewModel> _iAPIClientService;
        private string SubURL = string.Empty;

        public SystemRoleController(IAPIClientService<ManageUserRolesViewModel> iAPIClientService)
        {
            _iAPIClientService = iAPIClientService;
            SubURL = "SystemRoleAPI";
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
                List<ManageUserRolesViewModel> listManageUserRolesCRUDViewModel = await _iAPIClientService.GetAll(SubURL);
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
                    _GetGridItem = _GetGridItem.Where(obj => obj.RoleId_SL.ToString().Contains(searchValue)
                    || obj.RoleName.ToLower().Contains(searchValue));
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
        public IActionResult AddNewRole()
        {
            return PartialView("_AddNewRole");
        }
        [HttpPost]
        public async Task<IActionResult> SaveAddNewRole(ManageUserRolesViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                SubURL = SubURL + "/AddRole";
                var result = await _iAPIClientService.AddUpdate(vm, SubURL);
                return new JsonResult(result);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}