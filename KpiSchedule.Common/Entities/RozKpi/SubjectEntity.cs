namespace KpiSchedule.Common.Entities.RozKpi
{
    /// <summary>
    /// Schedule subject DynamoDb entity.
    /// See https://github.com/aws/aws-sdk-net/issues/1162 why property names are named lower case in this class.
    /// </summary>
    public class SubjectEntity
    {
        public string subjectName { get; set; }

        public string subjectFullName { get; set; }
    }
}
