namespace DoAnTotNghiep.Models.EntityModels
{
    public class Pay
    {
        public Guid ID { get; set; }
        public Guid UserId { get; set; }
        public Account? Account { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PaymentGate { get; set; }
    }
}
