using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Services.PaymentServices
{
    public interface IPaymentService
    {
        bool ProcessPayment(Guid userId);

        IEnumerable<RevenueStatistic> GetRevenueStatisticsForCurrentMonth();
    }
}
