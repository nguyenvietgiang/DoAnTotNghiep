using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.PayRepo
{
    public interface IPayRepository
    {
        Task<List<Pay>> GetAllPaymentsAsync();
        Task<List<Pay>> GetPaymentsByUserIdAsync(Guid userId);
        Task AddPaymentAsync(Pay pay);
    }
}
