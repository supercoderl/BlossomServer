using MediatR;
using System.Diagnostics;

namespace BlossomServer.Middlewares
{
    public sealed class PerformanceMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMonitoringMiddleware> _logger;

        public PerformanceMonitoringMiddleware(
            RequestDelegate next,
            ILogger<PerformanceMonitoringMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var endpoint = context.GetEndpoint()?.DisplayName ?? context.Request.Path;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                // Log slow requests
                if (elapsed > 1000)
                {
                    _logger.LogWarning("Slow request detected: {Endpoint} took {ElapsedMs}ms",
                        endpoint, elapsed);
                }
                else if (elapsed > 500)
                {
                    _logger.LogInformation("Request: {Endpoint} took {ElapsedMs}ms",
                        endpoint, elapsed);
                }

                // Add response header for debugging
                if (!context.Response.HasStarted)
                {
                    context.Response.Headers.Append("X-Response-Time", $"{elapsed}ms");
                }
            }
        }

        // MediatR Performance Behavior
        public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

            public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
            {
                _logger = logger;
            }

            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                var stopwatch = Stopwatch.StartNew();
                var requestName = typeof(TRequest).Name;

                try
                {
                    var response = await next();
                    return response;
                }
                finally
                {
                    stopwatch.Stop();
                    var elapsed = stopwatch.ElapsedMilliseconds;

                    if (elapsed > 500)
                    {
                        _logger.LogWarning("Long running request: {RequestName} took {ElapsedMs}ms with data {@Request}",
                            requestName, elapsed, request);
                    }
                }
            }
        }
    }
}
