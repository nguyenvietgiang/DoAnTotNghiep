using DoAnTotNghiep.Common;
using DoAnTotNghiep.Models.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class ChatController : BaseController
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly DataContext _dataContext;

        public ChatController(IHubContext<ChatHub> hubContext, DataContext dataContext)
        {
            _hubContext = hubContext;
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid receiverId)
        {
            var userId = GetUserIdFromClaim();
            var account = await _dataContext.Accounts
                .Include(a => a.SentMessages)
                .Include(a => a.ReceivedMessages)
                .FirstOrDefaultAsync(a => a.UserID == Guid.Parse(userId));

            if (account == null)
            {
                return NotFound();
            }

            // Lấy tin nhắn giữa hai người dùng
            var messages = await _dataContext.Messages
                .Where(m => (m.SenderID == Guid.Parse(userId) && m.ReceiverID == receiverId) ||
                            (m.SenderID == receiverId && m.ReceiverID == Guid.Parse(userId)))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            // Gửi tin nhắn cho người dùng khi họ mở trang chat
            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessages", messages);

            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> GetNewMessages(Guid receiverId)
        {
            var userId = GetUserIdFromClaim();

            // Lấy tin nhắn mới giữa hai người dùng
            var newMessages = await _dataContext.Messages
                .Where(m => m.ReceiverID == Guid.Parse(userId) && m.SenderID == receiverId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            await _dataContext.SaveChangesAsync();

            return Json(newMessages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Guid receiverId, string content)
        {
            var userId = GetUserIdFromClaim();
            var chatMessage = new Message
            {
                SenderID = Guid.Parse(userId),
                ReceiverID = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow,
            };

            _dataContext.Messages.Add(chatMessage);
            await _dataContext.SaveChangesAsync();

            // Gửi tin nhắn tới người nhận
            await _hubContext.Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", chatMessage);

            // Gửi tin nhắn tới người gửi để cập nhật UI người gửi (nếu cần)
            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", chatMessage);

            return Ok();
        }
    }
}

