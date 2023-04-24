namespace KpiSchedule.Common.Entities.Base
{
    /// <summary>
    /// Base class for schedule day DB entities.
    /// </summary>
    /// <typeparam name="TPair">Pair type.</typeparam>
    public class BaseScheduleDayEntity<TPair> where TPair : BaseSchedulePairEntity
    {
        public int DayNumber { get; set; }
        public List<TPair> Pairs { get; set; }
    }
}