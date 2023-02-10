namespace KpiSchedule.Common.Models.RozKpiApi
{
    public class RozKpiApiGroupSchedule
    {
        public string GroupName { get; set; }
        public IList<RozKpiApiGroupScheduleDay> FirstWeek { get; set; }
        public IList<RozKpiApiGroupScheduleDay> SecondWeek { get; set; }
    }
}
