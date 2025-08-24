using Microsoft.AspNetCore.Authorization;

namespace BlossomServer.Extensions
{
    public class GuestOrAuthenticatedRequirement : IAuthorizationRequirement
    {
    }

    public class GuestOrAuthenticatedHandler : AuthorizationHandler<GuestOrAuthenticatedRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GuestOrAuthenticatedRequirement requirement)
        {
            // Allow if user is authenticated
            if (context.User.Identity?.IsAuthenticated == true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Check for guest access through HttpContext
            if (context.Resource is Microsoft.AspNetCore.Http.HttpContext httpContext)
            {
                // Check query parameter
                if (httpContext.Request.Query.ContainsKey("isGuest"))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                // Check header
                if (httpContext.Request.Headers.ContainsKey("X-Guest-Access"))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                // For SignalR WebSocket connections, check the original HTTP request
                if (httpContext.WebSockets.IsWebSocketRequest)
                {
                    var originalQuery = httpContext.Request.QueryString.Value;
                    if (!string.IsNullOrEmpty(originalQuery) && originalQuery.Contains("isGuest=true"))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
            }

            // If none of the above conditions are met, the requirement fails
            return Task.CompletedTask;
        }
    }
}
