using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace KpiSchedule.Common.Parsers.TeacherSchedulePage
{
    public class TeacherScheduleWeekTableParser : BaseParser<IList<RozKpiApiTeacherScheduleDay>>
    {
        private readonly TeacherScheduleCellParser cellParser;

        public TeacherScheduleWeekTableParser(ILogger logger, TeacherScheduleCellParser cellParser) : base(logger)
        {
            this.cellParser = cellParser;
        }

        public override IList<RozKpiApiTeacherScheduleDay> Parse(HtmlNode tableNode)
        {
            var pairsCount = GetPairsCount(tableNode);
            var scheduleDays = InitScheduleDays(pairsCount);

            int pairNumber = 0;

            foreach (HtmlNode rowNode in tableNode.SelectNodes("tr"))
            {
                // increment and check if this is first row
                // first row contains day names, pair rows start from second
                if (pairNumber++ == 0) continue;
                LogContext.Push(new PropertyEnricher("pairNumber", pairNumber));
                logger.Verbose("Parsing row {rowNumber}", pairNumber);

                int dayNumber = 0;
                foreach (HtmlNode cellNode in rowNode.SelectNodes("td"))
                {
                    // increment and check if this is first column
                    // first column contains pair start time, pair cells start from second column

                    if (dayNumber == 0)
                    {
                        dayNumber++;
                        continue;
                    }

                    LogContext.Push(new PropertyEnricher("dayNumber", dayNumber));
                    logger.Verbose("Parsing cell {cellNumber}: {cellContents}", dayNumber, cellNode.InnerText);

                    var pairInCell = new RozKpiApiTeacherPair();
                    try
                    {
                        pairInCell = cellParser.Parse(cellNode, dayNumber);
                    }
                    catch (NotImplementedException ex)
                    {
                        logger.Fatal(ex.Message);
                    }

                    var day = scheduleDays[dayNumber - 1];
                    if (pairInCell is not null)
                    {
                        day.Pairs.Add(pairInCell);
                    }

                    dayNumber++;
                }
            }

            return scheduleDays;
        }

        /// <summary>
        /// Initializes a list of 6 empty schedule days.
        /// </summary>
        /// <param name="pairsCount">Count of pairs per day.</param>
        /// <returns>List of schedule days.</returns>
        private IList<RozKpiApiTeacherScheduleDay> InitScheduleDays(int pairsCount)
        {
            var scheduleDays = Enumerable.Range(1, 6).Select(i => new RozKpiApiTeacherScheduleDay()
            {
                DayNumber = i,
            }).ToList();

            logger.Verbose("Initialized empty schedule with {pairsCount} pairs per day", pairsCount);

            return scheduleDays.ToList();
        }

        private static int GetPairsCount(HtmlNode tableNode)
        {
            return tableNode?.SelectNodes("tr")?[0].SelectNodes("td")?.Count ?? 0;
        }
    }
}
