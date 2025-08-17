using BlossomServer.Domain.Notifications;
using BlossomServer.HealthChecks;
using BlossomServer.Infrastructure.BackgroundServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public sealed class MaintenanceController : ApiController
    {
        private readonly DatabaseWarmupService _warmupService;

        public MaintenanceController(
            INotificationHandler<DomainNotification> notifications,
            DatabaseWarmupService warmupService
        ) : base(notifications)
        {
            _warmupService = warmupService;
        }

        [HttpPost("warmup")]
        public async Task<IActionResult> WarmupDatabase()
        {
            await _warmupService.StartAsync(CancellationToken.None);
            return Ok("Database warmed up successfully");
        }

        [HttpPost("api/maintenance/warmup")]
        public IActionResult TriggerWarmup()
        {
            DatabaseWarmupHealthCheck.RequireWarmup();
            // Health check will perform warmup on next check
            return Ok("Warmup triggered");
        }
    }
}
