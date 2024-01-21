using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using DoAnTotNghiep.Repository.EmployerRepo;
using DoAnTotNghiep.Services.PaymentServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace DoAnTotNghiep.Controllers.ApisController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPaymentService _paymentService;
        private readonly IEmployerRepository _employerRepository;
        public StatisticsController(DataContext context, IPaymentService paymentService, IEmployerRepository employerRepository)
        {
            _employerRepository= employerRepository;
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

        [HttpGet("top5employers")]
        public async Task<ActionResult<List<Employer>>> GetTop5EmployersWithJobCount()
        {
            try
            {
                var top5Employers = await _employerRepository.GetTop5EmployersWithJobCount();
                return Ok(top5Employers);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
