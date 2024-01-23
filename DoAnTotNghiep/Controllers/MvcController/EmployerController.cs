using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.ResponseDTO;
using DoAnTotNghiep.Repository.EmployerRepo;
using DoAnTotNghiep.Repository.ImageGaleryRepo;
using DoAnTotNghiep.Repository.JobApplyFormRepo;
using DoAnTotNghiep.Repository.JobRepo;
using DoAnTotNghiep.Services.EmailServices;
using DoAnTotNghiep.Services.ExportServices;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.XlsIO;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class EmployerController : BaseController
    {
        private readonly IEmployerRepository _employerRepository;
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IJobApplyFormRepository _jobApplyFormRepository;
        private readonly IEmailServices _emailServices;
        private readonly IExcelExportService _excelExportService;
        private readonly IImageGaleryRepository _imageGaleryRepository;
        public EmployerController(IEmployerRepository employerRepository, DataContext dataContext, IFileService fileService, IJobPostingRepository jobPostingRepository, IJobApplyFormRepository jobApplyFormRepository, IEmailServices emailServices, IImageGaleryRepository imageGaleryRepository, IExcelExportService excelExportService)
        {
            _employerRepository = employerRepository;
            _fileService = fileService;
            _dataContext = dataContext;
            _jobPostingRepository = jobPostingRepository;
            _jobApplyFormRepository = jobApplyFormRepository;
            _emailServices = emailServices;
            _imageGaleryRepository= imageGaleryRepository;
            _excelExportService= excelExportService;
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
                employer.PhoneNumber = model.PhoneNumber;
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

        public async Task<IActionResult> CompanyJobList()
        {
            var userId = GetUserIdFromClaim();
            var approvedJobPostings = await _jobPostingRepository.GetApprovedJobPostingsByEmployerAsync(Guid.Parse(userId));
            var result = approvedJobPostings.Select(jp => new JobPostingResponseDto
            {
                JobId = jp.JobPostingID,
                Title = jp.Title,
                Image = jp.Employer.UrlImage,
                Position = jp.position,
                Location = jp.Location,
                Salary = jp.Salary,
                CompanyId = jp.EmployerID,
            });
            return View(result);
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
                try
                {
                    var userId = GetUserIdFromClaim();
                    var newJobPosting = new JobPosting
                    {
                        JobPostingID = Guid.NewGuid(), 
                        Title = jobPostingDto.Title,
                        Description = jobPostingDto.Description,
                        EmployerID = Guid.Parse(userId),
                        Location = jobPostingDto.Location,
                        CreateAt = DateTime.Now, 
                        Requirements = jobPostingDto.Requirements,
                        Number = jobPostingDto.Number,
                        Salary = jobPostingDto.Salary,
                        position = jobPostingDto.Position,
                        benefits = jobPostingDto.Benefits,
                        WorkingTime = jobPostingDto.WorkTime,
                        Status = false 
                    };
                    await _jobPostingRepository.CreateJobPostingAsync(newJobPosting);
                    TempData["SuccessMessage"] = "Tin tuyển dụng đã được lưu thành công và đang chờ được duyệt!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Đã có lỗi sảy ra khi thực hiện lưu dữ liệu: " + ex.Message;
                }
                return RedirectToAction("CompanyJobList");
            }
                return View(jobPostingDto);
        }
         
        public IActionResult DownloadExcelTemplate()
        {
            byte[] templateBytes = _excelExportService.GetExcelTemplate("job-posting");
            return File(templateBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "job-posting.xlsx");
        }

        // đang lỗi cần được fix
        [HttpPost]
        public async Task<IActionResult> ImportJobPostings(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    using (var stream = file.OpenReadStream())
                    {
                        using (ExcelEngine excelEngine = new ExcelEngine())
                        {
                            IApplication application = excelEngine.Excel;
                            IWorkbook workbook = application.Workbooks.Open(stream);
                            IWorksheet worksheet = workbook.Worksheets[0];
                            var rowCount = worksheet.Rows.Length;
                            for (int i = 2; i <= rowCount; i++)
                            {
                                var jobPostingDto = new JobPostingDto
                                {
                                    Title = worksheet.Rows[i].Cells[1].Text ?? string.Empty,
                                    Description = worksheet.Rows[i].Cells[2].Text ?? string.Empty,
                                    Location = worksheet.Rows[i].Cells[3].Text ?? string.Empty,
                                    Requirements = worksheet.Rows[i].Cells[4].Text ?? string.Empty,
                                    Number = int.Parse(worksheet.Rows[i].Cells[5].Text),
                                    Salary = int.Parse(worksheet.Rows[i].Cells[6].Text),
                                    Position = worksheet.Rows[i].Cells[7].Text,
                                    Benefits = worksheet.Rows[i].Cells[8].Text
                                };
                                var userId = GetUserIdFromClaim();
                                var newJobPosting = new JobPosting
                                {
                                    JobPostingID = Guid.NewGuid(),
                                    Title = jobPostingDto.Title,
                                    Description = jobPostingDto.Description,
                                    EmployerID = Guid.Parse(userId),
                                    Location = jobPostingDto.Location,
                                    CreateAt = DateTime.Now,
                                    Requirements = jobPostingDto.Requirements,
                                    Number = jobPostingDto.Number,
                                    Salary = jobPostingDto.Salary,
                                    position = jobPostingDto.Position,
                                    benefits = jobPostingDto.Benefits,
                                    Status = false
                                };
                                await _jobPostingRepository.CreateJobPostingAsync(newJobPosting);
                            }
                            TempData["SuccessMessage"] = "Dữ liệu từ file Excel đã được nhập thành công và đang được chờ duyệt!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi nhập dữ liệu từ file Excel: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng chọn một file Excel để nhập dữ liệu!";
            }

            return RedirectToAction("CompanyJobList");
        }

        public async Task<IActionResult> ApplyList(Guid JobId)
        {
            var jobApplyForms = await _jobApplyFormRepository.GetJobApplyFormsByJobPostingID(JobId);

            return View(jobApplyForms);
        }

        [HttpPost]
        public async Task<IActionResult> JobContactResponse(Guid id, string repcontent)
        {
            try
            {
                var jobcontact = await _jobApplyFormRepository.GetJobApplyFormById(id);
                await _emailServices.SendEmailAsync(jobcontact.Email, repcontent);
                TempData["SuccessMessage"] = "Gửi email thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Gửi email thất bại: " + ex.Message;
            }
            return RedirectToAction("CompanyJobList");
        }

        public async Task<IActionResult> ImageGalery() 
        {
            var userId = GetUserIdFromClaim();
            ViewBag.GaleryList = await _imageGaleryRepository.GetImageGaleriesByEmployerIdAsync(Guid.Parse(userId));
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ImageGalery(ImageGaleryCreateDTO imageGaleryDTO)
        {
            if (ModelState.IsValid) 
            {
                var userId = GetUserIdFromClaim();
                imageGaleryDTO.EmployerID = Guid.Parse(userId);
                await _imageGaleryRepository.CreateImageGaleryAsync(imageGaleryDTO);
                return RedirectToAction("CompanyProfile");
            }

            return View(imageGaleryDTO);
        }

    }
}
