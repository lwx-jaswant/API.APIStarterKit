using Core.Data.Context;
using Core.Data.Models;
using Core.Data.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.APIStarterKit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = MainMenu.AuditLogs.RoleName)]
    public class AuditLogsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditLogsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<AuditLogs>>> GetAll()
        {
            var result = await _context.AuditLogs.ToListAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<AuditLogs>> GetById(Int64 id)
        {
            var result = await _context.AuditLogs.FirstOrDefaultAsync(m => m.Id == id);
            return Ok(result);
        }
    }
}
