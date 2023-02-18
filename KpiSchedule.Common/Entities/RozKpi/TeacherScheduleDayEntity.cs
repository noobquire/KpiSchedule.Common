namespace KpiSchedule.Common.Entities.RozKpi
{
    public class TeacherScheduleDayEntity
    {
        /// <summary>
        /// 1-based day number.
        /// </summary>
        public int dayNumber { get; set; }

        /// <summary>
        /// List of pairs this day.
        /// </summary>
        public List<TeacherPairEntity> pairs { get; set; }
    }
}
