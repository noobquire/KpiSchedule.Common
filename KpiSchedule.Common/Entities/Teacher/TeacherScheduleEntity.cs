using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Entities.Base;

namespace KpiSchedule.Common.Entities.Teacher
{
    /// <summary>
    /// Teacher schedule DB entity.
    /// </summary>
    [DynamoDBTable("KpiSchedule-TeacherSchedules")]
    public class TeacherScheduleEntity : BaseScheduleEntity<TeacherScheduleDayEntity, TeacherSchedulePairEntity>
    {
        /// <summary>
        /// Teacher name as listed in schedule title.
        /// </summary>
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string TeacherName { get; set; }

        [DynamoDBHashKey]
        public override Guid ScheduleId { get; set; }
    }
}
