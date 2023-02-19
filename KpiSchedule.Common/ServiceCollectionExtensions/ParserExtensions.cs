using KpiSchedule.Common.Parsers;
using KpiSchedule.Common.Parsers.GroupSchedulePage;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using KpiSchedule.Common.Parsers.TeacherSchedulePage;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.ServiceCollectionExtensions
{
    public static class ParserExtensions
    {
        public static IServiceCollection AddRozKpiParsers(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<TeachersScheduleCache>();

            services.AddScoped<GroupScheduleCellParser>();
            services.AddScoped<GroupSchedulePageParser>();
            services.AddScoped<GroupScheduleWeekTableParser>();
            services.AddScoped<FormValidationParser>();
            services.AddScoped<ConflictingGroupNamesParser>();
            services.AddScoped<PairInfoInGroupScheduleCellParser>();
            services.AddScoped<TeachersInGroupScheduleCellParser>();
            services.AddScoped<GroupSchedulePairDataGroupper>();

            services.AddScoped<TeacherSchedulePageParser>();
            services.AddScoped<TeacherScheduleWeekTableParser>();
            services.AddScoped<TeacherScheduleCellParser>();
            services.AddScoped<PairInfoInTeacherScheduleCellParser>();
            return services;
        }
    }
}
