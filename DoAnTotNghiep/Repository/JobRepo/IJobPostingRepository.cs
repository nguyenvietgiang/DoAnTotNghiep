using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.JobRepo
{
    public interface IJobPostingRepository
    {
        Task<IEnumerable<JobPosting>> GetAllJobPostingsAsync();
        Task<JobPosting> GetJobPostingByIdAsync(Guid id);
        Task CreateJobPostingAsync(JobPosting jobPosting);
        Task UpdateJobPostingAsync(JobPosting jobPosting);
        Task DeleteJobPostingAsync(Guid id);
    }
}
