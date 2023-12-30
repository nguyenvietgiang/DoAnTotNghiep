using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.EmployerRepo;
using DoAnTotNghiep.Repository.JobRepo;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class EmployerController : BaseController
    {
        private readonly IEmployerRepository _employerRepository;
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IJobPostingRepository _jobPostingRepository;

        public EmployerController(IEmployerRepository employerRepository, DataContext dataContext, IFileService fileService, IJobPostingRepository jobPostingRepository)
        {
            _employerRepository = employerRepository;
            _fileService = fileService;
            _dataContext = dataContext;
            _jobPostingRepository = jobPostingRepository;
        }
        public IActionResult CompanyProfile()
        {
            var userId = GetUserIdFromClaim();

            if (userId == null)
            {
                return BadRequest("User Id not found");
            }

            var employer = _employerRepository.GetEmployerByIdAsync(Guid.Parse(userId)).Result;

            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);
        }

        public IActionResult Edit()
        {
            var userId = GetUserIdFromClaim();
            if (userId == null)
            {
                return BadRequest("User Id not found");
            }
            var employer = _employerRepository.GetEmployerByIdAsync(Guid.Parse(userId)).Result;

            if (employer == null)
            {
                return NotFound();
            }

            var editViewModel = new EmployerEditViewModel
            {
                EmployerID = employer.EmployerID,
                CompanyName = employer.CompanyName,
                UrlImage = employer.UrlImage,
                Industry = employer.Industry,
                CompanySize = employer.CompanySize,
                Location = employer.Location,
                Descrpitons = employer.Descrpitons
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employer = await _employerRepository.GetEmployerByIdAsync(model.EmployerID);

                if (employer == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin từ ViewModel vào đối tượng Employer
                employer.CompanyName = model.CompanyName;
                employer.Industry = model.Industry;
                employer.CompanySize = model.CompanySize;
                employer.Location = model.Location;
                employer.Descrpitons = model.Descrpitons;

                // Kiểm tra xem có ảnh mới không
                if (model.NewImage != null)
                {
                    // Lưu ảnh mới và cập nhật đường dẫn
                    employer.UrlImage = await _fileService.SaveImageAsync(model.NewImage);
                }


                await _dataContext.SaveChangesAsync();

                return RedirectToAction("CompanyProfile");
            }

            return View(model);
        }

        public IActionResult CompanyJobList()
        {
            return View();
        } 

        public IActionResult CreateJobPosting()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobPosting(JobPostingDto jobPostingDto)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserIdFromClaim();
                var newJobPosting = new JobPosting
                {
                    JobPostingID = Guid.NewGuid(), // Tự tạo ID mới
                    Title = jobPostingDto.Title,
                    Description = jobPostingDto.Description,
                    EmployerID = Guid.Parse(userId),
                    Location = jobPostingDto.Location,
                    CreateAt = DateTime.Now, // Thời gian hiện tại
                    Requirements = jobPostingDto.Requirements,
                    Number = jobPostingDto.Number,
                    Salary = jobPostingDto.Salary,
                    position = jobPostingDto.Position,
                    benefits = jobPostingDto.Benefits,
                    Status = false // Giá trị mặc định cho Status
                };
                await _jobPostingRepository.CreateJobPostingAsync(newJobPosting);
                return RedirectToAction("Index", "Home");
            }
            return View(jobPostingDto);
        }
    }
}
