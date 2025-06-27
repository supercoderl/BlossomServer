using BlossomServer.gRPC.Contexts;
using BlossomServer.gRPC.Interfaces;
using BlossomServer.gRPC.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlossomServer.gRPC.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionKey = "gRPC"
        )
        {
            var settings = new GRPCSettings();
            configuration.Bind(configSectionKey, settings);

            return AddGrpcClient(services, settings);
        }

        private static IServiceCollection AddGrpcClient(this IServiceCollection services, GRPCSettings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.BlossomUrl))
            {
                services.AddBlossomGrpcClient(settings.BlossomUrl);
            }

            services.AddSingleton<IBlossomServer, BlossomServer>();

            return services;
        }

        public static IServiceCollection AddBlossomGrpcClient(
            this IServiceCollection services,
            string gRPCUrl
        )
        {
            if (string.IsNullOrWhiteSpace(gRPCUrl))
            {
                return services;
            }

            var channel = GrpcChannel.ForAddress(gRPCUrl);

/*            var usersClient = new UsersApi.UsersApiClient(channel);
            services.AddSingleton(usersClient);*/

            services.AddSingleton<IUsersContext, UsersContext>();

            return services;
        }
    }
}
