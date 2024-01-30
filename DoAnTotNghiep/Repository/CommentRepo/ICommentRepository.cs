using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.CommentRepo
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsForDiscussAsync(Guid discussId);

    }
}
