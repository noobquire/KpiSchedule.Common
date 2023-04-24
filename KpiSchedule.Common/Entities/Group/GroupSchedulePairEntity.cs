using KpiSchedule.Common.Entities.Base;
using KpiSchedule.Common.Entities.Teacher;

namespace KpiSchedule.Common.Entities.Group
{
    /// <summary>
    /// Group schedule pair DB entity.
    /// </summary>
    public class GroupSchedulePairEntity : BaseSchedulePairEntity
    {
        /// <summary>
        /// Teacher(s) which conduct this pair.
        /// </summary>
        public List<TeacherEntity> Teachers { get; set; }
    }
}