namespace KpiSchedule.Common.Entities.RozKpi
{
    /// <summary>
    /// Group schedule day DynamoDb entity.
    /// See https://github.com/aws/aws-sdk-net/issues/1162 why property names are named lower case in this class.
    /// </summary>
    public class GroupScheduleDayEntity
    {
        /// <summary>
        /// 1-based day number.
        /// </summary>
        public int dayNumber { get; set; }

        /// <summary>
        /// List of pairs this day.
        /// </summary>
        public List<GroupPairEntity> pairs { get; set; }
    }
}
