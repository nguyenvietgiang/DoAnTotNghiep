using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.AccountRepo
{
    public class AccountRepository : IAccountRepository
    { 
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task UpdateAccountStatusAsync(Guid userId)
        {
            var account = await _context.Accounts.FindAsync(userId);

            if (account != null)
            {
                account.Status = !account.Status;
                await _context.SaveChangesAsync();
            }
        }
    }

}
