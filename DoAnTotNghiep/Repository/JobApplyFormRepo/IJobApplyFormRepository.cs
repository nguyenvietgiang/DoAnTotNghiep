using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.JobApplyFormRepo
{
    public interface IJobApplyFormRepository
    {
        Task AddJobApplyForm(JobApplyFormDTO jobApplyFormDTO);
        Task<IEnumerable<JobApplyForm>> GetJobApplyFormsByJobPostingID(Guid jobPostingID);
        Task<JobApplyForm> GetJobApplyFormById(Guid jobApplyID);
        Task DeleteJobApplyForm(Guid jobApplyID);
    }
}
