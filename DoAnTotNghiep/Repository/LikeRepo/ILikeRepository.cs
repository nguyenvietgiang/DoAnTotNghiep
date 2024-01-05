namespace DoAnTotNghiep.Repository.LikeRepo
{
    public interface ILikeRepository
    {
        Task AddLike(Guid userId, Guid discussId);
        Task RemoveLike(Guid userId, Guid discussId);
    }
}
