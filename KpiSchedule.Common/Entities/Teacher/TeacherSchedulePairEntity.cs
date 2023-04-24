using KpiSchedule.Common.Entities.Base;
using KpiSchedule.Common.Entities.Group;

namespace KpiSchedule.Common.Entities.Teacher
{
    /// <summary>
    /// Teacher pair DB entity.
    /// </summary>
    public class TeacherSchedulePairEntity : BaseSchedulePairEntity
    {
        /// <summary>
        /// Groups for which this pair is conducted.
        /// </summary>
        public List<GroupEntity> Groups { get; set; }
    }
}
