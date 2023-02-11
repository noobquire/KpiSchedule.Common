using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using System.Diagnostics;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    internal class GroupScheduleWeekTableParser : BaseParser<IList<RozKpiApiGroupScheduleDay>>
    {
        public GroupScheduleWeekTableParser(HtmlNode node) : base(node)
        {
        }

        public override IList<RozKpiApiGroupScheduleDay> Parse()
        {
            // TODO: check if valid daytime schedule
            // Daytime schedule should have 7-8 rows and 7 columns

            var scheduleDays = new List<RozKpiApiGroupScheduleDay>();

            // foreach day as column in table
            // nested foreach pair(s) as cell in column
            // 
            foreach (HtmlNode rowNode in node.SelectNodes("tr"))
            {
                Debug.WriteLine("row");
                int day = 0;
                foreach (HtmlNode cellNode in rowNode.SelectNodes("th|td"))
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
    }
}
