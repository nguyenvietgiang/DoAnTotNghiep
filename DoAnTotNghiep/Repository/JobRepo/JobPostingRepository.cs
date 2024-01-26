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

        public async Task<IEnumerable<JobPosting>> GetUnapprovedJobPostingsAsync()
        {
            return await _context.JobPostings
                .Include(jp => jp.Employer)
                .Where(jp => !jp.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobPosting>> GetApprovedJobPostingsAsync()
        {
            return await _context.JobPostings
                .Include(jp => jp.Employer)
                .Where(jp => jp.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobPosting>> SearchJobPostingsAsync(string searchTerm, string location)
        {
            var query = _context.JobPostings
                .Include(jp => jp.Employer)
                .Where(jp => jp.Status);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(jp => jp.Title.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(jp => jp.Location == location);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<JobPosting>> GetApprovedJobPostingsByEmployerAsync(Guid employerId)
        {
            return await _context.JobPostings
                .Include(jp => jp.Employer)
                .Where(jp => jp.EmployerID == employerId && jp.Status)
                .ToListAsync();
        }

        public async Task<IDictionary<string, int>> GetJobPositionsCountAsync()
        {
            var positionsCount = await _context.JobPostings
                .Where(jp => jp.Status)
                .GroupBy(jp => jp.position)
                .Select(g => new { Position = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToDictionaryAsync(x => x.Position, x => x.Count);

            return positionsCount;
        }

        public async Task<IEnumerable<JobPosting>> GetSimilarJobsAsync(Guid jobId)
        {
            var currentJob = await _context.JobPostings
                .Include(jp => jp.Employer)
                .FirstOrDefaultAsync(jp => jp.JobPostingID == jobId);

            if (currentJob == null)
            {
                return Enumerable.Empty<JobPosting>();
            }
            var similarJobs = await _context.JobPostings
                .Include(jp => jp.Employer)
                .Where(jp => jp.position == currentJob.position && jp.JobPostingID != jobId)
                .ToListAsync();

            return similarJobs;
        }

        public async Task<IEnumerable<JobPosting>> GetFilteredJobPostingsAsync(string title, string location, int? salary, string time)
        {
            var query = _context.JobPostings.Include(jp => jp.Employer).Where(jp => jp.Status);

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(jp => jp.Title.Contains(title) || jp.Requirements.Contains(title));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(jp => jp.Location.Contains(location));
            }

            if (salary.HasValue)
            {
                query = query.Where(jp => jp.Salary == salary.Value);
            }

            if (!string.IsNullOrEmpty(time))
            {
                query = query.Where(jp => jp.WorkingTime == time);
            }

            return await query.ToListAsync();
        }

    }
}
