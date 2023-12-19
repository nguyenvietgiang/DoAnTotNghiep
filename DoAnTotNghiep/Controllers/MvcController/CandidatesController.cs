using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class CandidatesController : BaseController
    {
        private readonly ICandidatesRepo _candidateRepository;
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public CandidatesController(ICandidatesRepo candidateRepository, DataContext dataContext, IFileService fileService)
        {
            _candidateRepository = candidateRepository;
            _dataContext = dataContext;
            _fileService = fileService;
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

        public IActionResult CreateCV() 
        {
            var userId = GetUserIdFromClaim();
            var account = _dataContext.Accounts.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
            if(account.AccountRole == AccountRole.CandidateFree)
            {
                return RedirectToAction("NoPermistion", "Home");
            }    

            return View();
        }

        public IActionResult Edit()
        {
            var userId = GetUserIdFromClaim();
            if (userId == null)
            {
                return BadRequest("User Id not found");
            }
            var cadiate = _candidateRepository.GetCandidateByIdAsync(Guid.Parse(userId)).Result;

            if (cadiate == null)
            {
                return NotFound();
            }

            var editViewModel = new CandidateEditViewModels
            {
                CandidateID = cadiate.CandidateID,
                Name = cadiate.Name,
                UrlImage = cadiate.UrlImage,
                Industry = cadiate.Industry,
                EducationLevel = cadiate.EducationLevel,
                DateOfBirth = cadiate.DateOfBirth,
                PhoneNumber = cadiate.PhoneNumber,
                Descrpitons= cadiate.Descrpitons,
                Experience = cadiate.Experience,
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CandidateEditViewModels model)
        {
            if (ModelState.IsValid)
            {
                var cadiate = await _candidateRepository.GetCandidateByIdAsync(model.CandidateID);

                if (cadiate == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin từ ViewModel vào đối tượng cadiate
                cadiate.Name = model.Name;
                cadiate.Industry = model.Industry;
                cadiate.DateOfBirth = model.DateOfBirth;
                cadiate.Experience = model.Experience;
                cadiate.EducationLevel = model.EducationLevel;
                cadiate.PhoneNumber = model.PhoneNumber;
                cadiate.Descrpitons = model.Descrpitons;

                // Kiểm tra xem có ảnh mới không
                if (model.NewImage != null)
                {
                    // Lưu ảnh mới và cập nhật đường dẫn
                    cadiate.UrlImage = await _fileService.SaveImageAsync(model.NewImage);
                }


                await _dataContext.SaveChangesAsync();

                return RedirectToAction("Profile");
            }

            return View(model);
        }
    }
}
