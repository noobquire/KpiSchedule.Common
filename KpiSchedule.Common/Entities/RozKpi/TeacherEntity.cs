namespace KpiSchedule.Common.Entities.RozKpi
{
    /// <summary>
    /// Schedule teacher DynamoDb entity.
    /// See https://github.com/aws/aws-sdk-net/issues/1162 why property names are named lower case in this class.
    /// </summary>
    public class TeacherEntity
    {
        /// <summary>
        /// Teacher's short name (usually with initials).
        /// </summary>
        public string shortName { get; set; }

        /// <summary>
        /// Teacher's full name.
        /// </summary>
        public string fullName { get; set; }

        /// <summary>
        /// Teacher schedule unique identifier
        /// </summary>
        public Guid scheduleId { get; set; }
    }
}
