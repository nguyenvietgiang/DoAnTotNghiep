using System.Security.Claims;

namespace DoAnTotNghiep.Middleware
{
    public class SessionToClaimsPrincipalMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionToClaimsPrincipalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var accountId = context.Session.GetString("Accountid");

            if (!string.IsNullOrEmpty(accountId))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, accountId)
            };

                var identity = new ClaimsIdentity(claims, "Session");
                context.User = new ClaimsPrincipal(identity);
            }

            await _next(context);
        }
    }

}
