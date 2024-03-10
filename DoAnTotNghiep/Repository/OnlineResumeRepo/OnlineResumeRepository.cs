using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.OnlineResumeRepo
{
    public class OnlineResumeRepository : IOnlineResumeRepository
    {
        private readonly DataContext _context;

        public OnlineResumeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OnlineResume resume)
        {
            await _context.OnlineResumes.AddAsync(resume);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OnlineResume>> GetByUserIdAsync(Guid userId)
        {
            return await _context.OnlineResumes.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var resume = await _context.OnlineResumes.FindAsync(id);
            if (resume != null)
            {
                _context.OnlineResumes.Remove(resume);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<OnlineResume> GetByIdAsync(Guid id)
        {
            return await _context.OnlineResumes.FindAsync(id);
        }
    }
}
