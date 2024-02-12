using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.SurveyRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class SurveyController : Controller
    {
        private readonly ISurveyRepo<Survey> _surveyRepo;
        private readonly ISurveyRepo<Question> _questionRepo;
        private readonly ISurveyRepo<Option> _optionRepo;
        private readonly DataContext _dataContext;

        public SurveyController(ISurveyRepo<Survey> surveyRepo, ISurveyRepo<Question> questionRepo, ISurveyRepo<Option> optionRepo, DataContext dataContext)
        {
            _surveyRepo = surveyRepo;
            _questionRepo = questionRepo;
            _optionRepo = optionRepo;
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            var surveys = _surveyRepo.GetAll();
            return View(surveys);
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
        public IActionResult Thankyou()
        {
            return View();
        }
    }
}
