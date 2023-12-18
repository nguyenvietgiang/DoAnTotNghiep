using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.EmployerRepo;
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

        public EmployerController(IEmployerRepository employerRepository, DataContext dataContext, IFileService fileService)
        {
            _employerRepository = employerRepository;
            _fileService= fileService;
            _dataContext = dataContext;
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
    }
}
