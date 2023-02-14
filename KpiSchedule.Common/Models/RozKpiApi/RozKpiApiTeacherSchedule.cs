namespace KpiSchedule.Common.Models.RozKpiApi
{
    public class RozKpiApiTeacherSchedule
    {
        public string TeacherName { get; set; }
        public IList<RozKpiApiTeacherScheduleDay> FirstWeek { get; set; }
        public IList<RozKpiApiTeacherScheduleDay> SecondWeek { get; set; }

        public RozKpiApiTeacherPair GetPair(PairIdentifier pairId)
        {
            if(pairId is null)
            {
                throw new ArgumentNullException(nameof(pairId));
            }

            return GetPair(pairId.WeekNumber, pairId.DayNumber, pairId.PairNumber);
        }

        private RozKpiApiTeacherPair GetPair(int weekNumber, int dayNumber, int pairNumber)
        {
            if (!new[] { 1, 2 }.Contains(weekNumber))
            {
                throw new ArgumentException(nameof(weekNumber), "Week number must be either 1 or 2");
            }

            var week = weekNumber == 1 ? this.FirstWeek : this.SecondWeek;

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
