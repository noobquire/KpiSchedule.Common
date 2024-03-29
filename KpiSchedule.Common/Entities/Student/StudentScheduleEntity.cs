﻿using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Entities.Base;

namespace KpiSchedule.Common.Entities.Student
{
    /// <summary>
    /// DB entity for personal student schedule.
    /// </summary>
    [DynamoDBTable("KpiSchedule-StudentSchedules")]
    public class StudentScheduleEntity : BaseScheduleEntity<StudentScheduleDayEntity, StudentSchedulePairEntity>
    {
        /// <summary>
        /// Boolean indicating if this schedule should be made public to other users.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Owner unique identifier.
        /// </summary>
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string OwnerId { get; set; }

        /// <summary>
        /// Schedule unique identifier.
        /// </summary>
        [DynamoDBHashKey]
        public override Guid ScheduleId { get; set; }

        /// <summary>
        /// Schedule name.
        /// </summary>
        public string ScheduleName { get; set; }
    }
}
