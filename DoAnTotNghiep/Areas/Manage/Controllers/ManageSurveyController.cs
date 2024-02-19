using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.SurveyRepo;
using DoAnTotNghiep.Services.ExportServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using System.Linq;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageSurveyController : ManageBaseController
    {
        private readonly ISurveyRepo<Survey> _surveyRepo;
        private readonly ISurveyRepo<Question> _questionRepo;
        private readonly ISurveyRepo<Option> _optionRepo;
        private readonly IExcelExportService _excelExportService;
        private readonly DataContext _dataContext;

        public ManageSurveyController(ISurveyRepo<Survey> surveyRepo, ISurveyRepo<Question> questionRepo, ISurveyRepo<Option> optionRepo, DataContext dataContext, IExcelExportService excelExportService)
        {
            _excelExportService = excelExportService;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Survey survey)
        {
                _surveyRepo.Insert(survey);
                _surveyRepo.Save();
                return RedirectToAction("Index");
        }

        public IActionResult Details(Guid id)
        {
            var survey = _dataContext.Surveys.Include(s => s.Questions).FirstOrDefault(s => s.SurveyId == id);
            if (survey == null)
            {
                return NotFound();
            }

            var questionsInSurvey = survey.Questions.ToList();
            ViewBag.QuestionsInSurvey = questionsInSurvey;

            return View(survey);
        }

        public IActionResult DownloadExcelTemplate()
        {
            byte[] templateBytes = _excelExportService.GetExcelTemplate("Question");
            return File(templateBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Question.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> ImportQuestionsFromExcel(Guid surveyId, IFormFile excelFile)
        {
            var survey = _surveyRepo.GetById(surveyId);
            if (excelFile != null && excelFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    stream.Position = 0;

                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        IApplication application = excelEngine.Excel;
                        IWorkbook workbook = application.Workbooks.Open(stream);
                        IWorksheet worksheet = workbook.Worksheets[0];

                        int rowCount = worksheet.Rows.Count();
                        int columnCount = worksheet.Columns.Count();

                        for (int i = 1; i < rowCount; i++) // Start from row 2, assuming row 1 is header
                        {
                            var row = worksheet.Rows[i];
                            if (row != null)
                            {
                                var questionText = worksheet.Rows[i].Cells[0].Value?.ToString();
                                if (!string.IsNullOrEmpty(questionText))
                                {
                                    var question = new Question { QuestionText = questionText, SurveyId = surveyId };

                                    // Add question to repository
                                    _questionRepo.Insert(question);

                                    // Add options if needed
                                    for (int j = 1; j < Math.Min(columnCount, 4); j++) // Assuming options are in columns B to E
                                    {
                                        var optionText = worksheet.Rows[i].Cells[j].Value?.ToString();
                                        if (!string.IsNullOrEmpty(optionText))
                                        {
                                            var option = new Option { OptionText = optionText, QuestionId = question.QuestionId };

                                            // Add option to repository
                                            _optionRepo.Insert(option);
                                        }
                                    }
                                }
                            }
                        }

                        _questionRepo.Save();
                        _optionRepo.Save();
                    }
                }
            }
            return RedirectToAction("Details", new { id = surveyId });
        }

    }
}
