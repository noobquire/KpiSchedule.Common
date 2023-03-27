using KpiSchedule.Common.Models;

namespace KpiSchedule.Common.Parsers
{
    public static class PairTypeParser
    {
        public static PairType ParsePairType(string pairTypeString)
        {
            if(string.IsNullOrEmpty(pairTypeString))
            {
                return PairType.Lecture;
            }

            if(pairTypeString.Contains("Лек") || pairTypeString.Contains("lecture"))
            {
                return PairType.Lecture;
            }

            if (pairTypeString.Contains("Прак") || pairTypeString.Contains("prac"))
            {
                return PairType.Practicum;
            }

            if (pairTypeString.Contains("Лаб") || pairTypeString.Contains("lab"))
            {
                return PairType.Lab;
            }

            if (pairTypeString.Contains("Сем") || pairTypeString.Contains("Семінар") || pairTypeString.Contains("seminar")) // this does not seem to exist, but appeared in the past
            {
                return PairType.Seminar;
            }

            return PairType.Lecture;
        }
    }
}
