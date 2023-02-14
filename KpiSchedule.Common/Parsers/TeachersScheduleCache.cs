using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Models.RozKpiApi;
using Microsoft.Extensions.Caching.Memory;

namespace KpiSchedule.Common.Parsers
{
    public class TeachersScheduleCache
    {
        private readonly IMemoryCache memoryCache;
        private readonly RozKpiApiTeachersClient teachersClient;

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
