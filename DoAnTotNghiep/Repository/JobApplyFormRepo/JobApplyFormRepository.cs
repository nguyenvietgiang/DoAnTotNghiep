using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Services.ImageServices;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.JobApplyFormRepo
{
    public class JobApplyFormRepository : IJobApplyFormRepository
    {
        private readonly DataContext _context;
        private readonly IFileService _fileService;

        public JobApplyFormRepository(DataContext context, IFileService fileService)
        {
            _context = context;
            _fileService= fileService;
        }

        public async Task AddJobApplyForm(JobApplyFormDTO jobApplyFormDTO)
        {
            string cvFilePath = await _fileService.SavePdfAsync(jobApplyFormDTO.CVFile);

            JobApplyForm jobApplyForm = new JobApplyForm
            {
                JobApplyID = Guid.NewGuid(),
                Name = jobApplyFormDTO.Name,
                JobPostingID = jobApplyFormDTO.JobPostingID,
                PhoneNumber = jobApplyFormDTO.PhoneNumber,
                Email = jobApplyFormDTO.Email,
                CVFile = cvFilePath,
                Status = false
            };

            _context.JobApplyForms.Add(jobApplyForm);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<JobApplyForm>> GetJobApplyFormsByJobPostingID(Guid jobPostingID)
        {
            return await _context.JobApplyForms
                .Where(j => j.JobPostingID == jobPostingID)
                .ToListAsync();
        }

        public async Task<JobApplyForm> GetJobApplyFormById(Guid jobApplyID)
        {
            return await _context.JobApplyForms
                .FirstOrDefaultAsync(j => j.JobApplyID == jobApplyID);
        }

        public async Task DeleteJobApplyForm(Guid jobApplyID)
        {
            var jobApplyForm = await _context.JobApplyForms.FindAsync(jobApplyID);
            if (jobApplyForm != null)
            {
                _context.JobApplyForms.Remove(jobApplyForm);
                await _context.SaveChangesAsync();
            }
        }
    }

}
