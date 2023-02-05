using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    public static class LoggerExtensions
    {
        public static IServiceCollection AddSerilogLogger(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
                builder.AddSerilog(logger);
            });

            return services;
        }
    }
}
