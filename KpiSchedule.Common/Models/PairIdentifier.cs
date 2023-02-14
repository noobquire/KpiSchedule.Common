namespace KpiSchedule.Common.Models
{
    public class PairIdentifier
    {
        public int WeekNumber { get; set; }
        public int DayNumber { get; set; }
        public int PairNumber { get; set; }

        public PairIdentifier()
        {
        }

        public PairIdentifier(int weekNumber, int dayNumber, int pairNumber)
        {
            WeekNumber = weekNumber;
            DayNumber = dayNumber;
            PairNumber = pairNumber;
        }
    }
}
