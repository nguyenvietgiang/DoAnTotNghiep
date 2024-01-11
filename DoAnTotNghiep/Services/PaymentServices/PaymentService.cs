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

                    // Cập nhật dữ liệu thống kê
                    var today = DateTime.Today;
                    var revenueStatistic = _context.RevenueStatistics
                        .FirstOrDefault(r => r.Date.Year == today.Year && r.Date.Month == today.Month);

                    if (revenueStatistic == null)
                    {
                        revenueStatistic = new RevenueStatistic
                        {
                            Id = Guid.NewGuid(),
                            Date = today,
                            Amount = 50000
                        };
                        _context.RevenueStatistics.Add(revenueStatistic);
                    }
                    else
                    {
                        revenueStatistic.Amount += 50000;
                    }

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
