using DoAnTotNghiep.Repository.OnlineResumeRepo;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class ResumeController : Controller
    {
        private readonly IOnlineResumeRepository _resumeRepository;

        public ResumeController(IOnlineResumeRepository resumeRepository)
        {
            _resumeRepository = resumeRepository;
        }
        public async Task<IActionResult> Index(Guid id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
            {
                return NotFound();
            }
            return View(resume);
        }
    }
}
