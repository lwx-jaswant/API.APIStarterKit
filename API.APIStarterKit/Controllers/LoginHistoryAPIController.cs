using API.APIStarterKit.GenericRepo;
using Core.Data.Models;
using Core.Data.Models.LoginHistoryViewModel;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.LoginHistory.RoleName)]
    public class LoginHistoryAPIController : ControllerBase
    {
        private readonly IRepository<LoginHistory> _Repository;

        public LoginHistoryAPIController(IRepository<LoginHistory> repository)
        {
            _Repository = repository;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<LoginHistoryCRUDViewModel>>> GetAll()
        {
            var result = await GetGridItem();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<LoginHistoryCRUDViewModel>> GetById(Int64 id)
        {
            var _GetByIdAsync = await _Repository.GetByIdAsync(id);
            return Ok(_GetByIdAsync);
        }
        private async Task<IQueryable<LoginHistoryCRUDViewModel>> GetGridItem()
        {
            try
            {
                var _GetAllAsync = await _Repository.GetAllAsync();
                var result = (from _LoginHistory in _GetAllAsync
                              where _LoginHistory.Cancelled == false
                              select new LoginHistoryCRUDViewModel
                              {
                                  Id = _LoginHistory.Id,
                                  UserName = _LoginHistory.UserName,
                                  LoginTimeDisplay = String.Format("{0:f}", _LoginHistory.LoginTime),
                                  LogoutTimeDisplay = String.Format("{0:f}", _LoginHistory.LogoutTime),

                                  Duration = Math.Round(_LoginHistory.Duration, 2),
                                  PublicIP = _LoginHistory.PublicIP,
                                  Latitude = Math.Round(Convert.ToDouble(_LoginHistory.Latitude), 2).ToString(),
                                  Longitude = Math.Round(Convert.ToDouble(_LoginHistory.Longitude), 2).ToString(),
                                  Browser = _LoginHistory.Browser,
                                  OperatingSystem = _LoginHistory.OperatingSystem,
                                  Device = _LoginHistory.Device,
                                  Action = _LoginHistory.Action,
                                  ActionStatus = _LoginHistory.ActionStatus,
                                  CreatedDate = _LoginHistory.CreatedDate,
                              }).OrderByDescending(x => x.Id);

                return result.AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
