using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.EmployerRepo
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly DataContext _context;

        public EmployerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employer>> GetAllAsync()
        {
           return await _context.Employers.ToListAsync();
        }

        public async Task<Employer> GetEmployerByIdAsync(Guid employerId)
        {
            return await _context.Employers
                .Include(e => e.Account) 
                .FirstOrDefaultAsync(e => e.EmployerID == employerId);
        }


    }
}
