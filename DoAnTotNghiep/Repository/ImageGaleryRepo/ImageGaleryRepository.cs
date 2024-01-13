using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.ImageGaleryRepo
{
    public class ImageGaleryRepository : IImageGaleryRepository
    {
        private readonly DataContext _context;

        public ImageGaleryRepository(DataContext context)
        {
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

        public async Task CreateImageGaleryAsync(ImageGalery imageGalery)
        {
            _context.ImageGaleries.Add(imageGalery);
            await _context.SaveChangesAsync();
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
