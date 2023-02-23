using KpiSchedule.Common.Entities;

namespace KpiSchedule.Common.Repositories.Interfaces
{
    /// <summary>
    /// Interface for getting group schedules data from DB.
    /// </summary>
    public interface IGroupSchedulesRepository : ISchedulesRepository<GroupScheduleEntity, GroupScheduleDayEntity, GroupSchedulePairEntity>
    {
        /// <summary>
        /// Lookup group schedule IDs for groups starting with specified prefix.
        /// </summary>
        /// <param name="groupNamePrefix">Group name prefix to search for.</param>
        /// <returns>List of groups starting with specified prefix.</returns>
        Task<IEnumerable<GroupScheduleSearchResult>> SearchGroupSchedules(string groupNamePrefix);

        /// <summary>
        /// Get unique teachers in this group's schedule.
        /// </summary>
        /// <param name="groupScheduleId">Group schedule unique identifier.</param>
        /// <returns>Teachers in group schedule.</returns>
        Task<IEnumerable<TeacherEntity>> GetTeachersInGroupSchedule(Guid groupScheduleId);
    }
}
