namespace KpiSchedule.Common.Models.RozKpiApi
{
    public class BaseRozKpiApiPair
    {
        /// <summary>
        /// 1-based pair number. 
        /// Not unique, if multiple pairs are at the same time.
        /// </summary>
        public int PairNumber { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public PairType Type { get; set; }

        public bool IsOnline { get; set; }

        public RozKpiApiSubject Subject { get; set; }

        public IList<string> Rooms { get; set; }
    }
}
