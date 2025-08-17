using BlossomServer.Application.Interfaces;
using BlossomServer.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Behaviors
{
    public class AuditContextPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IContextInfoService _contextService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUser _user;

        public AuditContextPipelineBehavior(
            IContextInfoService contextService,
            IHttpContextAccessor httpContextAccessor,
            IUser user)
        {
            _contextService = contextService;
            _httpContextAccessor = httpContextAccessor;
            _user = user;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = _user.GetUserId().ToString();

            // Set context for triggers
            if (!string.IsNullOrEmpty(userId))
            {
                await _contextService.SetContextAsync(
                    userId,
                    httpContext?.Connection?.RemoteIpAddress?.ToString(),
                    httpContext?.Request?.Headers["User-Agent"],
                    httpContext?.TraceIdentifier
                );
            }

            try
            {
                return await next();
            }
            finally
            {
                await _contextService.ClearContextAsync();
            }
        }
    }
}
