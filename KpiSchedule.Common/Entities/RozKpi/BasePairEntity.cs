namespace KpiSchedule.Common.Entities.RozKpi
{
    /// <summary>
    /// Base pair DynamoDb entity.
    /// See https://github.com/aws/aws-sdk-net/issues/1162 why property names are named lower case in this class.
    /// </summary>
    public class BasePairEntity
    {
        /// <summary>
        /// 1-based pair number. 
        /// Not unique, if multiple pairs are at the same time.
        /// </summary>
        public int pairNumber { get; set; }

        public string startTime { get; set; }

        public string endTime { get; set; }

        public string type { get; set; }

        public bool isOnline { get; set; }

        public SubjectEntity subject { get; set; }

        public List<string> rooms { get; set; }
    }
}
