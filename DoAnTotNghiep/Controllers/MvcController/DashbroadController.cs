using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DoAnTotNghiep.Controllers.MvcController
{
    public class DashbroadController : BaseController
    {
        private readonly DataContext _dataContext;

        public DashbroadController(DataContext dataContext)
        {
            _dataContext= dataContext;
        }
        public async Task<IActionResult> Index()
        {
            var userId = GetUserIdFromClaim();
            var account = _dataContext.Accounts.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
            if (account.AccountRole == AccountRole.CandidateFree || account.AccountRole == AccountRole.EmployerFree)
            {
                return RedirectToAction("NoPermistion", "Home");
            }
            var topFiveJobs = await _dataContext.JobPostings.OrderByDescending(j => j.Salary).Take(5).ToListAsync();

            ViewBag.surveysWithMostCommonOptions = await _dataContext.Surveys
        .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
        .Select(s => new
        {
            Survey = s,
            QuestionWithMostCommonOption = s.Questions
                .Select(q => new
                {
                    Question = q,
                    MostCommonOption = q.Options
                        .OrderByDescending(o => o.AnswerCounts.Count)
                        .FirstOrDefault()
                })
                .OrderByDescending(q => q.MostCommonOption.AnswerCounts.Count)
                .FirstOrDefault()
        })
        .ToListAsync();
            return View(topFiveJobs);
        }
    }
}
