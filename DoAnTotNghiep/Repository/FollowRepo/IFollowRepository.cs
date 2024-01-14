namespace DoAnTotNghiep.Repository.FollowRepo
{
    public interface IFollowRepository
    {
        Task AddFollow(Guid userId, Guid followUserId);
        Task RemoveFollow(Guid userId, Guid followUserId);
        Task<int> GetFollowCount(Guid followUserId);
        Task<bool> IsFollowing(Guid userId, Guid followUserId);
    }
}
