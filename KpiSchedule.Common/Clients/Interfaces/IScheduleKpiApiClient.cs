using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.ScheduleKpiApi.Group;
using KpiSchedule.Common.Models.ScheduleKpiApi.Teacher;
using KpiSchedule.Common.Models.ScheduleKpiApi.Time;

namespace KpiSchedule.Common.Clients.Interfaces
{
    /// <summary>
    /// Interface for pulling schedules data from schedule.kpi.ua.
    /// </summary>
    public interface IScheduleKpiApiClient
    {
        /// <summary>
        /// Get list of all groups.
        /// </summary>
        /// <returns>List of all groups.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public Task<ScheduleKpiApiGroupsResponse> GetAllGroups();

        /// <summary>
        /// Get list of all teachers.
        /// </summary>
        /// <returns>List of all groups.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public Task<ScheduleKpiApiTeachersResponse> GetAllTeachers();

        /// <summary>
        /// Get group schedule for specific group.
        /// </summary>
        /// <param name="groupId">Unique group identifier.</param>
        /// <returns>Group schedule.</returns>
        public Task<ScheduleKpiApiGroupScheduleResponse> GetGroupSchedule(string groupId);

        /// <summary>
        /// Get current schedule time info.
        /// </summary>
        /// <returns>Time info.</returns>
        public Task<ScheduleKpiApiTimeResponse> GetTimeInfo();

        /// <summary>
        /// Get teacher schedule for specific teacher.
        /// </summary>
        /// <param name="teacherId">Unique teacher identifier.</param>
        /// <returns>Teacher schedule.</returns>
        public Task<ScheduleKpiApiTeacherScheduleResponse> GetTeacherSchedule(string teacherId);


    }
}
