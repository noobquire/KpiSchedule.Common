using KpiSchedule.Common.Entities.Base;
using KpiSchedule.Common.Entities.Teacher;

namespace KpiSchedule.Common.Entities.Student
{
    /// <summary>
    /// Student schedule pair DB entity.
    /// </summary>
    public class StudentSchedulePairEntity : BaseSchedulePairEntity
    {
        /// <summary>
        /// Teacher(s) which conduct this pair.
        /// </summary>
        public List<TeacherEntity> Teachers { get; set; }

        /// <summary>
        /// Link to join the online conference for this pair.
        /// </summary>
        public string OnlineConferenceUrl { get; set; }
    }
}
