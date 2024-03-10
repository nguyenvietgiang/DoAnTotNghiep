using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.OnlineResumeRepo
{
    public interface IOnlineResumeRepository
    {
        Task AddAsync(OnlineResume resume);
        Task<OnlineResume> GetByIdAsync(Guid id);
        Task<List<OnlineResume>> GetByUserIdAsync(Guid userId);
        Task RemoveAsync(Guid id);
    }
}
