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

            if(pairTypeString.Contains("Лек"))
            {
                return PairType.Lecture;
            }

            if (pairTypeString.Contains("Прак"))
            {
                return PairType.Practicum;
            }

            if (pairTypeString.Contains("Лаб"))
            {
                return PairType.Lab;
            }

            if (pairTypeString.Contains("Сем") | pairTypeString.Contains("Семінар")) // this is very rare
            {
                return PairType.Seminar;
            }

            return PairType.Lecture;
        }
    }
}
