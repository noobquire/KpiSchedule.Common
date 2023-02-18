using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Entities.RozKpi
{
    [DynamoDBTable("RozKpiTeacherSchedules", LowerCamelCaseProperties = true)]
    public class TeacherScheduleEntity
    {
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string TeacherName { get; set; }

        public IList<RozKpiApiTeacherScheduleDay> FirstWeek { get; set; }

        public IList<RozKpiApiTeacherScheduleDay> SecondWeek { get; set; }
    }
}
