using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.DisscussRepo;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class CandidatesController : BaseController
    {
        private readonly ICandidatesRepo _candidateRepository;
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDiscussRepository _discussRepository;

        public CandidatesController(ICandidatesRepo candidateRepository, DataContext dataContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _candidateRepository = candidateRepository;
            _dataContext = dataContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
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

        public IActionResult CreateCV(int? page) 
        {
            var userId = GetUserIdFromClaim();
            var account = _dataContext.Accounts.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
            if(account.AccountRole == AccountRole.CandidateFree)
            {
                return RedirectToAction("NoPermistion", "Home");
            }

            int pageSize = 10;
            int pageNumber = page ?? 1;
            var cvs = _dataContext.CvLibraries.ToList();
            int totalItemCount = _dataContext.CvLibraries.Count();
            var pagedList = new StaticPagedList<CvLibrary>(cvs.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }

        public async Task<IActionResult> DownloadCv(Guid cvId)
        {
            var cvLibrary = await _dataContext.CvLibraries.FindAsync(cvId);

            if (cvLibrary == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, cvLibrary.CvFile.TrimStart('/'));

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", cvLibrary.CvName + ".pdf");
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

        // Action để hiển thị form tạo mới Discussion
        public IActionResult CreateDiscuss() 
        {
            return View();
        }

        // Action để xử lý việc gửi dữ liệu từ form tạo mới Discussion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscuss(DiscussViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserIdFromClaim();
                var newDiscussion = new Discuss
                {
                   
                    Title = model.Title,
                    Content = model.Content,
                    UserId = new Guid(userId),
                    CreatedAt = DateTime.Now,
                    Type = model.Type,
                    Status = false
                };
                _dataContext.Discusses.Add(newDiscussion);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
