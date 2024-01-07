using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.LikeRepo
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddLike(Guid userId, Guid discussId)
        {
            // Kiểm tra xem người dùng đã like bài viết này chưa
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.DiscussID == discussId);

            if (existingLike == null)
            {
                // Người dùng chưa like, thêm mới
                var like = new Like
                {
                    ID = Guid.NewGuid(),
                    UserId = userId,
                    DiscussID = discussId
                };

                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
            }

        }

        public async Task RemoveLike(Guid userId, Guid discussId)
        {
            // Tìm like để xóa
            var likeToRemove = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.DiscussID == discussId);

            if (likeToRemove != null)
            {
                // Xóa like
                _context.Likes.Remove(likeToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Like>> GetLikesByDiscussId(Guid discussId)
        {
            return await _context.Likes
                .Where(l => l.DiscussID == discussId)
                .ToListAsync();
        }

        public async Task<int> GetLikeCountByDiscussId(Guid discussId)
        {
            return await _context.Likes
                .Where(l => l.DiscussID == discussId)
                .CountAsync();
        }
    }
}
