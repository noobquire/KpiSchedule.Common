namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// Pair identifier.
    /// </summary>
    public class PairIdentifier
    {
        /// <summary>
        /// Week number, 1 or 2
        /// </summary>
        public int WeekNumber { get; set; }

        /// <summary>
        /// 1-based day number.
        /// </summary>
        public int DayNumber { get; set; }

        /// <summary>
        /// 1-based pair number as in schedule.
        /// </summary>
        public int PairNumber { get; set; }

        /// <summary>
        /// 1-based number of the duplicate pair if there are multiple pairs conducted at the same time.
        /// 1 if there is only single pair.
        /// </summary>
        public int DuplicatePairNumber { get; set; } = 1;
    }
}