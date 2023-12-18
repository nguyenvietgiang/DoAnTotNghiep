using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageCvController : ManageBaseController
    {
        private readonly DataContext _context;
        private readonly IFileService _fileService;

        public ManageCvController(DataContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var cvs = _context.CvLibraries.ToList();
            int totalItemCount = _context.CvLibraries.Count();
            var pagedList = new StaticPagedList<CvLibrary>(cvs.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CvLibraryViewModel cvViewModel)
        {
            if (ModelState.IsValid)
            {
                var imageUrl = await _fileService.SaveImageAsync(cvViewModel.ImageFile);
                var pdfUrl = await _fileService.SavePdfAsync(cvViewModel.PdfFile);

                var cvLibrary = new CvLibrary
                {
                    CvID = Guid.NewGuid(),
                    CvName = cvViewModel.CvName,
                    CvType = cvViewModel.CvType,
                    CvImage = imageUrl,
                    CvFile = pdfUrl
                };

                _context.CvLibraries.Add(cvLibrary);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(cvViewModel);
        }
    }
}
