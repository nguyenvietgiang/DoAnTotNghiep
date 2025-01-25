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

            // Tìm tài khoản từ UserId
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserID == userId);

            if (existingLike == null)
            {
                // Người dùng chưa like, thêm mới
                var like = new Like
                {
                    ID = Guid.NewGuid(),
                    UserId = userId,
                    Account = account,
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


        /// <summary>
        /// sử dụng transaction cho like (vừa thêm hoặc xóa like vưa cập nhập số lượng like)
        /// Transaction đảm bảo rằng cả hai thao tác đều được hoàn thành hoặc đều bị hủy nếu một thao tác thất bại.
        /// Nếu chỉ dùng vs 1 bảng và không có ràng buộc, EF Core tự quản lý transaction qua SaveChangesAsync() ko cần BeginTransactionAsync().
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="discussId"></param>
        /// <returns></returns>
        public async Task ToggleLike(Guid userId, Guid discussId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Kiểm tra xem người dùng đã like bài viết này chưa
                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.UserId == userId && l.DiscussID == discussId);

                // Tìm tài khoản từ UserId
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserID == userId);

                if (existingLike == null)
                {
                    // Người dùng chưa like, thêm mới
                    var like = new Like
                    {
                        ID = Guid.NewGuid(),
                        UserId = userId,
                        Account = account,
                        DiscussID = discussId
                    };

                    _context.Likes.Add(like);

                    // Cập nhật số lượng like của bài viết
                    var discuss = await _context.Discusses.FindAsync(discussId);
                    if (discuss != null)
                    {
                        discuss.LikeCount++;
                        _context.Discusses.Update(discuss);
                    }
                }
                else
                {
                    // Người dùng đã like, xóa like
                    _context.Likes.Remove(existingLike);

                    // Cập nhật số lượng like của bài viết
                    var discuss = await _context.Discusses.FindAsync(discussId);
                    if (discuss != null && discuss.LikeCount > 0)
                    {
                        discuss.LikeCount--;
                        _context.Discusses.Update(discuss);
                    }
                }

                // Lưu thay đổi
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await transaction.RollbackAsync();
                throw; // Log lỗi nếu cần
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
