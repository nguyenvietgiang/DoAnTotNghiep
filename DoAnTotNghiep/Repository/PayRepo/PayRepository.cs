using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace DoAnTotNghiep.Repository.PayRepo
{
    public class PayRepository : IPayRepository
    {
        private readonly DataContext _dbContext;

        public PayRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Pay>> GetAllPaymentsAsync()
        {
            return await _dbContext.Pays.Include(p => p.Account).ToListAsync();
        }

        public async Task<List<Pay>> GetPaymentsByUserIdAsync(Guid userId)
        {
            return await _dbContext.Pays.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task AddPaymentAsync(Pay pay)
        {
            _dbContext.Pays.Add(pay);
            await _dbContext.SaveChangesAsync();
        }
    }

}
