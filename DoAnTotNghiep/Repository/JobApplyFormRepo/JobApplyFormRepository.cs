using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoAnTotNghiep.Repository.JobApplyFormRepo
{
    public class JobApplyFormRepository : IJobApplyFormRepository
    {
        private readonly DbContextOptions<DataContext> _contextOptions;
        private readonly IFileService _fileService;
        private readonly ILogger<JobApplyFormRepository> _logger;

        public JobApplyFormRepository(DbContextOptions<DataContext> contextOptions, IFileService fileService, ILogger<JobApplyFormRepository> logger)
        {
            _contextOptions = contextOptions;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task AddJobApplyForm(JobApplyFormDTO jobApplyFormDTO)
        {
            try
            {
                string cvFilePath = await _fileService.SavePdfAsync(jobApplyFormDTO.CVFile);

                using (var context = new DataContext(_contextOptions))
                {
                    JobPosting jobPosting = await context.JobPostings.FirstOrDefaultAsync(jp => jp.JobPostingID == jobApplyFormDTO.JobPostingID);

                    JobApplyForm jobApplyForm = new JobApplyForm
                    {
                        JobApplyID = Guid.NewGuid(),
                        Name = jobApplyFormDTO.Name,
                        JobPostingID = jobApplyFormDTO.JobPostingID,
                        JobPosting = jobPosting,
                        PhoneNumber = jobApplyFormDTO.PhoneNumber,
                        Email = jobApplyFormDTO.Email,
                        CVFile = cvFilePath,
                        Status = false
                    };

                    context.JobApplyForms.Add(jobApplyForm);
                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm mới JobApplyForm: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm mới JobApplyForm: {Message}", ex.Message);
            }
        }

        public async Task<IEnumerable<JobApplyForm>> GetJobApplyFormsByJobPostingID(Guid jobPostingID)
        {
            using (var context = new DataContext(_contextOptions))
            {
                return await context.JobApplyForms
                    .Where(j => j.JobPostingID == jobPostingID)
                    .ToListAsync();
            }
        }

        public async Task<JobApplyForm> GetJobApplyFormById(Guid jobApplyID)
        {
            using (var context = new DataContext(_contextOptions))
            {
                return await context.JobApplyForms
                    .FirstOrDefaultAsync(j => j.JobApplyID == jobApplyID);
            }
        }

        public async Task DeleteJobApplyForm(Guid jobApplyID)
        {
            using (var context = new DataContext(_contextOptions))
            {
                var jobApplyForm = await context.JobApplyForms.FindAsync(jobApplyID);
                if (jobApplyForm != null)
                {
                    context.JobApplyForms.Remove(jobApplyForm);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateStatusAsync(Guid id)
        {
            using (var context = new DataContext(_contextOptions))
            {
                var jobApplyForm = await context.JobApplyForms.FindAsync(id);
                if (jobApplyForm != null)
                {
                    jobApplyForm.Status = !jobApplyForm.Status;
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
