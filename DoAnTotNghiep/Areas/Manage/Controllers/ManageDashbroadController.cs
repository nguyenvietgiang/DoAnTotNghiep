using DoAnTotNghiep.Models.EntityModels;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageDashbroadController : ManageBaseController
    {
        private readonly DataContext _dataContext;

        public ManageDashbroadController(DataContext dataContext)
        {
            _dataContext= dataContext;
        }
        public IActionResult Index()
        {
            ViewBag.Employer = _dataContext.Employers.Count();
            ViewBag.Cadidates = _dataContext.Candidates.Count();
            ViewBag.Job = _dataContext.JobPostings.Count();
            ViewBag.Disscus = _dataContext.Discusses.Count();
            return View();
        }

        public IActionResult GetRevenueData()
        {
            var revenueData = _dataContext.RevenueStatistics
                .OrderBy(r => r.Date)
                .Select(r => new { Date = r.Date.ToString("yyyy-MM-dd"), Amount = r.Amount })
                .ToList();

            return Json(revenueData);
        }
    }
}
