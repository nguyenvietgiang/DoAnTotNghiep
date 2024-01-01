using DoAnTotNghiep.Models.ResponseDTO;
using DoAnTotNghiep.Repository.JobRepo;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class SearchController : Controller
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public SearchController(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        public IActionResult SearchResult()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchJob(string searchTerm, string location)
        {
            var jobPostings = await _jobPostingRepository.SearchJobPostingsAsync(searchTerm, location);

            // Map to DTO
            var result = jobPostings.Select(jp => new JobPostingResponseDto
            {
                JobId = jp.JobPostingID,
                Title = jp.Title,
                Image = jp.Employer.UrlImage,
                Position = jp.position,
                Location = jp.Location
            });

            return View("SearchResult", result);
        }
    }
}
