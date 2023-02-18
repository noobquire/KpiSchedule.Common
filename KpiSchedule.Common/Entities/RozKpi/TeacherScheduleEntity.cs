using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities.RozKpi
{
    [DynamoDBTable("RozKpiTeacherSchedules", LowerCamelCaseProperties = true)]
    public class TeacherScheduleEntity
    {
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string TeacherName { get; set; }

        public List<TeacherScheduleDayEntity> FirstWeek { get; set; }

        public List<TeacherScheduleDayEntity> SecondWeek { get; set; }
    }
}
