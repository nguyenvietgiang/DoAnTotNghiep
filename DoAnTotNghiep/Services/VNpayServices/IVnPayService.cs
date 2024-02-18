using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.ResponseDTO;

namespace DoAnTotNghiep.Services.VNpayServices
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
