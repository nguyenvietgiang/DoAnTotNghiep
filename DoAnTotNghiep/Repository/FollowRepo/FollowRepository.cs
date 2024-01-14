using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.FollowRepo
{
    public class FollowRepository : IFollowRepository
    {
        private readonly DataContext _context;

        public FollowRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddFollow(Guid userId, Guid followUserId)
        {
            var follow = new Follow
            {
                ID = Guid.NewGuid(),
                UserId = userId,
                FollowUserId = followUserId
            };

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFollow(Guid userId, Guid followUserId)
        {
            var follow = await _context.Follows
                .Where(f => f.UserId == userId && f.FollowUserId == followUserId)
                .FirstOrDefaultAsync();

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetFollowCount(Guid followUserId)
        {
            return await _context.Follows
                .Where(f => f.FollowUserId == followUserId)
                .CountAsync();
        }

        public async Task<bool> IsFollowing(Guid userId, Guid followUserId)
        {
            return await _context.Follows
                .AnyAsync(f => f.UserId == userId && f.FollowUserId == followUserId);
        }
    }
}
