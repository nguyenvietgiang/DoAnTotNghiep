namespace DoAnTotNghiep.Services.ImageServices
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile);
    }
}
