using HtmlAgilityPack;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using System.Diagnostics;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    internal class GroupScheduleWeekTableParser : BaseParser<IList<RozKpiApiGroupScheduleDay>>
    {
        public GroupScheduleWeekTableParser(HtmlNode tableNode) : base(tableNode)
        {
        }

        public override IList<RozKpiApiGroupScheduleDay> Parse()
        {
            if(!IsDaytimeScheduleTable(this.node))
            {
                throw new KpiScheduleParserException("Not a daytime schedule table. Probably this group is extramural or has no schedule.");
            }

            var scheduleDays = new List<RozKpiApiGroupScheduleDay>();

            // foreach day as column in table
            // nested foreach pair(s) as cell in column
            // 
            foreach (HtmlNode rowNode in node.SelectNodes("tr"))
            {
                Debug.WriteLine("row");
                int day = 0;
                foreach (HtmlNode cellNode in rowNode.SelectNodes("td"))
                {
                    Debug.WriteLine("cell: " + cellNode.InnerText);
                    var scheduleCellScraper = new GroupScheduleCellParser(cellNode);
                    var pairsInCell = scheduleCellScraper.Parse();
                    //scheduleDays.AddRange(pairsInCell);
                    day++;
                }
            }

            return null;
        }

        /// <summary>
        /// Check if schedule table node has expected amount of rows and columns.
        /// </summary>
        /// <param name="tableNode">Schedule table node.</param>
        /// <returns>Boolean indicating if schedule table is valid daytime schedule.</returns>
        private static bool IsDaytimeScheduleTable(HtmlNode tableNode)
        {
            // fulltime group schedule must have
            // 7 or 8 rows (6-7 pairs and one row for day names)
            // 7 columns (6 days and one column for pair time)
            var rowsCount = tableNode?.SelectNodes("tr")?.Count ?? 0;
            var columnsCount = tableNode?.SelectNodes("tr")?[0].SelectNodes("td")?.Count ?? 0;
            return (new[] { 7, 8 }).Contains(rowsCount) && columnsCount == 7;
        }
    }
}
