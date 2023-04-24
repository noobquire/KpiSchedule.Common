namespace KpiSchedule.Common.Models.RozKpiApi.Teacher
{
    public class RozKpiApiTeacherScheduleDay
    {
        /// <summary>
        /// 1-based day number.
        /// </summary>
        public int DayNumber { get; set; }

        /// <summary>
        /// List of pairs this day.
        /// </summary>
        public IList<RozKpiApiTeacherPair> Pairs { get; set; } = new List<RozKpiApiTeacherPair>();
    }
}
