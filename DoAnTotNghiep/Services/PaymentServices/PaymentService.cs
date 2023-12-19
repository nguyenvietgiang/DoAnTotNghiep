using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;

namespace DoAnTotNghiep.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly DataContext _context;

        public PaymentService(DataContext context)
        {
            _context = context;
        }

        public bool ProcessPayment(Guid userId)
        {
            try
            {
                var account = _context.Accounts.Find(userId);

                if (account == null)
                {
                    return false;
                }
                else
                {
                    account.AccountRole = AccountRole.CandidatePaid;
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
