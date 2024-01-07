using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.LikeRepo
{
    public interface ILikeRepository
    {
        Task AddLike(Guid userId, Guid discussId);
        Task RemoveLike(Guid userId, Guid discussId);
        Task<List<Like>> GetLikesByDiscussId(Guid discussId);
        Task<int> GetLikeCountByDiscussId(Guid discussId);
    }
}
