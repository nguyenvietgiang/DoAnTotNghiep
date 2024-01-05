using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.DisscussRepo
{
    public class DiscussRepository : IDiscussRepository
    {
        private readonly DataContext _context;

        public DiscussRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Discuss>> GetAllDiscussions()
        {
            return await _context.Discusses
                .Include(dis => dis.Account) 
                .ToListAsync();
        }

        public async Task<Discuss> GetDiscussionById(Guid id)
        {
            return await _context.Discusses.FindAsync(id);
        }

        public async Task CreateDiscussion(Discuss discuss)
        {
            _context.Discusses.Add(discuss);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDiscussion(Discuss discuss)
        {
            _context.Discusses.Update(discuss);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscussion(Guid id)
        {
            var discuss = await _context.Discusses.FindAsync(id);
            if (discuss != null)
            {
                _context.Discusses.Remove(discuss);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleDiscussStatusAsync(Guid id)
        {
            var disscus = await _context.Discusses.FindAsync(id);

            if (disscus != null)
            {
                disscus.Status = !disscus.Status;
                await _context.SaveChangesAsync();
            }
        }
    }

}
