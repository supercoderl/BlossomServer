using Aikido.Zen.DotNetCore;
using BlossomServer;
using BlossomServer.Application.Extensions;
using BlossomServer.Application.gRPC;
using BlossomServer.Application.Hubs;
using BlossomServer.Domain.Consumers;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Extensions;
using BlossomServer.Domain.Interfaces.BackgroundServices;
using BlossomServer.Domain.Settings;
using BlossomServer.Extensions;
using BlossomServer.HealthChecks;
using BlossomServer.Infrastructure.Database;
using BlossomServer.Infrastructure.Extensions;
using BlossomServer.Middlewares;
using BlossomServer.ServiceDefaults;
using BlossomServer.SharedKernel.Models;
using BlossomServer.SharedKernel.Utils;
using Hangfire;
using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static BlossomServer.Middlewares.PerformanceMonitoringMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsProduction())
{
    builder.Services.AddZenFirewall();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
    });
}

var isAspire = builder.Configuration["ASPIRE_ENABLED"] == "true";

var rabbitConfiguration = builder.Configuration.GetRabbitMqConfiguration();
var redisConnectionString =
    isAspire ? builder.Configuration["ConnectionStrings:Redis"] : builder.Configuration["RedisHostName"];
var dbConnectionString = isAspire
    ? builder.Configuration["ConnectionStrings:Database"]
    : builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services
    .AddHealthChecks()
    .AddCheck<DatabaseWarmupHealthCheck>("database-warmup",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
        tags: new[] { "database", "warmup" })
    .AddDbContextCheck<ApplicationDbContext>()
    .AddApplicationStatus()
    .AddSqlServer(dbConnectionString!)
    .AddRedis(redisConnectionString!, "Redis")
    .AddRabbitMQ(
        rabbitConnectionString: rabbitConfiguration.ConnectionString,
        name: "RabbitMQ");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(dbConnectionString, b =>
    {
        b.MigrationsAssembly("BlossomServer.Infrastructure");
        b.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null
        );
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy", x =>
        x.SetIsOriginAllowed(x => _ = true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddSwagger();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddCSRFProtection(builder.Environment);
builder.Services.AddInfrastructure("BlossomServer.Infrastructure", dbConnectionString!);
builder.Services.AddQueryHandlers();
builder.Services.AddServices();
builder.Services.AddSortProviders();
builder.Services.AddCommandHandlers();
builder.Services.AddNotificationHandlers();
builder.Services.AddApiUser();
builder.Services.AddHttpClient();
builder.Services.AddScoped<OAuthHelper>();
builder.Services.AddNotificationHandlersApplication();
builder.Services.AddSettings<BunnyCDNSettings>(builder.Configuration, "BunnyCDN");
builder.Services.AddSettings<ImageKitSettings>(builder.Configuration, "ImageKit");
builder.Services.AddSettings<GroqSettings>(builder.Configuration, "Groq");
builder.Services.AddSettings<TwillioSettings>(builder.Configuration, "Twillio");
builder.Services.AddSettings<MailSettings>(builder.Configuration, "EmailConfiguration");
builder.Services.AddSettings<ClientSettings>(builder.Configuration, "Client");
builder.Services.Configure<Dictionary<string, OAuthProviderOptions>>(
    builder.Configuration.GetSection("OAuthProviders"));
builder.Services.AddBackground();
builder.Services.AddTriggerBasedAuditing();
builder.Services.AddHangfire(builder.Configuration);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<FanoutEventConsumer>();
    x.AddConsumer<BookingCreatedEventConsumer>();
    x.AddConsumer<UserSubscribedEventConsumer>();
    x.AddConsumer<ResponseContactEventConsumer>();
    x.AddConsumer<CreateFileUploadedEventConsumer>();
    x.AddConsumer<DeleteOldFileUploadedEventConsumer>();
    x.AddConsumer<SendEmailReminderConsumer>();
    x.AddConsumer<NotificationRequiredEventConsumer>();
    x.AddConsumer<EmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureNewtonsoftJsonSerializer(settings =>
        {
            settings.TypeNameHandling = TypeNameHandling.Objects;
            settings.NullValueHandling = NullValueHandling.Ignore;
            return settings;
        });
        cfg.UseNewtonsoftJsonSerializer();
        cfg.ConfigureNewtonsoftJsonDeserializer(settings =>
        {
            settings.TypeNameHandling = TypeNameHandling.Objects;
            settings.NullValueHandling = NullValueHandling.Ignore;
            return settings;
        });

        cfg.Host(rabbitConfiguration.Host, (ushort)rabbitConfiguration.Port, rabbitConfiguration.VirtualHost, h =>
        {
            h.Username(rabbitConfiguration.Username);
            h.Password(rabbitConfiguration.Password);
        });

        // Every instance of the service will receive the message
        cfg.ReceiveEndpoint("blossom-server-fanout-event-" + Guid.NewGuid(), e =>
        {
            e.Durable = false;
            e.AutoDelete = true;
            e.ConfigureConsumer<FanoutEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("blossom-server-bookings", e =>
        {
            e.ConfigureConsumer<BookingCreatedEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("blossom-server-subscribers", e =>
        {
            e.ConfigureConsumer<UserSubscribedEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("blossom-server-response", e =>
        {
            e.ConfigureConsumer<ResponseContactEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("blossom-server-create-file", e =>
        {
            e.ConfigureConsumer<CreateFileUploadedEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("blossom-server-delete-file", e =>
        {
            e.ConfigureConsumer<DeleteOldFileUploadedEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("blossom-server-create-notification", e =>
        {
            e.ConfigureConsumer<NotificationRequiredEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("blossom-server-email-reminder", e =>
        {
            e.ConfigureConsumer<SendEmailReminderConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("blossom-server-emails", e =>
        {
            e.Durable = true; // Keep emails durable for reliability
            e.ConfigureConsumer<EmailConsumer>(context);

            // Configure retry policy for failed emails
            e.UseMessageRetry(r => r.Intervals(
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(30),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5)
            ));

            // Optional: Configure circuit breaker
            e.UseCircuitBreaker(cb =>
            {
                cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                cb.TripThreshold = 15;
                cb.ActiveThreshold = 10;
                cb.ResetInterval = TimeSpan.FromMinutes(5);
            });
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
});

builder.Services.AddLogging(x => x.AddSimpleConsole(console =>
{
    console.TimestampFormat = "[yyyy-MM-ddTHH:mm:ss.fff] ";
    console.IncludeScopes = true;
}));

builder.Services.AddSignalR();

if (builder.Environment.IsProduction() || !string.IsNullOrWhiteSpace(redisConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
        options.InstanceName = "blossom";
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
    options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.BottomLeft;
    options.PopupShowTimeWithChildren = true;
}).AddEntityFramework();

var app = builder.Build();

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var appDbContext = service.GetRequiredService<ApplicationDbContext>();
    var storeDbContext = service.GetRequiredService<EventStoreDbContext>();
    var domainStoreDbContext = service.GetRequiredService<DomainNotificationStoreDbContext>();

    appDbContext.EnsureMigrationsApplied();
    storeDbContext.EnsureMigrationsApplied();
    domainStoreDbContext.EnsureMigrationsApplied();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapGrpcReflectionService();
app.UseMiddleware<PerformanceMonitoringMiddleware>();

app.UseHttpsRedirection();

app.UseCors("policy");

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.UseMiniProfiler();

if (builder.Environment.IsProduction())
{
    app.UseZenFirewall();
}

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

// Schedule recurring job to process email reminders
RecurringJob.AddOrUpdate<IEmailReminderBackgroundService>(
    "process-email-reminders",
    service => service.ProcessEmailRemindersAsync(),
    Cron.Daily(9)); // Run daily at 9 AM - adjust as needed

app.MapControllers();
app.MapGrpcService<UsersApiImplementation>();

app.MapHub<TrackerHub>("/trackerHub");

await app.RunAsync();
//app.Run("http://0.0.0.0:80"); Change port here to EXPOSE in docker

// Needed for integration tests web application factory
public partial class Program
{
}