using KpiSchedule.Common.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    /// <summary>
    /// IServiceCollection extensions for KPI Groups API client.
    /// </summary>
    public static class KpiGroupsClientExtensions
    {
        /// <summary>
        /// Add a scoped instance of KPI Groups API client to the service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="config">Application </param>
        /// <returns></returns>
        public static IServiceCollection AddKpiGroupsClient(this IServiceCollection services, IConfiguration config)
        {
            var baseUrl = config.GetSection("KpiGroupsClient").GetSection("Url").Value;

            services.AddHttpClient(nameof(KpiGroupsClient), c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

            services.AddScoped<KpiGroupsClient>();

            return services;
        }
    }
}
