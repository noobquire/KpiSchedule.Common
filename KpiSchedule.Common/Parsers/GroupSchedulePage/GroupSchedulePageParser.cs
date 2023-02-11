using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class GroupSchedulePageParser : BaseParser<RozKpiApiGroupSchedule>
    {
        private readonly GroupScheduleWeekTableParser tableParser;

        public GroupSchedulePageParser(ILogger logger, GroupScheduleWeekTableParser tableParser) : base(logger)
        {
            this.tableParser = tableParser;
        }

        public override RozKpiApiGroupSchedule Parse(HtmlNode documentNode)
        {
            var document = documentNode.OwnerDocument;
            var labelHeaderNode = document.GetElementbyId("ctl00_MainContent_lblHeader");
            // Розклад занять для groupName
            var groupName = labelHeaderNode.InnerText.Substring(19);
            logger.Information("Parsing schedule tables for {groupName}", groupName);

            var firstWeekTableNode = document.GetElementbyId("ctl00_MainContent_FirstScheduleTable");
            var firstWeek = tableParser.Parse(firstWeekTableNode);

            var secondWeekTableNode = document.GetElementbyId("ctl00_MainContent_SecondScheduleTable");
            var secondWeek = tableParser.Parse(secondWeekTableNode);

            var schedule = new RozKpiApiGroupSchedule()
            {
                GroupName = groupName,
                FirstWeek = firstWeek,
                SecondWeek = secondWeek
            };
            return schedule;
        }
    }
}
