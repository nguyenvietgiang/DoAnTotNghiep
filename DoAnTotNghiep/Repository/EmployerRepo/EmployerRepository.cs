using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO.Implementation.Security;

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


        public async Task<List<Employer>> GetTop5EmployersWithJobCount()
        {
            var top5Employers = await _context.Employers
                .OrderByDescending(e => e.JobPostings.Count)
                .Take(5)
                .ToListAsync();

            return top5Employers;
        }

    }
}
