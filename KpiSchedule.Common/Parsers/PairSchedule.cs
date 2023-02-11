namespace KpiSchedule.Common.Parsers
{
    public static class PairSchedule
    {
        /// <summary>
        /// Get pair start and end time by its number.
        /// </summary>
        /// <param name="pairNumber">1-based pair number.</param>
        /// <returns>Pair start and end time.</returns>
        public static (TimeOnly pairStart, TimeOnly pairEnd) GetPairStartAndEnd(int pairNumber)
        {
            return pairNumber switch
            {
                1 => (new TimeOnly(8, 30), new TimeOnly(10, 5)),
                2 => (new TimeOnly(10, 25), new TimeOnly(12, 0)),
                3 => (new TimeOnly(12, 20), new TimeOnly(13, 55)),
                4 => (new TimeOnly(14, 15), new TimeOnly(15, 50)),
                5 => (new TimeOnly(16, 10), new TimeOnly(17, 45)),
                6 => (new TimeOnly(18, 30), new TimeOnly(20, 5)),
                _ => (new TimeOnly(0, 0), new TimeOnly(0, 0))
            };
        }
    }
}
