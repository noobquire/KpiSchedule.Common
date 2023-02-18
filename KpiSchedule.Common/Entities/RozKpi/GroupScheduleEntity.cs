using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Entities.RozKpi
{
    [DynamoDBTable("RozKpiGroupSchedules", LowerCamelCaseProperties = true)]
    public class TeacherScheduleEntity
    {
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string TeacherName { get; set; }

        public IList<RozKpiApiTeacherScheduleDay> FirstWeek { get; set; }

        public IList<RozKpiApiTeacherScheduleDay> SecondWeek { get; set; }

        public TeacherScheduleEntity()
        {
        }

        public TeacherScheduleEntity(RozKpiApiTeacherSchedule rozKpiSchedule)
        {
            this.ScheduleId = rozKpiSchedule.ScheduleId;
            this.FirstWeek = rozKpiSchedule.FirstWeek;
            this.SecondWeek = rozKpiSchedule.SecondWeek;
            this.TeacherName = rozKpiSchedule.TeacherName;
        }
    }
}
