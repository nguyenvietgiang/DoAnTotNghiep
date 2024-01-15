using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.ImageGaleryRepo
{
    public class ImageGaleryRepository : IImageGaleryRepository
    {
        private readonly DataContext _context;
        private readonly IFileService _fileService;
        public ImageGaleryRepository(DataContext context, IFileService fileService)
        {
            _fileService= fileService;
            _context = context;
        }

        public async Task<ImageGalery> GetImageGaleryByIdAsync(Guid id)
        {
            return await _context.ImageGaleries.FindAsync(id);
        }

        public async Task<List<ImageGalery>> GetAllImageGaleriesAsync()
        {
            return await _context.ImageGaleries.ToListAsync();
        }

        public async Task CreateImageGaleryAsync(ImageGaleryCreateDTO imageGaleryDTO)
        {
            if (imageGaleryDTO.ImageFile != null)
            {
                string imgUrl = await _fileService.SaveImageAsync(imageGaleryDTO.ImageFile);

                var imageGalery = new ImageGalery
                {
                    JobPostingID = Guid.NewGuid(),
                    EmployerID = imageGaleryDTO.EmployerID,
                    ImgUrl = imgUrl
                };

                _context.ImageGaleries.Add(imageGalery);
                await _context.SaveChangesAsync();
            }
        }


        public async Task UpdateImageGaleryAsync(ImageGalery imageGalery)
        {
            _context.Entry(imageGalery).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImageGaleryAsync(Guid id)
        {
            var imageGalery = await _context.ImageGaleries.FindAsync(id);
            if (imageGalery != null)
            {
                _context.ImageGaleries.Remove(imageGalery);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ImageGalery>> GetImageGaleriesByEmployerIdAsync(Guid employerId)
        {
            return await _context.ImageGaleries
                .Where(img => img.EmployerID == employerId)
                .ToListAsync();
        }
    }

}
