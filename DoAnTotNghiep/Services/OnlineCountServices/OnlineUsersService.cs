using DoAnTotNghiep.Middleware;

namespace DoAnTotNghiep.Services.OnlineCountServices
{
    public class OnlineUsersService : IOnlineUsersService
    {
        public int GetOnlineUsersCount()
        {
            return OnlineUsersMiddleware.GetOnlineUsersCount();
        }
    }
}
