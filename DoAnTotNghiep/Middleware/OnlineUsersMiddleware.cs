namespace DoAnTotNghiep.Middleware
{
    public class OnlineUsersMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _onlineUsersCount;

        public OnlineUsersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Tăng số người truy cập mỗi khi có yêu cầu mới
            Interlocked.Increment(ref _onlineUsersCount);

            // Chuyển xử lý tới middleware tiếp theo trong pipeline
            await _next(context);

            // Giảm số người truy cập khi yêu cầu hoàn thành
            Interlocked.Decrement(ref _onlineUsersCount);
        }

        public static int GetOnlineUsersCount()
        {
            return _onlineUsersCount;
        }
    }

}
