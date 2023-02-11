﻿using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
        public static IServiceCollection AddSerilogConsoleLogger(this IServiceCollection services)
        {
            services.AddScoped<ILogger>(c => 
                new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger()
            );

            return services;
        }
    }
}