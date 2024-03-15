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
        Task ToggleJobStatusAsync(Guid id);
        Task<IEnumerable<JobPosting>> GetUnapprovedJobPostingsAsync();
        Task<IEnumerable<JobPosting>> GetApprovedJobPostingsAsync();
        Task<IEnumerable<JobPosting>> GetApprovedJobPostingsByEmployerAsync(Guid employerId);
        Task<IEnumerable<JobPosting>> SearchJobPostingsAsync(string searchTerm, string location);
        Task<IDictionary<string, int>> GetJobPositionsCountAsync();
        Task<IEnumerable<JobPosting>> GetSimilarJobsAsync(Guid jobId);
        Task<IEnumerable<JobPosting>> GetFilteredJobPostingsAsync(string title, string location, int? salary, string time, string recommend);
        Task<List<JobPosting>> GetJobPostingsByApplicantEmailAsync(string applicantEmail);

    }
}
