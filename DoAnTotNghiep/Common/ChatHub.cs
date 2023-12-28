using DoAnTotNghiep.Models.EntityModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Common
{
    public class ChatHub : Hub
    {
        private readonly DataContext _dbContext;

        public ChatHub(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task JoinGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Guid senderId, Guid receiverId, string message)
        {
            var chatMessage = new Message
            {
                SenderID = senderId,
                ReceiverID = receiverId,
                Content = message,
                SentAt = DateTime.UtcNow,
            };

            _dbContext.Messages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            // Gửi tin nhắn chỉ đến người nhận
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", chatMessage);

            // Gửi tin nhắn tới người gửi để cập nhật UI người gửi (nếu cần)
            await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", chatMessage);
        }

        public async Task GetMessages(Guid senderId, Guid receiverId)
        {
            var messages = await _dbContext.Messages
                .Where(m => (m.SenderID == senderId && m.ReceiverID == receiverId) ||
                            (m.SenderID == receiverId && m.ReceiverID == senderId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }
    }
}
