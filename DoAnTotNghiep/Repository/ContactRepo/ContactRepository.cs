using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.ContactRepo
{
    public class ContactRepository : IGenericRepository<Contact>, IContactRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<Contact> _dbSet;

        public ContactRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<Contact>();
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Contact> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task CreateAsync(Contact entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contact entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Contact> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
