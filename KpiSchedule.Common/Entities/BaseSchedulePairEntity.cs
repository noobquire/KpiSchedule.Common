namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// Base class for schedule pair DB entities.
    /// </summary>
    public class BaseSchedulePairEntity
    {
        /// <summary>
        /// 1-based pair number.
        /// </summary>
        public int PairNumber { get; set; }

        /// <summary>
        /// Pair start time in HH:mm format
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Pair end time in HH:mm format
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Pair type: lec/prac/lab
        /// </summary>
        public string PairType { get; set; }

        /// <summary>
        /// Boolean indicating if pair occurs online.
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Pair subject.
        /// </summary>
        public SubjectEntity Subject { get; set; }

        /// <summary>
        /// Room(s) where pair occurs.
        /// </summary>
        public List<string> Rooms { get; set; }
    }
}