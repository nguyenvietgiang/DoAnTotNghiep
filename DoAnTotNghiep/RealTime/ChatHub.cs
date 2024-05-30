using Microsoft.AspNetCore.SignalR;
using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.RealTime
{
    public class ChatHub : Hub
    {
        private readonly DataContext _context;

        public ChatHub(DataContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var httpContext = Context.GetHttpContext();
            var senderIdString = httpContext.Session.GetString("Accountid");

            if (string.IsNullOrEmpty(senderIdString))
            {
                throw new ArgumentNullException(nameof(senderIdString), "UserIdentifier cannot be null");
            }

            var senderId = Guid.Parse(senderIdString);

            var msg = new Message
            {
                MessageID = Guid.NewGuid(),
                SenderID = senderId,
                ReceiverID = Guid.Parse(receiverId),
                Content = message,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId.ToString(), message);
            await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", senderId.ToString(), message); // Also send to sender
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Session.GetString("Accountid");

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "UserIdentifier cannot be null");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            Context.Items["UserIdentifier"] = userId;

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Session.GetString("Accountid");

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "UserIdentifier cannot be null");
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}



