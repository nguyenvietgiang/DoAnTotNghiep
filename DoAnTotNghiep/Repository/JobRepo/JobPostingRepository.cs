using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.JobRepo
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly DataContext _context;

        public JobPostingRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobPosting>> GetAllJobPostingsAsync()
        {
            return await _context.JobPostings.Include(jp => jp.Employer).ToListAsync();
        }

        public async Task<JobPosting> GetJobPostingByIdAsync(Guid id)
        {
            return await _context.JobPostings.Include(jp => jp.Employer).FirstOrDefaultAsync(jp => jp.JobPostingID == id);
        }

        public async Task CreateJobPostingAsync(JobPosting jobPosting)
        {
            _context.JobPostings.Add(jobPosting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJobPostingAsync(JobPosting jobPosting)
        {
            _context.Entry(jobPosting).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobPostingAsync(Guid id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting != null)
            {
                _context.JobPostings.Remove(jobPosting);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleJobStatusAsync(Guid id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);

            if (jobPosting != null)
            {
                jobPosting.Status = !jobPosting.Status; 
                await _context.SaveChangesAsync();
            }
        }
    }
}
