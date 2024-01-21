using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.DisscussRepo
{
    public interface IDiscussRepository
    {
        Task<List<Discuss>> GetAllDiscussions();
        Task<Discuss> GetDiscussionById(Guid id);
        Task CreateDiscussion(Discuss discuss);
        Task UpdateDiscussion(Discuss discuss);
        Task DeleteDiscussion(Guid id);
        Task ToggleDiscussStatusAsync(Guid id);
        Task<List<Discuss>> GetApprovedDiscussions();
        Task<List<Discuss>> GetUnapprovedDiscussions();
    }
}
