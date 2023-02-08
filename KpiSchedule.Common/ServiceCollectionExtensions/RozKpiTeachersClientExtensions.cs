using KpiSchedule.Common.Clients.RozKpiApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    /// <summary>
    /// IServiceCollection extensions for roz.kpi.ua Groups API client.
    /// </summary>
    public static class RozKpiTeachersClientExtensions
    {
        /// <summary>
        /// Add a scoped instance of roz.kpi.ua KPI Teachers API client to the service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="config">Application </param>
        /// <returns></returns>
        public static IServiceCollection AddRozKpiTeachersClient(this IServiceCollection services, IConfiguration config)
        {
            var baseUrl = config.GetSection("RozKpiTeachersClient").GetSection("Url").Value;

            services.AddHttpClient(nameof(RozKpiTeachersClient), c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

            services.AddScoped<RozKpiTeachersClient>();

            return services;
        }
    }
}
