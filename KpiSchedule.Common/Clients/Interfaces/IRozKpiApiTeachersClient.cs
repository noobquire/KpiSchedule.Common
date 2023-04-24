using KpiSchedule.Common.Exceptions;
using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi.Teacher;

namespace KpiSchedule.Common.Clients.Interfaces
{
    /// <summary>
    /// Interface for pulling teacher schedules data from roz.kpi.ua.
    /// </summary>
    public interface IRozKpiApiTeachersClient : IBaseRozKpiClient
    {
        /// <summary>
        /// Get list of teacher names starting with specified prefix.
        /// </summary>
        /// <param name="teacherNamePrefix">Teacher name prefix.</param>
        /// <returns>List of teacher names with specified prefix.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public Task<RozKpiApiTeachersList> GetTeachers(string teacherNamePrefix);

        /// <summary>
        /// Get teacher schedule selection HTML page.
        /// </summary>
        /// <returns>Teacher schedule selection HTML page.</returns>
        public Task<HtmlDocument> GetTeacherSelectionPage();

        /// <summary>
        /// Get teacher schedule ID for given teacher name.
        /// </summary>
        /// <param name="teacherName">Teacher name.</param>
        /// <returns>Teacher schedule id.</returns>
        public Task<Guid> GetTeacherScheduleId(string teacherName);

        /// <summary>
        /// Get parsed <see cref="RozKpiApiTeacherSchedule"/> for given teacher schedule id.
        /// </summary>
        /// <param name="teacherScheduleId">Teacher schedule id.</param>
        /// <returns>Parsed teacher schedule.</returns>
        public Task<RozKpiApiTeacherSchedule> GetTeacherSchedule(Guid teacherScheduleId);
    }
}
