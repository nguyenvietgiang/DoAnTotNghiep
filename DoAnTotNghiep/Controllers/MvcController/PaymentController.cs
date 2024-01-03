using DoAnTotNghiep.Common;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Services.ImageServices;
using DoAnTotNghiep.Services.PaymentServices;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService  _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
           _paymentService= paymentService;
        }
        public IActionResult PaymentWithPaypal()
        {
            // Lấy APIContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            // Tạo đối tượng Payment để bắt đầu thanh toán
            var payment = CreatePayment(apiContext);

            // Thực hiện redirect đến trang thanh toán của PayPal
            var redirectUrl = payment.GetApprovalUrl();
            return Redirect(redirectUrl);
        }

        private Payment CreatePayment(APIContext apiContext)
        {
            var userId = GetUserIdFromClaim();
            // Tạo đối tượng Item
            var item = new Item
            {
                name = "Nang cap tai khoan",
                currency = "USD",
                price = "2", // Giá sản phẩm
                quantity = "1",
                sku = "sku"
            };

            var itemList = new ItemList
            {
                items = new List<Item> { item }
            };

            // Tạo đối tượng Transaction
            var transaction = new Transaction
            {
                description = "Thanh toan tai khoan.",
                invoice_number = Guid.NewGuid().ToString(), // Mã hóa đơn duy nhất
                amount = new Amount
                {
                    currency = "USD",
                    total = "2", 
                },
                item_list = itemList
            };

            var transactions = new List<Transaction> { transaction };

            // Tạo đối tượng Payer
            var payer = new Payer
            {
                payment_method = "paypal"
            };

            // Tạo đối tượng Payment
            var payment = new Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactions,
                redirect_urls = new RedirectUrls
                {
                    cancel_url = "https://localhost:7235/Home/NoPermistion", // Đường dẫn khi hủy thanh toán
                    return_url = "https://localhost:7235/Home/Sucess" // Đường dẫn khi thanh toán thành công
                }
            };
            // để tạm thế này nhưng nên maintaint lại phòng khi thanh toán ko thành công
            _paymentService.ProcessPayment(Guid.Parse(userId));
            // Tạo thanh toán
            return payment.Create(apiContext);
        }

        public IActionResult PaymentWithVNPay() 
        {
            return View();
        }
     }
}
