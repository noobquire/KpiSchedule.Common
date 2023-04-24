using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Clients.Interfaces;
using KpiSchedule.Common.Models.RozKpiApi.Teacher;
using Microsoft.Extensions.Caching.Memory;

namespace KpiSchedule.Common.Parsers
{
    /// <summary>
    /// Memory cache for accessing teacher schedules pulled from roz.kpi.ua.
    /// This is used to prevent extra calls to roz.kpi.ua in order to speed up parsing and not overload the website.
    /// </summary>
    public class TeachersScheduleCache
    {
        private readonly IMemoryCache memoryCache;
        private readonly IRozKpiApiTeachersClient teachersClient;

        public TeachersScheduleCache(IMemoryCache memoryCache, IRozKpiApiTeachersClient teachersClient)
        {
            this.memoryCache = memoryCache;
            this.teachersClient = teachersClient;
        }

        public TeachersScheduleCache(IMemoryCache memoryCache, RozKpiApiTeachersClient teachersClient)
        {
            this.memoryCache = memoryCache;
            this.teachersClient = teachersClient;
        }

        public async Task<RozKpiApiTeacherSchedule> GetTeacherSchedule(Guid scheduleId)
        {
            if(!memoryCache.TryGetValue(scheduleId, out RozKpiApiTeacherSchedule schedule))
            {
                schedule = await teachersClient.GetTeacherSchedule(scheduleId);
                memoryCache.Set(scheduleId, schedule);
            }

            return schedule;
        }
    }
}
