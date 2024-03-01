using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.PayRepo;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageDashbroadController : ManageBaseController
    {
        private readonly DataContext _dataContext;
        private readonly IPayRepository _payRepository;
        public ManageDashbroadController(DataContext dataContext, IPayRepository payRepository)
        {
            _dataContext= dataContext;
            _payRepository= payRepository;
        }
        public IActionResult Index()
        {
            int currentYear = DateTime.Now.Year;
            ViewBag.CurrentYear = currentYear;
            ViewBag.Employer = _dataContext.Employers.Count();
            ViewBag.Cadidates = _dataContext.Candidates.Count();
            ViewBag.Job = _dataContext.JobPostings.Count();
            ViewBag.Disscus = _dataContext.Discusses.Count();
            return View();
        }

        public IActionResult GetRevenueData(int selectedYear)
        {
            var revenueData = _dataContext.RevenueStatistics
                .Where(r => r.Date.Year == selectedYear)
                .OrderBy(r => r.Date)
                .Select(r => new { Date = r.Date.ToString("yyyy-MM-dd"), Amount = r.Amount })
                .ToList();

            return Json(revenueData);
        }
         
        public async Task<IActionResult> PaymentDashbroad()
        {
            var allPayments = await _payRepository.GetAllPaymentsAsync();
            return View(allPayments);
        }

    }
}
