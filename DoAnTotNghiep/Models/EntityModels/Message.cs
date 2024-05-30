using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Message
    {
        [Key]
        public Guid MessageID { get; set; }

        [Required]
        public Guid SenderID { get; set; }

        [Required]
        public Guid ReceiverID { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
