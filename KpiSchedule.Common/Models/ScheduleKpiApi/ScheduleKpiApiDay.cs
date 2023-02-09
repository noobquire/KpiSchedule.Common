using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models.ScheduleKpiApi
{
    /// <summary>
    /// Model for schedule day from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiApiDay
    {
        /// <summary>
        /// Short day of the week name: Пн/Вв/Ср/Чт/Пт/Сб
        /// </summary>
        [JsonPropertyName("day")]
        public string DayName { get; set; }

        /// <summary>
        /// Pairs this day.
        /// </summary>
        public IList<ScheduleKpiApiPair> Pairs { get; set; }
    }
}
