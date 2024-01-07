using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.ApisController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly DataContext _context;

        public StatisticsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("account-roles")]
        public IActionResult GetAccountRoleStats()
        {
            var stats = _context.Accounts
                .Where(a => a.AccountRole != AccountRole.Admin)
                .GroupBy(a => a.AccountRole)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToList();

            return Ok(stats);
        }

    }
}
