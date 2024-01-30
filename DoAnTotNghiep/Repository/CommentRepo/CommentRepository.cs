using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.CommentRepo
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetCommentsForDiscussAsync(Guid discussId)
        {
            return await _context.Comments
                .Where(c => c.DiscussID == discussId)
                .ToListAsync();
        }
    }

}
