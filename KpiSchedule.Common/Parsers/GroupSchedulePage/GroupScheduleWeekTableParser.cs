using HtmlAgilityPack;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class GroupScheduleWeekTableParser : BaseParser<IList<RozKpiApiGroupScheduleDay>>
    {
        private readonly GroupScheduleCellParser cellParser;
        public GroupScheduleWeekTableParser(ILogger logger, GroupScheduleCellParser cellParser) : base(logger)
        {
            this.cellParser = cellParser;
        }

        public override IList<RozKpiApiGroupScheduleDay> Parse(HtmlNode tableNode)
        {
            if (!IsDaytimeScheduleTable(tableNode))
            {
                logger.Error("Encountered non-expected table size for a daytime schedule, aborting");
                throw new KpiScheduleParserException("Not a daytime schedule table.");
            }

            var pairsCount = GetPairsCount(tableNode);
            var scheduleDays = InitScheduleDays(pairsCount);

            int pairNumber = 0;
            foreach (HtmlNode rowNode in tableNode.SelectNodes("tr"))
            {
                // increment and check if this is first row
                // first row contains day names, pair rows start from second
                if (pairNumber++ == 0) continue;
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

                    logger.Verbose("Parsing cell {cellNumber}: {cellContents}", dayNumber, cellNode.InnerText);
                    var pairsInCell = cellParser.Parse(cellNode, dayNumber).ToList();

                    foreach (var pair in pairsInCell)
                    {
                        var day = scheduleDays[dayNumber - 1];
                        day.Pairs.Add(pair);
                    }

                    dayNumber++;
                }
            }

            return scheduleDays;
        }

        private static int GetPairsCount(HtmlNode tableNode)
        {
            return tableNode?.SelectNodes("tr")?[0].SelectNodes("td")?.Count ?? 0;
        }

        /// <summary>
        /// Initializes a list of 6 empty schedule days.
        /// </summary>
        /// <param name="pairsCount">Count of pairs per day.</param>
        /// <returns>List of schedule days.</returns>
        private IList<RozKpiApiGroupScheduleDay> InitScheduleDays(int pairsCount)
        {
            var scheduleDays = Enumerable.Range(1, 6).Select(i => new RozKpiApiGroupScheduleDay()
            {
                DayNumber = i,
            }).ToList();

            logger.Verbose("Initialized empty schedule with {pairsCount} pairs per day", pairsCount);

            return scheduleDays.ToList();
        }

        /// <summary>
        /// Check if schedule table node has expected amount of rows and columns.
        /// </summary>
        /// <param name="tableNode">Schedule table node.</param>
        /// <returns>Boolean indicating if schedule table is valid daytime schedule.</returns>
        private bool IsDaytimeScheduleTable(HtmlNode tableNode)
        {
            // fulltime group schedule must have
            // 7 or 8 rows (6-7 pairs and one row for day names)
            // 7 columns (6 days and one column for pair time)
            var rowsCount = tableNode?.SelectNodes("tr")?.Count ?? 0;
            var columnsCount = tableNode?.SelectNodes("tr")?[0].SelectNodes("td")?.Count ?? 0;
            logger.Verbose("Table has {rowsCount} rows and {columnsCount} columns", rowsCount, columnsCount);
            return (new[] { 7, 8 }).Contains(rowsCount) && columnsCount == 7;
        }
    }
}
