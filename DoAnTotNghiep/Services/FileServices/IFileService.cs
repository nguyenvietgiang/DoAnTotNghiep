namespace DoAnTotNghiep.Services.ImageServices
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile imageFile);
        Task<string> SavePdfAsync(IFormFile pdfFile);
    }
}
