using KpiSchedule.Common.Clients.RozKpiApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    /// <summary>
    /// IServiceCollection extensions for schedule.kpi.ua Lecturers API client.
    /// </summary>
    public static class ScheduleKpiLecturersClientExtensions
    {
        /// <summary>
        /// Add a scoped instance of schedule.kpi.ua KPI Lecturers API client to the service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="config">Application </param>
        /// <returns></returns>
        public static IServiceCollection AddScheduleKpiLecturersClient(this IServiceCollection services, IConfiguration config)
        {
            var baseUrl = config.GetSection("ScheduleKpiLecturersClient").GetSection("Url").Value;

            services.AddHttpClient(nameof(ScheduleKpiLecturersClient), c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

            services.AddScoped<ScheduleKpiLecturersClient>();

            return services;
        }
    }
}
