namespace KpiSchedule.Common.Models.RozKpiApi.Teacher
{
    public class RozKpiApiTeacher
    {
        /// <summary>
        /// Teacher's short name (usually with initials).
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Teacher's full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Teacher schedule unique identifier, to be used with /Schedules/ViewSchedule.aspx?v=scheduleId
        /// </summary>
        public Guid ScheduleId { get; set; }
    }
}
