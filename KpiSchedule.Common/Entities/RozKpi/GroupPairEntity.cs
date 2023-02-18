namespace KpiSchedule.Common.Entities.RozKpi
{
    /// <summary>
    /// Group pair DynamoDb entity.
    /// See https://github.com/aws/aws-sdk-net/issues/1162 why property names are named lower case in this class.
    /// </summary>
    public class GroupPairEntity : BasePairEntity
    {
        /// <summary>
        /// List of teachers for this pair.
        /// </summary>
        public List<TeacherEntity> teachers { get; set; }

    }
}