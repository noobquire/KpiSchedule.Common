namespace KpiSchedule.Common.Models.RozKpiApi.Teacher
{
    /// <summary>
    /// Teacher schedule parsed from roz.kpi.ua.
    /// </summary>
    public class RozKpiApiTeacherSchedule
    {
        /// <summary>
        /// Schedule identifier on roz.kpi.ua.
        /// </summary>
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Teacher name with faculty division.
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// First week of the schedule.
        /// </summary>
        public IList<RozKpiApiTeacherScheduleDay> FirstWeek { get; set; }

        /// <summary>
        /// Second week of the schedule.
        /// </summary>
        public IList<RozKpiApiTeacherScheduleDay> SecondWeek { get; set; }

        /// <summary>
        /// Get teacher's pair by identifier.
        /// </summary>
        /// <param name="pairId">Pair identifier (week, day, pair)</param>
        /// <returns>Teacher's pair at specified identifier.</returns>
        /// <exception cref="ArgumentNullException">Pair identifier is null.</exception>
        public RozKpiApiTeacherPair GetPair(PairIdentifier pairId)
        {
            if (pairId is null)
            {
                throw new ArgumentNullException(nameof(pairId));
            }

            return GetPair(pairId.WeekNumber, pairId.DayNumber, pairId.PairNumber);
        }

        /// <summary>
        /// Get teacher's pair by identifier.
        /// </summary>
        private RozKpiApiTeacherPair GetPair(int weekNumber, int dayNumber, int pairNumber)
        {
            if (!new[] { 1, 2 }.Contains(weekNumber))
            {
                throw new ArgumentException(nameof(weekNumber), "Week number must be either 1 or 2");
            }

            var week = weekNumber == 1 ? FirstWeek : SecondWeek;

            if (!Enumerable.Range(1, week.Count).Contains(dayNumber))
            {
                throw new ArgumentException(nameof(dayNumber), $"Day number must be between 1 and {week.Count}");
            }

            var day = week[dayNumber - 1];

            var pairNumbersThisDay = day.Pairs.Select(p => p.PairNumber).Distinct();
            if (!pairNumbersThisDay.Contains(pairNumber))
            {
                throw new ArgumentException(nameof(pairNumber), $"Pair number must be in [{string.Join(", ", pairNumbersThisDay)}]");
            }

            var pair = day.Pairs.First(p => p.PairNumber == pairNumber);

            return pair;
        }
    }
}
