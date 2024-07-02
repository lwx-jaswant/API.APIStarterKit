using Client.ConsumeAPI.APIClient;
using Core.Data.Context;
using Core.Data.Models.LoginHistoryViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using UserMSDev.Services;

namespace UserMSDev.Controllers
{
    [Route("[controller]/[action]")]
    public class LoginHistoryController : Controller
    {
        private readonly IAPIClientService<LoginHistoryCRUDViewModel> _iAPIClientService;
        private string SubURL = string.Empty;

        public LoginHistoryController(IAPIClientService<LoginHistoryCRUDViewModel> iAPIClientService)
        {
            _iAPIClientService = iAPIClientService;
            SubURL = "LoginHistoryAPI";
        }
        [HttpGet]
        public IActionResult Index()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"];
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
                List<LoginHistoryCRUDViewModel> listLoginHistoryCRUDViewModel = await _iAPIClientService.GetAll(SubURL);
                var _GetGridItem = listLoginHistoryCRUDViewModel.AsQueryable();
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
                    || obj.UserName.ToLower().Contains(searchValue)
                    || obj.Duration.ToString().ToLower().Contains(searchValue)
                    || obj.PublicIP.ToLower().Contains(searchValue)

                    || obj.Latitude.ToLower().Contains(searchValue)
                    || obj.Longitude.ToLower().Contains(searchValue)
                    || obj.Browser.ToLower().Contains(searchValue)
                    || obj.OperatingSystem.ToLower().Contains(searchValue)
                    || obj.Device.ToLower().Contains(searchValue)
                    || obj.Action.ToLower().Contains(searchValue)
                    || obj.ActionStatus.ToLower().Contains(searchValue)
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
            SubURL = SubURL + "/GetById/" + id;
            LoginHistoryCRUDViewModel vm = await _iAPIClientService.GetById(SubURL);
            return PartialView("_Details", vm);
        }
    }
}
