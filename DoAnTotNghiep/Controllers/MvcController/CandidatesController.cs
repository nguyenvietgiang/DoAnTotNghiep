using DoAnTotNghiep.Repository.CandidatesRepo;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class CandidatesController : BaseController
    {
        private readonly ICandidatesRepo _candidateRepository;
        public CandidatesController(ICandidatesRepo candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }
        public IActionResult Profile()
        {
            var userId = GetUserIdFromClaim();

            if (userId == null)
            {
                return BadRequest("User Id not found");
            }

            var candidate = _candidateRepository.GetCandidateByIdAsync(Guid.Parse(userId)).Result;

            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }


        public IActionResult Survey()
        {
            return View(); 
        }

        public IActionResult Edit()
        {
            return View();
        }
     }
}
