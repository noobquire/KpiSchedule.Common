using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Entities.RozKpi
{
    [DynamoDBTable("RozKpiGroupSchedules", LowerCamelCaseProperties = true)]
    public class GroupScheduleEntity
    {
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string GroupName { get; set; }

        public IList<RozKpiApiGroupScheduleDay> FirstWeek { get; set; }

        public IList<RozKpiApiGroupScheduleDay> SecondWeek { get; set; }

        public GroupScheduleEntity()
        {
        }

        public GroupScheduleEntity(RozKpiApiGroupSchedule rozKpiSchedule)
        {
            this.ScheduleId = rozKpiSchedule.ScheduleId;
            this.FirstWeek = rozKpiSchedule.FirstWeek;
            this.SecondWeek = rozKpiSchedule.SecondWeek;
            this.GroupName = rozKpiSchedule.GroupName;
        }
    }
}
