using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Models.DTO
{
    public class ChatViewModel
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public List<Message> Messages { get; set; }
    }
}
