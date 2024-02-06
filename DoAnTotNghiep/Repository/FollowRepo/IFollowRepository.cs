using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.FollowRepo
{
    public interface IFollowRepository
    {
        Task AddFollow(Guid userId, Guid followUserId);
        Task RemoveFollow(Guid userId, Guid followUserId);
        Task<int> GetFollowCount(Guid followUserId);
        Task<bool> IsFollowing(Guid userId, Guid followUserId);
        List<Account> GetFollowers(Guid userId);
        List<Account> MyFollow(Guid userId);
        Task<List<Account>> GetFollowersAsync(Guid accountId);
        Task<List<Account>> GetFollowingAsync(Guid accountId);
    }
}
