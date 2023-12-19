namespace DoAnTotNghiep.Services.PaymentServices
{
    public interface IPaymentService
    {
        bool ProcessPayment(Guid userId);
    }
}
