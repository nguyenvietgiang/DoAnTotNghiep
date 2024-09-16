using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using DoAnTotNghiep.Models.ResponseDTO;
using DoAnTotNghiep.Repository.EmployerRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class DashbroadController : BaseController
    {
        private readonly DataContext _dataContext;
        private readonly IEmployerRepository _employerRepository;

        public DashbroadController(DataContext dataContext,IEmployerRepository employerRepository)
        {
            _dataContext= dataContext;
            _employerRepository= employerRepository;
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

            var surveysWithMostCommonOptions = await _dataContext.Surveys
    .Include(s => s.Questions)
        .ThenInclude(q => q.Options)
    .Select(s => new SurveyWithMostCommonOptionViewModel
    {
        Survey = s,
        QuestionWithMostCommonOption = s.Questions
            .Select(q => new QuestionWithMostCommonOptionViewModel
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

            ViewBag.SurveysWithMostCommonOptions = surveysWithMostCommonOptions;


            var topEmployers = await _employerRepository.GetTop3EmployersWithJobPostCounts();
            ViewBag.TopEmployers = topEmployers;

            return View(topFiveJobs);
        }

        public IActionResult Detail(Guid id)
        {
            // Lấy thông tin bài khảo sát theo ID
            var survey = _dataContext.Surveys.FirstOrDefault(s => s.SurveyId == id);

            if (survey == null)
            {
                return NotFound();
            }

            // Lấy danh sách câu hỏi và số lần xuất hiện của mỗi lựa chọn
            var questionsWithOptionCounts = _dataContext.Questions
                .Where(q => q.SurveyId == id)
                .Select(q => new QuestionWithOptionCountsViewModel
                {
                    Question = q,
                    OptionCounts = q.Options.Select(o => new OptionCountViewModel
                    {
                        Option = o,
                        Count = _dataContext.AnswerCounts.Count(ac => ac.OptionId == o.OptionId)
                    }).ToList()
                }).ToList();

            var model = new SurveyDetailViewModel
            {
                Survey = survey,
                QuestionsWithOptionCounts = questionsWithOptionCounts
            };

            return View(model);
        }
    }
}
