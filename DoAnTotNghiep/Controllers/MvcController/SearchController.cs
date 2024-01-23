using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.ResponseDTO;
using DoAnTotNghiep.Repository.JobRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class SearchController : Controller
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly DataContext _dataContext;

        public SearchController(IJobPostingRepository jobPostingRepository, DataContext dataContext)
        {
            _jobPostingRepository = jobPostingRepository;
            _dataContext = dataContext;
        }

        public IActionResult SearchResult()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchJob(string? searchTerm, string? location)
        {
            var jobPostings = await _jobPostingRepository.SearchJobPostingsAsync(searchTerm, location);
            var result = jobPostings.Select(jp => new JobPostingResponseDto
            {
                JobId = jp.JobPostingID,
                Title = jp.Title,
                Image = jp.Employer.UrlImage,
                Position = jp.position,
                Location = jp.Location,
                Salary= jp.Salary,
                CompanyId = jp.EmployerID,
            });

            return View("SearchResult", result);
        }

        public async Task<IActionResult> DetailJob(Guid id)
        {
            var jobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

            if (jobPosting == null)
            {
                return NotFound();
            }
            ViewBag.SameJob = await _jobPostingRepository.GetSimilarJobsAsync(id);
            return View(jobPosting);
        }

        [HttpGet]
        public async Task<IActionResult> GetTitlesBySearch(string searchString)
        {
            IQueryable<string> query = _dataContext.JobPostings
                                        .Where(m => m.Status == true && m.Title.Contains(searchString))
                                        .Select(m => m.Title);

            var titles = await query.ToListAsync();
            return Json(titles);
        } 

        [HttpGet]
        public async Task<IActionResult> GetCompanyBySearch(string searchString)
        {
            IQueryable<string> query = _dataContext.Employers
                                        .Where(m => m.CompanyName.Contains(searchString))
                                        .Select(m => m.CompanyName);

            var titles = await query.ToListAsync();
            return Json(titles);
        }
    }
}
