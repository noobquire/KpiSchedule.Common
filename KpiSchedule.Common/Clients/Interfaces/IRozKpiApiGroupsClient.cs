using KpiSchedule.Common.Models.RozKpiApi;
using KpiSchedule.Common.Exceptions;
using HtmlAgilityPack;

namespace KpiSchedule.Common.Clients.Interfaces
{
    /// <summary>
    /// Interface for pulling group schedules data from roz.kpi.ua.
    /// </summary>
    public interface IRozKpiApiGroupsClient : IBaseRozKpiClient
    {
        /// <summary>
        /// Get list of group names starting with specified prefix.
        /// </summary>
        /// <param name="groupPrefix">Group prefix.</param>
        /// <returns>List of groups with specified prefix.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public Task<RozKpiApiGroupsList> GetGroups(string groupPrefix);

        /// <summary>
        /// Get group schedule selection HTML page.
        /// </summary>
        /// <returns>Schedule group selection HTML page.</returns>
        public Task<HtmlDocument> GetGroupSelectionPage();

        /// <summary>
        /// Get group schedule IDs for given group name.
        /// If conflicting group names are found, return all of their IDs.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <returns>Group schedule id, all ids if got group name conflict.</returns>
        public Task<IEnumerable<Guid>> GetGroupScheduleIds(string groupName);

        /// <summary>
        /// Get parsed <see cref="RozKpiApiGroupSchedule"/> for given group schedule id.
        /// </summary>
        /// <param name="groupScheduleId">Group schedule id.</param>
        /// <returns>Parsed group schedule.</returns>
        public Task<RozKpiApiGroupSchedule> GetGroupSchedule(Guid groupScheduleId);
    }
}
