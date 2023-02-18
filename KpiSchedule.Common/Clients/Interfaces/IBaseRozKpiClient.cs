using HtmlAgilityPack;

namespace KpiSchedule.Common.Clients.Interfaces
{
    /// <summary>
    /// Base interface for roz.kpi.ua clients.
    /// </summary>
    public interface IBaseRozKpiClient
    {
        /// <summary>
        /// Get schedule page using schedule id.
        /// </summary>
        /// <param name="scheduleId">Schedule id.</param>
        /// <param name="type">Schedule type: group or teacher.</param>
        /// <returns>Schedule HTML document.</returns>
        public Task<HtmlDocument> GetSchedulePage(Guid scheduleId, RozKpiApiScheduleType type = RozKpiApiScheduleType.GroupSchedule);
    }
}
