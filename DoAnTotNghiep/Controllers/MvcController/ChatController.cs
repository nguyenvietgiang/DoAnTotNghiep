using Microsoft.AspNetCore.Mvc;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Controllers.MvcController;

namespace DoAnTotNghiep.Controllers
{
    public class ChatController : BaseController
    {
        private readonly DataContext _context;

        public ChatController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(Guid id) // id ở đây là CandidateID của người được liên hệ
        {
            var senderId = Guid.Parse(HttpContext.Session.GetString("Accountid"));
            var receiverId = id;

            var chatViewModel = new ChatViewModel
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Messages = _context.Messages
                    .Where(m => (m.SenderID == senderId && m.ReceiverID == receiverId) ||
                                (m.SenderID == receiverId && m.ReceiverID == senderId))
                    .OrderBy(m => m.Timestamp)
                    .ToList()
            };

            return View(chatViewModel);
        }
    }
}
