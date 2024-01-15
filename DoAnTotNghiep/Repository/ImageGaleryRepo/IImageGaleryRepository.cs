using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.ImageGaleryRepo
{
    public interface IImageGaleryRepository
    {
        Task<ImageGalery> GetImageGaleryByIdAsync(Guid id);
        Task<List<ImageGalery>> GetAllImageGaleriesAsync();
        Task CreateImageGaleryAsync(ImageGaleryCreateDTO imageGaleryDTO);
        Task UpdateImageGaleryAsync(ImageGalery imageGalery);
        Task DeleteImageGaleryAsync(Guid id);
        Task<List<ImageGalery>> GetImageGaleriesByEmployerIdAsync(Guid employerId);
    }
}
