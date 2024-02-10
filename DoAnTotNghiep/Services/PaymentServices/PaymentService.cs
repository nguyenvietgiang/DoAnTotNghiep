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
                    if (account.AccountRole == AccountRole.CandidateFree)
                    {
                        account.AccountRole = AccountRole.CandidatePaid;
                    }
                    else if (account.AccountRole == AccountRole.EmployerFree)
                    {
                        account.AccountRole = AccountRole.EmployerPaid;
                    }
                    else
                    {
                        // Nếu tài khoản không phải là "Free" thì không thực hiện gì cả
                        return false;
                    }

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


        public IEnumerable<RevenueStatistic> GetRevenueStatisticsForCurrentMonth()
        {
            var today = DateTime.Today;
            var revenueStatistics = _context.RevenueStatistics
                .Where(r => r.Date.Year == today.Year && r.Date.Month == today.Month)
                .ToList();

            return revenueStatistics;
        }
    }

}
