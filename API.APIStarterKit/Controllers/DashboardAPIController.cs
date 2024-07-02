using Core.Data.Context;
using Core.Data.Models;
using Core.Data.Models.DashboardViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.Dashboard.RoleName)]
    public class DashboardAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DashboardAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetDashboardData")]
        public async Task<ActionResult<IEnumerable<UserMSDevSummaryViewModel>>> GetDashboardData()
        {
           var result = await GetSummaryData();
            return Ok(result);
        }
       private async Task<UserMSDevSummaryViewModel> GetSummaryData()
        {
            UserMSDevSummaryViewModel vm = new();
            try
            {
                vm.TotalEmployee = GetTableData<UserProfile>().Where(x => x.Cancelled == false).Count();
                vm.TotalUserRole = GetTableData<ManageUserRoles>().Where(x => x.Cancelled == false).Count();
                vm.TotalDepartment = GetTableData<Department>().Where(x => x.Cancelled == false).Count();
                vm.TotalDesignation = GetTableData<Designation>().Where(x => x.Cancelled == false).Count();

                vm.TotalEmployeeType = GetTableData<EmployeeType>().Where(x => x.Cancelled == false).Count();
                vm.listUserProfile = await _context.UserProfile.Where(x => x.Cancelled == false).OrderByDescending(x => x.CreatedDate).Take(10).ToListAsync();
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<T> GetTableData<T>() where T : class
        {
            return _context.Set<T>().ToList();
        }
    }
}
