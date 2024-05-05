using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using DoAnTotNghiep.Repository.SurveyRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class SurveyController : BaseController
    {
        private readonly ISurveyRepo<Survey> _surveyRepo;
        private readonly DataContext _dataContext;

        public SurveyController(ISurveyRepo<Survey> surveyRepo, DataContext dataContext)
        {
            _surveyRepo = surveyRepo;
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            var userId = GetUserIdFromClaim();
            var account = _dataContext.Accounts.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
            if (account != null)
            {
                if (account.AccountRole == AccountRole.CandidateFree || account.AccountRole == AccountRole.CandidatePaid)
                {
                    var surveys = _dataContext.Surveys.Where(s => s.Status && s.surveyTarget == SurveyTarget.Candidate).ToList();
                    return View(surveys);
                }
                else if (account.AccountRole == AccountRole.EmployerFree || account.AccountRole == AccountRole.EmployerPaid)
                {
                    var surveys = _dataContext.Surveys.Where(s => s.Status && s.surveyTarget == SurveyTarget.Employer).ToList();
                    return View(surveys);
                }
            }
            return View();
        }


        public IActionResult Survey(Guid id)
        {
            var survey = _dataContext.Surveys
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefault(s => s.SurveyId == id);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        [HttpPost]
        public IActionResult SubmitSurvey(Guid surveyId, IFormCollection form)
        {
            foreach (var key in form.Keys)
            {
                if (Guid.TryParse(key, out Guid questionId))
                {
                    if (Guid.TryParse(form[key], out Guid optionId))
                    {
                        var answerCount = new AnswerCount
                        {
                            OptionId = optionId
                        };

                        _dataContext.AnswerCounts.Add(answerCount);
                    }
                }
            }

            _dataContext.SaveChanges();

            return RedirectToAction("Thankyou", "Survey"); 
        }

        [HttpGet]
        public IActionResult GetSurveyResults(Guid surveyId)
        {
            var survey = _dataContext.Surveys
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                        .ThenInclude(o => o.AnswerCounts)
                .FirstOrDefault(s => s.SurveyId == surveyId);

            if (survey == null)
            {
                return NotFound();
            }

            var results = new List<object>();

            foreach (var question in survey.Questions)
            {
                foreach (var option in question.Options)
                {
                    var count = option.AnswerCounts.Count;
                    results.Add(new
                    {
                        OptionId = option.OptionId,
                        OptionText = option.OptionText,
                        Count = count
                    });
                }
            }

            return Json(results);
        }


        public IActionResult Thankyou()
        {
            return View();
        }
    }
}
