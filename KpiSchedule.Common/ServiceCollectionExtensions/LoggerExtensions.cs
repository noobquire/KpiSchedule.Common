using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Events;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    /// <summary>
    /// IServiceCollection extensions for logging.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Add Serilog logger with console sink.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddSerilogConsoleLogger(this IServiceCollection services, LogEventLevel minimumLogLevel = LogEventLevel.Information)
        {
            var consoleMessageTemplate = "[{Level:w3}] {groupName} d{dayNumber}p{pairNumber}: {Message:l}{NewLine}";
            services.AddScoped<ILogger>(c =>
                new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Information()
                    .WriteTo.Console(outputTemplate: consoleMessageTemplate)
                    .CreateLogger()
            );

            return services;
        }

        public static IServiceCollection AddSerilogLogFile(this IServiceCollection services, LogEventLevel minimumLogLevel = LogEventLevel.Information)
        {
            var fileMessageTemplate = "{Timestamp:HH:mm:ss} {Level:w3} {groupName} d{dayNumber}p{pairNumber}: {Message:l}{NewLine}";
            services.AddScoped<ILogger>(c =>
                new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Information()
                    .WriteTo.File($"logs/group-schedule-scraper-{DateTime.Now.ToString("yyyy-MM-dd_HH-mm")}.log", outputTemplate: fileMessageTemplate)
                    .CreateLogger()
            );

            return services;
        }
    }
}
