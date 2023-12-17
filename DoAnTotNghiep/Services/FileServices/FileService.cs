namespace DoAnTotNghiep.Services.ImageServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                await imageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));
                return "/uploads/" + uniqueFileName;
            }
            return null;
        }

        public async Task<string> SavePdfAsync(IFormFile pdfFile)
        {
            if (pdfFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "cvFile");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + pdfFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                await pdfFile.CopyToAsync(new FileStream(filePath, FileMode.Create));
                return "/cvFile/" + uniqueFileName;
            }
            return null;
        }
    }
}
