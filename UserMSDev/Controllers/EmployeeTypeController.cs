using Client.ConsumeAPI.APIClient;
using Core.Data.Models;
using Core.Data.Models.EmployeeTypeViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class EmployeeTypeController : Controller
    {
        private IAPIClientService<EmployeeType> _iAPIClientService;
        private string SubURL = string.Empty;
        public EmployeeTypeController(IAPIClientService<EmployeeType> iAPIClientService)
        {
            _iAPIClientService = iAPIClientService;
            SubURL = "EmployeeTypeAPI";
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

                string SubURL = "EmployeeTypeAPI/GetAll";
                List<EmployeeType> listEmployeeType = await _iAPIClientService.GetAll(SubURL);
                var _GetGridItem = listEmployeeType.AsQueryable();

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
        public async Task<IActionResult> Details(long id)
        {
            SubURL = SubURL + "/GetByGridData/" + id;
            EmployeeTypeCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            EmployeeTypeCRUDViewModel vm = new();
            if (id > 0)
            {
                SubURL = SubURL + "/GetById/" + id;
                vm = await _iAPIClientService.GetById(SubURL);
            }
            return PartialView("_AddEdit", vm);
        }
    }
}
