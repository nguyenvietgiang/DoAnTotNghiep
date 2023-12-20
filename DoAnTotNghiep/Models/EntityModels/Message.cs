using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Message
    {
        [Key]
        public Guid MessageID { get; set; }

        [Required]
        public Guid SenderID { get; set; }
        public Account Sender { get; set; }
        [Required]

        public Guid ReceiverID { get; set; }
        public Account Receiver { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime SentAt { get; set; }
    }
}
