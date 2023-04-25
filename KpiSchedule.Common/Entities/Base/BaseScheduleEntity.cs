using KpiSchedule.Common.Models;

namespace KpiSchedule.Common.Entities.Base
{
    /// <summary>
    /// Base DB entity for schedules.
    /// </summary>
    /// <typeparam name="TScheduleDay">Schedule day type.</typeparam>
    public abstract class BaseScheduleEntity<TScheduleDay, TSchedulePair> where TScheduleDay : BaseScheduleDayEntity<TSchedulePair> where TSchedulePair : BaseSchedulePairEntity
    {
        /// <summary>
        /// Last updated UTC timestamp.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Schedule unique identifier.
        /// </summary>
        public virtual Guid ScheduleId { get; set; }

        /// <summary>
        /// First week of the schedule.
        /// </summary>
        public List<TScheduleDay> FirstWeek { get; set; }

        /// <summary>
        /// Second week of the schedule.
        /// </summary>
        public List<TScheduleDay> SecondWeek { get; set; }

        private void CheckPairId(PairIdentifier pairId)
        {
            if (!new[] { 1, 2 }.Contains(pairId.WeekNumber))
            {
                throw new ArgumentException("Week number must be either 1 or 2", nameof(pairId));
            }

            var week = pairId.WeekNumber == 1 ? FirstWeek : SecondWeek;

            if (!Enumerable.Range(1, week.Count).Contains(pairId.DayNumber))
            {
                throw new ArgumentException($"Day number must be between 1 and {week.Count}", nameof(pairId));
            }

            var day = week[pairId.DayNumber - 1];

            var pairNumbersThisDay = day.Pairs.Select(p => p.PairNumber).Distinct();
            if (!pairNumbersThisDay.Contains(pairId.PairNumber))
            {
                throw new ArgumentException($"Pair number must be in [{string.Join(", ", pairNumbersThisDay)}]", nameof(pairId));
            }

            var pairs = day.Pairs.Where(p => p.PairNumber == pairId.PairNumber);
            if (pairs.Count() < pairId.DuplicatePairNumber || !pairs.Any() || pairId.DuplicatePairNumber < 1)
            {
                var minDuplicatePair = pairs.Any() ? 1 : 0;
                throw new ArgumentException($"Duplicate pair number must be between {minDuplicatePair} and {pairs.Count()}", nameof(pairId));
            }
        }

        /// <summary>
        /// Get schedule pair by Id.
        /// </summary>
        /// <param name="pairId"></param>
        /// <returns></returns>
        public TSchedulePair GetPairById(PairIdentifier pairId)
        {
            CheckPairId(pairId);
            var week = pairId.WeekNumber == 1 ? FirstWeek : SecondWeek;
            var day = week.ElementAt(pairId.DayNumber - 1);
            var pairs = day.Pairs.Where(p => p.PairNumber == pairId.PairNumber);
            var dupPair = pairs.ElementAt(pairId.DuplicatePairNumber - 1);
            return dupPair;
        }

        /// <summary>
        /// Create new or update exisitng schedule pair.
        /// </summary>
        /// <param name="pairId">Pair identifier.</param>
        /// <param name="pairData">Pair data.</param>
        public void UpdateSchedulePair(PairIdentifier pairId, TSchedulePair pairData)
        {
            if (!new[] { 1, 2 }.Contains(pairId.WeekNumber))
            {
                throw new ArgumentException("Week number must be either 1 or 2", nameof(pairId));
            }

            var week = pairId.WeekNumber == 1 ? FirstWeek : SecondWeek;

            if (!Enumerable.Range(1, week.Count).Contains(pairId.DayNumber))
            {
                throw new ArgumentException($"Day number must be between 1 and {week.Count}", nameof(pairId));
            }

            var day = week[pairId.DayNumber - 1];

            // Check only if 0 < pair number < 7 in case we want to create a new pair.
            if (pairId.PairNumber > 6 || pairId.PairNumber < 1)
            {
                throw new ArgumentException($"Pair number must between 1 and 6", nameof(pairId));
            }

            var existingPairs = day.Pairs.Where(p => p.PairNumber == pairId.PairNumber);
            if (!existingPairs.Any())
            {
                // No pairs exist, creating a new one
                day.Pairs.Add(pairData);
                return;
            }

            if (pairId.DuplicatePairNumber < 1)
            {
                throw new ArgumentException($"Duplicate pair number must be > 0", nameof(pairId));
            }

            if (existingPairs.Count() >= pairId.DuplicatePairNumber)
            {
                // Update existing duplicate.
                var existingDuplicatePair = existingPairs.ElementAt(pairId.DuplicatePairNumber - 1);
                day.Pairs.Remove(existingDuplicatePair);
                day.Pairs.Add(pairData); // sorting of pairs must not rely on order in which they were added
            }

            // Create new duplicate.
            day.Pairs.Add(pairData);
        }

        /// <summary>
        /// Deletes a pair from the schedule.
        /// </summary>
        public void RemoveSchedulePair(PairIdentifier pairId)
        {
            CheckPairId(pairId);
            var week = pairId.WeekNumber == 1 ? FirstWeek : SecondWeek;
            var day = week.ElementAt(pairId.DayNumber - 1);
            var pairs = day.Pairs.Where(p => p.PairNumber == pairId.PairNumber);
            var dupPair = pairs.ElementAt(pairId.DuplicatePairNumber - 1);
            day.Pairs.Remove(dupPair);
        }
    }
}
