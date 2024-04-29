using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.DisscussRepo;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using X.PagedList;
using System.Drawing.Drawing2D;
using Microsoft.EntityFrameworkCore;
using DoAnTotNghiep.Middleware;
using DoAnTotNghiep.Repository.OnlineResumeRepo;
using DoAnTotNghiep.Repository.JobRepo;

namespace DoAnTotNghiep.Controllers.MvcController
{
    [CheckUserRoleFilter(AccountRole.CandidateFree, AccountRole.CandidatePaid)]
    public class CandidatesController : BaseController
    {
        private readonly ICandidatesRepo _candidateRepository;
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDiscussRepository _discussRepository;
        private readonly IOnlineResumeRepository _onlineResumeRepository;
        private readonly IJobPostingRepository _jobPostingRepository;

        public CandidatesController(ICandidatesRepo candidateRepository, DataContext dataContext, IFileService fileService, IWebHostEnvironment webHostEnvironment, IOnlineResumeRepository onlineResumeRepository, IJobPostingRepository jobPostingRepository)
        {
            _candidateRepository = candidateRepository;
            _dataContext = dataContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _onlineResumeRepository = onlineResumeRepository;
            _jobPostingRepository = jobPostingRepository;
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

        public async Task<IActionResult> MyCvOnline()
        {
            var userId = GetUserIdFromClaim();
            var resumes = await _onlineResumeRepository.GetByUserIdAsync(Guid.Parse(userId));
            return View(resumes);
        }

        public IActionResult CreateCV(int? page)
        {
            var userId = GetUserIdFromClaim();
            var account = _dataContext.Accounts.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
            if (account.AccountRole == AccountRole.CandidateFree)
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

        public IActionResult CreateCvOnline()
        {
            return View();
        }


        public async Task<IActionResult> MyApply()
        {
            var userId = GetUserIdFromClaim();
            var account = _dataContext.Accounts.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
            List<JobPosting> jobPostings = await _jobPostingRepository.GetJobPostingsByApplicantEmailAsync(account.Email);
            return View(jobPostings);
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
                Descrpitons = cadiate.Descrpitons,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscuss(DiscussViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserIdFromClaim();

                // Tìm tài khoản từ UserId
                var account = await _dataContext.Accounts.FirstOrDefaultAsync(a => a.UserID == new Guid(userId));

                if (account != null)
                {
                    var newDiscussion = new Discuss
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Account = account, // Liên kết thảo luận với tài khoản
                        UserId = account.UserID, // Cập nhật UserId
                        CreatedAt = DateTime.Now,
                        Type = model.Type,
                        Status = false
                    };

                    _dataContext.Discusses.Add(newDiscussion);
                    await _dataContext.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Bài thảo luận đã được lưu thành công và đang chờ được duyệt!";
                    return RedirectToAction("Forum", "Home");
                }
                else
                {
                    TempData["SuccessMessage"] = "Bài thảo luận đã được lưu thành công và đang chờ được duyệt!";
                    return RedirectToAction("Forum", "Home");
                }
            }
            return View(model);
        }



        // tải file CV đang bị lỗi Font tiếng việt, dùng tiếng anh tạm
        public async Task<IActionResult> DownloadPDF(string fullName, string personalPage, string description, string education, string profession, string skill, string personalProjects)
        {
            byte[] pdfBytes;

            using (MemoryStream stream = new MemoryStream())
            {
                using (PdfDocument document = new PdfDocument())
                {
                    PdfPage page = document.Pages.Add();
                    PdfGraphics graphics = page.Graphics;
                    float pageWidth = page.GetClientSize().Width;
                    float startX = 10;
                    float startY = 10;
                    float titleFontSize = 14;
                    float contentFontSize = 12;
                    float lineHeight = 15;

                    // Tiêu đề
                    graphics.DrawString(fullName, new PdfStandardFont(PdfFontFamily.Helvetica, titleFontSize, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight;
                    // Personal page
                    graphics.DrawString(personalPage, new PdfStandardFont(PdfFontFamily.Helvetica, contentFontSize - 2), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight;

                    // Mô tả
                    graphics.DrawString("Description:", new PdfStandardFont(PdfFontFamily.Helvetica, titleFontSize, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight;
                    graphics.DrawString(description, new PdfStandardFont(PdfFontFamily.Helvetica, contentFontSize), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight * 5;

                    // Học vấn
                    graphics.DrawString("Education:", new PdfStandardFont(PdfFontFamily.Helvetica, titleFontSize, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight;
                    graphics.DrawString(education, new PdfStandardFont(PdfFontFamily.Helvetica, contentFontSize), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight * 5;

                    // Nghề nghiệp
                    graphics.DrawString("Faculary:", new PdfStandardFont(PdfFontFamily.Helvetica, titleFontSize, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight;
                    graphics.DrawString(profession, new PdfStandardFont(PdfFontFamily.Helvetica, contentFontSize), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight * 3;

                    // Kỹ năng
                    graphics.DrawString("Skill:", new PdfStandardFont(PdfFontFamily.Helvetica, titleFontSize, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight;
                    graphics.DrawString(skill, new PdfStandardFont(PdfFontFamily.Helvetica, contentFontSize), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight * 3;

                    // Kinh nghiệm làm việc
                    graphics.DrawString("Experience:", new PdfStandardFont(PdfFontFamily.Helvetica, titleFontSize, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));
                    startY += lineHeight;
                    graphics.DrawString(personalProjects, new PdfStandardFont(PdfFontFamily.Helvetica, contentFontSize), PdfBrushes.Black, new Syncfusion.Drawing.PointF(startX, startY));

                    document.Save(stream);
                }
                pdfBytes = stream.ToArray();
            }

            var userId = GetUserIdFromClaim();
            // Tạo một bản cv online mới
            var newResume = new OnlineResume
            {
                ID = Guid.NewGuid(), 
                UserId = Guid.Parse(userId),
                Name = fullName,
                email = personalPage,
                poisition = "Developer",
                education = education,
                descpription = description,
                skill= skill,
                experience = personalProjects
            };
            // Gọi phương thức thêm mới của repository
            await _onlineResumeRepository.AddAsync(newResume);

            return new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = "MyCV.pdf"
            };
        }


    }
}
