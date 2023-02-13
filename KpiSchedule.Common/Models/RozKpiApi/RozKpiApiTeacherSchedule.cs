namespace KpiSchedule.Common.Models.RozKpiApi
{
    public class RozKpiApiTeacherSchedule
    {
        public string TeacherName { get; set; }
        public IList<RozKpiApiTeacherScheduleDay> FirstWeek { get; set; }
        public IList<RozKpiApiTeacherScheduleDay> SecondWeek { get; set; }
    }
}
