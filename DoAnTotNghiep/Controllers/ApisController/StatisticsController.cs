using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using DoAnTotNghiep.Services.PaymentServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.ApisController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPaymentService _paymentService;
        public StatisticsController(DataContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
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

        [HttpGet("revenue-statistics")]
        public IActionResult GetRevenueStatistics()
        {
            var revenueStatistics = _paymentService.GetRevenueStatisticsForCurrentMonth();
            return Ok(new { success = true, revenueStatistics });
        }
    }
}
