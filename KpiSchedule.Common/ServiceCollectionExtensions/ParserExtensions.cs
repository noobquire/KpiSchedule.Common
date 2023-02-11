using KpiSchedule.Common.Parsers.GroupSchedulePage;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    public static class ParserExtensions
    {
        public static IServiceCollection AddRozKpiParsers(this IServiceCollection services)
        {
            services.AddScoped<GroupScheduleCellParser>();
            services.AddScoped<GroupSchedulePageParser>();
            services.AddScoped<GroupScheduleWeekTableParser>();
            services.AddScoped<FormValidationParser>();
            services.AddScoped<ConflictingGroupNamesParser>();
            return services;
        }
    }
}
