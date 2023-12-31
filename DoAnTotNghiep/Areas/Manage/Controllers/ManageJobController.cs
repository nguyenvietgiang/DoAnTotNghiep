using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.JobRepo;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageJobController : ManageBaseController
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        
        public ManageJobController(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository= jobPostingRepository;
        }


        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var job = await _jobPostingRepository.GetAllJobPostingsAsync();
            int totalItemCount = job.Count();
            var pagedList = new StaticPagedList<JobPosting>(job.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var jobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            return View(jobPosting);
        }
    }
}
