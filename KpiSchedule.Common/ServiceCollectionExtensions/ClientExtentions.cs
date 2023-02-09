using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    /// <summary>
    /// IServiceCollection extensions for KPI API clients.
    /// </summary>
    public static class ClientExtentions
    {
        /// <summary>
        /// Add a scoped instance of KPI API client to the service collection.
        /// </summary>
        /// <typeparam name="TClient">Client type.</typeparam>
        /// <param name="services">Service collection.</param>
        /// <param name="config">Application </param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddKpiClient<TClient>(this IServiceCollection services, IConfiguration config) where TClient : ClientBase
        {
            var clientConfiguration = config.GetSection(typeof(TClient).Name).Get<KpiApiClientConfiguration>();

            services.AddHttpClient(typeof(TClient).Name, c =>
            {
                c.BaseAddress = new Uri(clientConfiguration.Url);
                c.Timeout = TimeSpan.FromSeconds(clientConfiguration.TimeoutSeconds);
            });

            services.AddScoped<TClient>();

            return services;
        }
    }
}
