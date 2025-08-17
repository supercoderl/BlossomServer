using Hangfire.Dashboard;

namespace BlossomServer
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }

            // Option 3: IP-based restriction (for production)
            var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString();
            var allowedIPs = new[] { "127.0.0.1", "::1" }; // localhost only

            if (allowedIPs.Contains(remoteIp))
            {
                return true;
            }

            return false;
        }
    }
}
